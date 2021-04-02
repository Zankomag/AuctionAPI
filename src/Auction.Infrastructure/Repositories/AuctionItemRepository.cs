using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Core.Entities;
using Auction.Core.Repositories;
using Auction.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Repositories {

	public class AuctionItemRepository : CrudRepository<AuctionItem, int>, IAuctionItemRepository {
		/// <inheritdoc />
		public AuctionItemRepository(DbContext context) : base(context) { }

		/// <inheritdoc />
		public async Task<IEnumerable<AuctionItem>> GetAllWithDetailsAsync()
			=> await GetAllWithDetails().ToListAsync();

		/// <inheritdoc />
		public async Task<AuctionItem> GetByIdWithDetailsAsync(int id) 
			=> await GetAllWithDetails().FirstOrDefaultAsync(x => x.Id == id);

		public IQueryable<AuctionItem> GetAllWithDetails()
			=> GetAll()
				.Include(x => x.Images)
				.Include(x => x.WinningBid)
				.Include(x => x.Bids)
				.Include(x => x.Seller)
				.Include(x => x.AuctionItemCategory);

		/// <inheritdoc />
		public override Task<bool> DeleteByIdAsync(int id) {
			DbSet.Remove(new AuctionItem() {Id = id});
			return Task.FromResult(true);
		}
	}

}