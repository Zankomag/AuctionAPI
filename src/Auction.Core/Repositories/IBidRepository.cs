using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Core.Entities;
using Auction.Core.Repositories.Generic;

namespace Auction.Core.Repositories {

	public interface IBidRepository : IRepository<Bid, int> {
		/// <summary>
		/// Returns query without tracking 
		/// </summary>
		IQueryable<Bid> GetAll();

		Task<Bid> GetByIdWithDetailsAsync(int id);
		
		Task<IEnumerable<Bid>> GetByAuctionItemIdWithDetailsAsync(int auctionItemId);

		Task AddAsync(Bid entity);
	}

}