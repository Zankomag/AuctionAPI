using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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

	public class AuctionItemCategoryService : IAuctionItemCategoryService {
		private readonly IUnitOfWork workUnit;
		private readonly IMapper mapper;
		private readonly ILogger<AuctionItemCategoryService> logger;

		public AuctionItemCategoryService(IUnitOfWork workUnit, IMapper mapper,
			ILogger<AuctionItemCategoryService> logger) {
			this.workUnit = workUnit;
			this.mapper = mapper;
			this.logger = logger;
		}

		/// <inheritdoc />
		public async Task<IEnumerable<AuctionItemCategoryModel>> GetAllAsync() {
			try {
				IEnumerable<AuctionItemCategory> auctionItemCategories =
					await workUnit.AuctionItemCategoryRepository.GetAllAsync();
				return mapper.Map<IEnumerable<AuctionItemCategoryModel>>(auctionItemCategories);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<AuctionItemCategoryModel> GetByIdAsync(int id) {
			try {
				AuctionItemCategory auctionItemCategory = await workUnit.AuctionItemCategoryRepository.GetByIdAsync(id);
				return mapper.Map<AuctionItemCategoryModel>(auctionItemCategory);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<IEnumerable<AuctionItemCategoryModel>> GetByNameAsync(string categoryName) {
			try {
				List<AuctionItemCategory> auctionItemCategories = await workUnit.AuctionItemCategoryRepository.GetAll()
					//If we'd used string.Contains, then expression wouldn't be case insensitive
					//we use ~ as escape char because ToLikeString() escapes chars with ~
					.Where(x => EF.Functions.Like(x.Name, categoryName.ToLikeString(), "~"))
					.ToListAsync();
				return mapper.Map<IEnumerable<AuctionItemCategoryModel>>(auctionItemCategories);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<AuctionItemCategoryModel> AddAsync(AuctionItemCategoryModel model) {
			if(!Validator.TryValidateObject(model, new ValidationContext(model), null, true))
				return null;
			try {
				AuctionItemCategory auctionItemCategory = mapper.Map<AuctionItemCategory>(model);
				await workUnit.AuctionItemCategoryRepository.AddAsync(auctionItemCategory);
				await workUnit.SaveAsync();
				model.Id = auctionItemCategory.Id;
				return model;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<AuctionItemCategoryModel> UpdateAsync(AuctionItemCategoryModel model) {
			if(!Validator.TryValidateObject(model, new ValidationContext(model), null, true))
				return null;
			try {
				AuctionItemCategory auctionItemCategory = mapper.Map<AuctionItemCategory>(model);
				workUnit.AuctionItemCategoryRepository.Update(auctionItemCategory);
				await workUnit.SaveAsync();
				return model;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task DeleteByIdAsync(int id) {
			try {
				await workUnit.AuctionItemCategoryRepository.DeleteByIdAsync(id);
				await workUnit.SaveAsync();
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}
	}

}