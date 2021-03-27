using AuctionAPI.Core.Entities;
using AuctionAPI.Core.Repositories.Generic;

namespace AuctionAPI.Core.Repositories {

	public interface IAuctionItemCategoryRepository : IRepository<AuctionItemCategory, int> { }

}