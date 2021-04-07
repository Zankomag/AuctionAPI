using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Core.Entities;
using Auction.Core.Repositories.Generic;

namespace Auction.Core.Repositories {

	public interface IAuctionItemRepository : ICrudRepository<AuctionItem, int> {
		Task<IEnumerable<AuctionItem>> GetAllWithDetailsAsync();
		Task<AuctionItem> GetByIdWithDetailsAsync(int id);
		
	}

}