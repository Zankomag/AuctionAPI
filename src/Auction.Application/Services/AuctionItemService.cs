﻿using System;
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
	public class AuctionItemService : IAuctionItemService {

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
		public async Task<AuctionItemInputModel> AddAsync(AuctionItemInputModel model) {
			if(!Validator.TryValidateObject(model, new ValidationContext(model), null, true))
				return null;
			try {
				AuctionItem auctionItem = mapper.Map<AuctionItem>(model);
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
			try {
				model.Id = id;
				AuctionItem auctionItem = mapper.Map<AuctionItem>(model);
				workUnit.AuctionItemRepository.Update(auctionItem);
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
				if(!await workUnit.AuctionItemRepository.DeleteByIdAsync(id))
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
