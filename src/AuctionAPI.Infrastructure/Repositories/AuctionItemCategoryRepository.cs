using AuctionAPI.Core.Entities;
using AuctionAPI.Core.Repositories;
using AuctionAPI.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Infrastructure.Repositories {

	public class AuctionItemCategoryRepository : Repository<AuctionItemCategory, int>, IAuctionItemCategoryRepository {

		/// <inheritdoc />
		public AuctionItemCategoryRepository(DbContext context) : base(context) { }
	}

}