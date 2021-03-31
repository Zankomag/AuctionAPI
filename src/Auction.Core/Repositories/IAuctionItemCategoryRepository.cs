using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Core.Entities;
using Auction.Core.Repositories.Generic;

namespace Auction.Core.Repositories {

	public interface IAuctionItemCategoryRepository : ICrudRepository<AuctionItemCategory, int> {

		IQueryable<AuctionItemCategory> GetAllWithDetails();
		Task<IEnumerable<AuctionItemCategory>> GetAllWithDetailsAsync();
		Task<AuctionItemCategory> GetByIdWithDetailsAsync(int id);
	}

}