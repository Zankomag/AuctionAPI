using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Core.Entities;
using Auction.Core.Repositories;
using Auction.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Repositories {

	public class AuctionItemCategoryRepository : CrudRepository<AuctionItemCategory, int>, IAuctionItemCategoryRepository {

		/// <inheritdoc />
		public AuctionItemCategoryRepository(DbContext context) : base(context) { }

		/// <inheritdoc />
		public async Task<IEnumerable<AuctionItemCategory>> GetAllWithDetailsAsync() {
			//To optimize: First we get all categories in one query and then perform sorting locally
			//(anyway we need all categories)

			var rootCategories = await GetAllWithDetails()
				.Where(x => x.ParentCategoryId == null)
				.ToListAsync();
			foreach(AuctionItemCategory category in rootCategories
				.Where(category => category.ChildCategories?.Any() == true)) {
				category.ChildCategories = await GetChildren(category);
			}
			return rootCategories;
		}
		
		

		private async Task<ICollection<AuctionItemCategory>> GetChildren(AuctionItemCategory parent) {
			var children = await GetAll().Where(x => x.ParentCategoryId == parent.Id)
				.ToListAsync();

			foreach(AuctionItemCategory child in children) {
				child.ChildCategories = await GetChildren(child);
			}
			return children;
		}

		/// <inheritdoc />
		public async Task<AuctionItemCategory> GetByIdWithDetailsAsync(int id) {
			var category =  await GetAllWithDetails().FirstOrDefaultAsync(x => x.Id == id);
			if(category?.ChildCategories?.Any() == true) {
				category.ChildCategories = await GetChildren(category);
			}
			return category;
		}

		public IQueryable<AuctionItemCategory> GetAllWithDetails() => GetAll()
			//.Include(x => x.ParentCategory)
			.Include(x => x.ChildCategories);
	}

}