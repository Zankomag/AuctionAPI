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
using static Auction.Application.Utils.LoggingMessage;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Auction.Application.Services {

	public class AuctionItemService : IAuctionItemService {
		private const int minAuctionStartToClosingIntervalMinutes = 10;

		private readonly IUnitOfWork workUnit;
		private readonly IMapper mapper;
		private readonly ILogger<AuctionItemService> logger;

		public AuctionItemService(IUnitOfWork workUnit, IMapper mapper,
			ILogger<AuctionItemService> logger) {
			this.workUnit = workUnit;
			this.mapper = mapper;
			this.logger = logger;
		}

		/// <inheritdoc />
		public async Task<IEnumerable<AuctionItemModel>> GetAllAsync() {
			try {
				IEnumerable<AuctionItem> auctionItems = await workUnit.AuctionItemRepository.GetAllWithDetailsAsync();
				return mapper.Map<IEnumerable<AuctionItemModel>>(auctionItems);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<AuctionItemModel> GetByIdAsync(int id) {
			try {
				AuctionItem auctionItem = await workUnit.AuctionItemRepository.GetByIdWithDetailsAsync(id);
				return mapper.Map<AuctionItemModel>(auctionItem);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<IEnumerable<AuctionItemModel>> GetByNameAsync(string name) {
			try {
				List<AuctionItem> auctionItems = await workUnit.AuctionItemRepository
					.GetAll()
					.Where(x => EF.Functions.Like(x.Name, name.ToLikeString(), "~"))
					.ToListAsync();
				return mapper.Map<IEnumerable<AuctionItemModel>>(auctionItems);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<int> GetOwnerId(int id) {
			try {
				int ownerId = await workUnit.AuctionItemRepository.GetAll()
					.Where(x => x.Id == id)
					.Select(x => x.SellerId)
					.FirstOrDefaultAsync();
				return ownerId;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<bool> IsUserOwner(int auctionItemId, int userId) {
			if(userId == default) return false;
			return userId == await GetOwnerId(auctionItemId);
		}

		/// <inheritdoc />
		public async Task<AuctionItemInputModel> AddAsync(AuctionItemInputModel model) {
			if(!Validator.TryValidateObject(model, new ValidationContext(model), null, true))
				return null;
			if(!AreAuctionItemDatesValid(model))
				return null;
			try {
				AuctionItem auctionItem = mapper.Map<AuctionItem>(model);
				auctionItem.ActualCloseDate = auctionItem.PlannedCloseDate;
				await workUnit.AuctionItemRepository.AddAsync(auctionItem);
				await workUnit.SaveAsync();
				model.Id = auctionItem.Id;
				return model;
			} catch(DbUpdateException) {
				return null;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<AuctionItemModel> UpdateAsync(int id, AuctionItemInputModel model) {
			if(!Validator.TryValidateObject(model, new ValidationContext(model), null, true))
				return null;
			if(!AreAuctionItemDatesValid(model))
				return null;
			try {
				var auctionItem = await workUnit.AuctionItemRepository.GetAll(false)
					.FirstOrDefaultAsync(x => x.Id == id);
				if(auctionItem == null || DateTime.UtcNow >= auctionItem.StartDate) {
					return null;
				}
				model.Id = id;
				mapper.Map(model, auctionItem);
				auctionItem.ActualCloseDate = auctionItem.PlannedCloseDate;
				await workUnit.SaveAsync();
				AuctionItem result = await workUnit.AuctionItemRepository.GetByIdWithDetailsAsync(id);
				return mapper.Map<AuctionItemModel>(result);
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
				//We can delete only not started auctions
				if(!await workUnit.AuctionItemRepository.GetAll().AnyAsync(x => x.Id == id
					&& DateTime.UtcNow < x.StartDate)) {

					return false;
				}
				await workUnit.AuctionItemRepository.DeleteByIdAsync(id);
				await workUnit.SaveAsync();
				return true;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		private bool AreAuctionItemDatesValid(AuctionItemInputModel model)
			=> DateTime.UtcNow < model.StartDate
				&& model.PlannedCloseDate >= model.StartDate.AddMinutes(minAuctionStartToClosingIntervalMinutes);
	}

}