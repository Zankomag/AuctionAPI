using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Core.Entities;
using Auction.Core.Repositories.Generic;

namespace Auction.Core.Repositories {

	public interface IAuctionItemRepository : ICrudRepository<AuctionItem, int> {
		IQueryable<AuctionItem> GetAllWithDetails();
		Task<IEnumerable<AuctionItem>> GetAllWithDetailsAsync();
		Task<AuctionItem> GetByIdWithDetailsAsync(int id);
		void UpdateActualClosingDate(AuctionItem auctionItem);
		void UpdateWinningBidId(AuctionItem auctionItem);
		Task AddImageAsync(int auctionItemId, byte[] image);
		IQueryable<AuctionItemImage> GetAllImages();

	}

}