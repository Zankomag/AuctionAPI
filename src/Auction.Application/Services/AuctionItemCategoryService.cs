using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Auction.Application.Extensions;
using Auction.Application.Models;
using Auction.Application.Services.Abstractions;
using Auction.Core.Entities;
using Auction.Core.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using static Auction.Application.Utils.LoggingMessages;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Auction.Application.Services {

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
		public async Task<IEnumerable<AuctionItemCategoryDetailedModel>> GetAllAsync() {
			try {
				IEnumerable<AuctionItemCategory> auctionItemCategories =
					await workUnit.AuctionItemCategoryRepository.GetAllWithDetailsAsync();
				return mapper.Map<IEnumerable<AuctionItemCategoryDetailedModel>>(auctionItemCategories);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<AuctionItemCategoryDetailedModel> GetByIdAsync(int id) {
			try {
				AuctionItemCategory auctionItemCategory =
					await workUnit.AuctionItemCategoryRepository.GetByIdWithDetailsAsync(id);
				return mapper.Map<AuctionItemCategoryDetailedModel>(auctionItemCategory);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<IEnumerable<AuctionItemCategoryDetailedModel>> GetByNameAsync(string categoryName) {
			try {
				List<AuctionItemCategory> auctionItemCategories = await workUnit.AuctionItemCategoryRepository
					.GetAll()

					//If we'd used string.Contains, then expression wouldn't be case insensitive
					//we use ~ as escape char because ToLikeString() escapes chars with ~
					.Where(x => EF.Functions.Like(x.Name, categoryName.ToLikeString(), "~"))
					.ToListAsync();
				return mapper.Map<IEnumerable<AuctionItemCategoryDetailedModel>>(auctionItemCategories);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<AuctionItemCategoryInputModel> AddAsync(AuctionItemCategoryInputModel model) {
			if(!Validator.TryValidateObject(model, new ValidationContext(model), null, true))
				return null;
			try {
				AuctionItemCategory auctionItemCategory = mapper.Map<AuctionItemCategory>(model);
				await workUnit.AuctionItemCategoryRepository.AddAsync(auctionItemCategory);
				await workUnit.SaveAsync();
				model.Id = auctionItemCategory.Id;
				return model;
			} catch(DbUpdateException) {
				return null;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<AuctionItemCategoryDetailedModel> UpdateAsync(int id, AuctionItemCategoryInputModel model) {
			if(!Validator.TryValidateObject(model, new ValidationContext(model), null, true))
				return null;
			try {
				model.Id = id;
				AuctionItemCategory auctionItemCategory = mapper.Map<AuctionItemCategory>(model);
				workUnit.AuctionItemCategoryRepository.Update(auctionItemCategory);
				await workUnit.SaveAsync();
				AuctionItemCategory result = await workUnit.AuctionItemCategoryRepository.GetByIdWithDetailsAsync(id);
				return mapper.Map<AuctionItemCategoryDetailedModel>(result);
			} catch(DbUpdateException) {
				return null;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<bool> DeleteByIdAsync(int id) {
			try {
				if(!await workUnit.AuctionItemCategoryRepository.DeleteByIdAsync(id))
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