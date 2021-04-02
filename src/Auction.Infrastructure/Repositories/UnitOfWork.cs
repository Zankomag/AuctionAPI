using System.Threading.Tasks;
using Auction.Core.Repositories;
using Auction.Infrastructure.Data;

namespace Auction.Infrastructure.Repositories {

	public class UnitOfWork : IUnitOfWork {
		protected AuctionDbContext Context;
		private IAuctionItemCategoryRepository auctionItemCategoryRepository;
		private IUserRepository userRepository;
		private IAuctionItemRepository auctionItemRepository;

		public UnitOfWork(AuctionDbContext context) => Context = context;

		/// <inheritdoc />
		public IUserRepository UserRepository
			=> userRepository ??= new UserRepository(Context);

		public IAuctionItemRepository AuctionItemRepository
			=> auctionItemRepository ??= new AuctionItemRepository(Context);
		
		/// <inheritdoc />
		public IAuctionItemCategoryRepository AuctionItemCategoryRepository
			=> auctionItemCategoryRepository ??= new AuctionItemCategoryRepository(Context);

		/// <inheritdoc />
		public async Task<int> SaveAsync() => await Context.SaveChangesAsync();
	}

}