using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AuctionAPI.Application.Authorization;
using AuctionAPI.Application.Extensions;
using AuctionAPI.Application.Models;
using AuctionAPI.Application.Services.Abstractions;
using AuctionAPI.Core.Entities;
using AuctionAPI.Core.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static AuctionAPI.Application.Utils.LoggingMessages;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace AuctionAPI.Application.Services {

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
		public async Task<IEnumerable<UserModel>> GetAllAsync() {
			try {
				IEnumerable<User> users = await workUnit.UserRepository.GetAllAsync();
				return mapper.Map<IEnumerable<UserModel>>(users);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<UserModel> GetByIdAsync(int id) {
			try {
				User user = await workUnit.UserRepository.GetByIdAsync(id);
				return mapper.Map<UserModel>(user);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<UserModel> GetByEmailAsync(string email) {
			try {
				User user = await workUnit.UserRepository.GetByEmailAsync(email);
				return mapper.Map<UserModel>(user);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<UserModel> AddAsync(UserInputModel model) {
			if(!Validator.TryValidateObject(model, new ValidationContext(model), null, true))
				return null;
			try {
				User user = mapper.Map<User>(model);
				user.Role = Roles.User;
				user.PasswordHash = model.Password.ToPasswordHash(out byte[] salt);
				user.PasswordSalt = salt;
				await workUnit.UserRepository.AddAsync(user);
				await workUnit.SaveAsync();
				var result = mapper.Map<UserModel>(user);
				return result;
			} catch(DbUpdateException) {
				return null;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		private async Task<bool> UpdateRoleAsync(int userId, string role) {
			if(String.IsNullOrEmpty(role))
				return false;
			try {
				workUnit.UserRepository.UpdateRoleAsync(userId, role);
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
		public async Task<bool> UpdateRoleToAdminAsync(int userId) => await UpdateRoleAsync(userId, Roles.Admin);

		/// <inheritdoc />
		public async Task<bool> UpdateRoleToUserAsync(int userId) => await UpdateRoleAsync(userId, Roles.User);

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
	}

}