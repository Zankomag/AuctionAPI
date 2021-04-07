using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Core.Entities;
using Auction.Core.Repositories;
using Auction.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Repositories {

	public class BidRepository : Repository<Bid, int>, IBidRepository {
		/// <inheritdoc />
		public BidRepository(DbContext context) : base(context) { }

		/// <inheritdoc />
		public IQueryable<Bid> GetAll() => DbSet.AsNoTracking();

		/// <inheritdoc />
		public async Task<Bid> GetByIdWithDetailsAsync(int id)
			=> await GetAll()
				.Include(x => x.AuctionItem)
				.Include(x => x.Bidder)
				.FirstOrDefaultAsync(x => x.Id == id);

		/// <inheritdoc />
		public async Task<IEnumerable<Bid>> GetByAuctionItemIdWithDetailsAsync(int auctionItemId)
			=> await GetAll()
				.Include(x => x.Bidder)
				.Where(x => x.AuctionItemId == auctionItemId)
				.ToListAsync();

		/// <inheritdoc />
		public async Task AddAsync(Bid entity) => await DbSet.AddAsync(entity);
	}

}