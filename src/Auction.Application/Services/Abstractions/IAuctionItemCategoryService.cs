using System.Collections.Generic;
using System.Threading.Tasks;
using Auction.Application.Models;
using Auction.Application.Services.Abstractions.Generic;

namespace Auction.Application.Services.Abstractions {

	public interface IAuctionItemCategoryService : ICrudService<AuctionItemCategoryDetailedModel,
		AuctionItemCategoryInputModel, int> {
		Task<IEnumerable<AuctionItemCategoryDetailedModel>> GetByNameAsync(string categoryName);
	}

}