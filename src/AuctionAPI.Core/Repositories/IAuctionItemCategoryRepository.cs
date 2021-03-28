using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionAPI.Core.Entities;
using AuctionAPI.Core.Repositories.Generic;

namespace AuctionAPI.Core.Repositories {

	public interface IAuctionItemCategoryRepository : ICrudRepository<AuctionItemCategory, int> {

		IQueryable<AuctionItemCategory> GetAllWithDetails();
		Task<IEnumerable<AuctionItemCategory>> GetAllWithDetailsAsync();
		Task<AuctionItemCategory> GetByIdWithDetailsAsync(int id);
	}

}