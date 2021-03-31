using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionAPI.Application.Models;
using AuctionAPI.Application.Services.Abstractions.Generic;

namespace AuctionAPI.Application.Services.Abstractions {

	public interface IAuctionItemCategoryService : ICrudService<AuctionItemCategoryDetailedModel,
		AuctionItemCategoryInputModel, int> {
		Task<IEnumerable<AuctionItemCategoryDetailedModel>> GetByNameAsync(string categoryName);
	}

}