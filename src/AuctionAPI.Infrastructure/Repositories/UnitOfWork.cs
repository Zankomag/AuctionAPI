using System.Threading.Tasks;
using AuctionAPI.Core.Repositories;
using AuctionAPI.Infrastructure.Data;

namespace AuctionAPI.Infrastructure.Repositories {

	public class UnitOfWork : IUnitOfWork {
		protected AuctionDbContext Context;
		private IAuctionItemCategoryRepository auctionItemCategoryRepository;
		private IUserRepository userRepository;

		public UnitOfWork(AuctionDbContext context) => Context = context;

		/// <inheritdoc />
		public IUserRepository UserRepository
			=> userRepository ??= new UserRepository(Context);

		/// <inheritdoc />
		public IAuctionItemCategoryRepository AuctionItemCategoryRepository
			=> auctionItemCategoryRepository ??= new AuctionItemCategoryRepository(Context);

		/// <inheritdoc />
		public async Task<int> SaveAsync() => await Context.SaveChangesAsync();
	}

}