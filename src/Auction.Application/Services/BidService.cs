using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
	public class BidService : IBidService {
		private const int minIntervalToClosingAuctionItemMinutes = 2;

		private readonly IUnitOfWork workUnit;
		private readonly IMapper mapper;
		private readonly ILogger<BidService> logger;

		public BidService(IUnitOfWork workUnit, IMapper mapper,
			ILogger<BidService> logger) {
			this.workUnit = workUnit;
			this.mapper = mapper;
			this.logger = logger;
		}
		
		/// <inheritdoc />
		public async Task<BidModel> GetByIdWithDetailsAsync(int id) {
			try {
				Bid bid = await workUnit.BidRepository.GetByIdWithDetailsAsync(id);
				return mapper.Map<BidModel>(bid);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<IEnumerable<BidModel>> GetByAuctionItemIdWithDetailsAsync(int auctionItemId) {
			try {
				IEnumerable<Bid> bids = await workUnit.BidRepository.GetByAuctionItemIdWithDetailsAsync(auctionItemId);
				return mapper.Map<IEnumerable<BidModel>>(bids);
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}

		/// <inheritdoc />
		public async Task<BidModel> AddAsync(BidInputModel model) {
			if(!Validator.TryValidateObject(model, new ValidationContext(model), null, true))
				return null;
			try {
				AuctionItem auctionItem = await workUnit.AuctionItemRepository.GetAll()
					.Where(x => x.Id == model.AuctionItemId)
					.Select(x => new AuctionItem {
						Id = x.Id,
						ActualCloseDate = x.ActualCloseDate,
						StartDate = x.StartDate,
						WinningBid = x.WinningBid
					}).FirstOrDefaultAsync();
				var dateTimeUtcNow = DateTime.UtcNow;
				if(auctionItem == null
					|| dateTimeUtcNow <= auctionItem.StartDate
					|| dateTimeUtcNow >= auctionItem.ActualCloseDate) {
					
					return null;
				}
				if(auctionItem.WinningBid != null && auctionItem.WinningBid.Price >= model.Price) {
					return null;
				}
				var newClosingDate = dateTimeUtcNow.AddMinutes(minIntervalToClosingAuctionItemMinutes);
				if((auctionItem.ActualCloseDate - dateTimeUtcNow).Minutes <= minIntervalToClosingAuctionItemMinutes) {
					auctionItem.ActualCloseDate = newClosingDate;
					workUnit.AuctionItemRepository.UpdateActualClosingDate(auctionItem);
				}
				Bid bid = mapper.Map<Bid>(model);
				await using var transaction = await workUnit.BeginTransactionAsync(IsolationLevel.Serializable);
				await workUnit.BidRepository.AddAsync(bid);
				await workUnit.SaveAsync();
				auctionItem.WinningBidId = bid.Id;
				workUnit.AuctionItemRepository.UpdateWinningBidId(auctionItem);
				await workUnit.SaveAsync();
				await transaction.CommitAsync();
				var result = mapper.Map<BidModel>(bid);
				return result;
			} catch(DbUpdateException) {
				return null;
			} catch(Exception ex) {
				logger.LogError(ex, ExceptionThrownInService);
				throw;
			}
		}
	}
}
