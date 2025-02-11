﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;
using Auction.Application.Authorization;
using Auction.Application.Extensions;
using Auction.Application.Models;
using Auction.Application.Services.Abstractions;
using Auction.Core.Entities;
using Auction.Core.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Auction.Application.Utils.LoggingMessage;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Auction.Application.Services {

	public class UserService : IUserService {

		private readonly IUnitOfWork workUnit;
		private readonly IMapper mapper;
		private readonly ILogger<UserService> logger;

		public UserService(IUnitOfWork workUnit, IMapper mapper,
			ILogger<UserService> logger) {
			this.workUnit = workUnit;
			this.mapper = mapper;
			this.logger = logger;
		}

		/// <inheritdoc />
		public async Task<IEnumerable<UserDetailedModel>> GetAllAsync() {
			try {
				IEnumerable<User> users = await workUnit.UserRepository.GetAllAsync();
				return mapper.Map<IEnumerable<UserDetailedModel>>(users);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<UserDetailedModel> GetByIdAsync(int id) {
			try {
				User user = await workUnit.UserRepository.GetByIdAsync(id);
				return mapper.Map<UserDetailedModel>(user);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<UserDetailedModel> GetByEmailAsync(string email) {
			try {
				User user = await workUnit.UserRepository.GetByEmailAsync(email);
				return mapper.Map<UserDetailedModel>(user);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<UserDetailedModel> GetAuthorizationInfoByEmailAndPasswordAsync(
			string email, string password) {
			
			try {
				User user = await workUnit.UserRepository.GetAuthorizationInfoByEmailAsync(email);
				if(user == null)
					return null;
				var passwordHash = password.ToPasswordHashBySalt(user.PasswordSalt);
				if(passwordHash != user.PasswordHash)
					return null;
				var result = mapper.Map<UserDetailedModel>(user);
				return result;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<bool> UserExistsAsync(int userId) {
			try {
				return await workUnit.UserRepository.UserExistsAsync(userId);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<bool> UpdatePasswordAsync(PasswordChangeModel model) {
			if(!Validator.TryValidateObject(model, new ValidationContext(model), null, true))
				return false;
			if(model.NewPassword == model.Password)
				return false;
			try {
				User user = await workUnit.UserRepository.GetAuthorizationInfoByEmailAsync(model.Email);
				if(user == null)
					return false;
				var passwordHash = model.Password.ToPasswordHashBySalt(user.PasswordSalt);
				if(passwordHash != user.PasswordHash)
					return false;
				user = new User() {
					Id = user.Id,
					PasswordHash = model.NewPassword.ToPasswordHash(out byte[] salt),
					PasswordSalt = salt
				};
				workUnit.UserRepository.UpdatePasswordHashAndSalt(user);
				await workUnit.SaveAsync();
				return true;
			} catch(DbUpdateException) {
				return false;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<UserDetailedModel> AddAsync(UserInputModel model) {
			if(!Validator.TryValidateObject(model, new ValidationContext(model), null, true))
				return null;
			if(!Role.TryGetRoleId(Role.User, out int defaultRoleId))
				return null;
			await using var transaction = await workUnit.BeginTransactionAsync(IsolationLevel.Serializable);
			try {
				User user = mapper.Map<User>(model);
				user.PasswordHash = model.Password.ToPasswordHash(out byte[] salt);
				user.PasswordSalt = salt;
				await workUnit.UserRepository.AddAsync(user);
				await workUnit.SaveAsync();
				workUnit.UserRepository.AddRole(user, defaultRoleId);
				await workUnit.SaveAsync();
				await transaction.CommitAsync();
				var result = mapper.Map<UserDetailedModel>(user);
				return result;
			} catch(DbUpdateException) {
				return null;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<bool> AddAdminRoleAsync(int userId) => await AddRoleAsync(userId, Role.Admin);

		/// <inheritdoc />
		public async Task<bool> RemoveAdminRoleAsync(int userId) => await RemoveRoleAsync(userId, Role.Admin);

		/// <inheritdoc />
		public async Task<bool> DeleteAsync(int userId) {
			try {
				if(!await workUnit.UserRepository.DeleteAsync(userId))
					return false;
				await workUnit.SaveAsync();
				return true;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		private async Task<bool> RemoveRoleAsync(int userId, string role) {
			if(String.IsNullOrEmpty(role))
				return false;
			if(!Role.TryGetRoleId(role, out int roleId)) {
				return false;
			}
			try {
				if(await workUnit.UserRepository.RemoveRoleAsync(userId, roleId)) {
					await workUnit.SaveAsync();
					return true;
				}
				return false;
			} catch(DbUpdateException) {
				return false;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}
		
		private async Task<bool> AddRoleAsync(int userId, string role) {
			if(String.IsNullOrEmpty(role))
				return false;
			if(!Role.TryGetRoleId(role, out int roleId)) {
				return false;
			}
			try {
				if(await workUnit.UserRepository.AddRoleAsync(userId, roleId)) {
					await workUnit.SaveAsync();
					return true;
				}
				return false;
			} catch(DbUpdateException) {
				return false;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}
	}

}