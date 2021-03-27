using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionAPI.Core.Entities;
using AuctionAPI.Core.Repositories;
using AuctionAPI.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Infrastructure.Repositories {

	public class AuctionItemCategoryRepository : Repository<AuctionItemCategory, int>, IAuctionItemCategoryRepository {

		/// <inheritdoc />
		public AuctionItemCategoryRepository(DbContext context) : base(context) { }

		/// <inheritdoc />
		public async Task<IEnumerable<AuctionItemCategory>> GetAllWithDetailsAsync()
			=> await GetAllWithDetails().ToListAsync();

		/// <inheritdoc />
		public async Task<AuctionItemCategory> GetByIdWithDetailsAsync(int id)
			=> await GetAllWithDetails().FirstOrDefaultAsync(x => x.Id == id);

		public IQueryable<AuctionItemCategory> GetAllWithDetails() => GetAll()
			.Include(x => x.ParentCategory)
			.Include(x => x.ChildCategories);
	}

}