using System.Collections.Generic;
using System.Threading.Tasks;
using Auction.Application.Models;

namespace Auction.Application.Services.Abstractions {

	public interface IBidService {
		Task<BidModel> GetByIdWithDetailsAsync(int id);

		Task<IEnumerable<BidModel>> GetByAuctionItemIdWithDetailsAsync(int auctionItemId);

		Task<BidModel> AddAsync(BidInputModel model);
	}

}