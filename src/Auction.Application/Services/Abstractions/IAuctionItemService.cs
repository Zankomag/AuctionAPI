using System.Collections.Generic;
using System.Threading.Tasks;
using Auction.Application.Models;
using Auction.Application.Services.Abstractions.Generic;

namespace Auction.Application.Services.Abstractions {

	public interface IAuctionItemService : ICrudService<AuctionItemModel, AuctionItemInputModel, int> {
		Task<IEnumerable<AuctionItemModel>> GetByNameAsync(string name);
	}

}