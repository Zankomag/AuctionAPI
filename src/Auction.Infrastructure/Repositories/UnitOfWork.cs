using System.Threading.Tasks;
using Auction.Core.Repositories;
using Auction.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using IsolationLevel = System.Data.IsolationLevel;

namespace Auction.Infrastructure.Repositories {

	public class UnitOfWork : IUnitOfWork {
		protected AuctionDbContext Context;
		private IAuctionItemCategoryRepository auctionItemCategoryRepository;
		private IUserRepository userRepository;
		private IAuctionItemRepository auctionItemRepository;
		private IBidRepository bidRepository;

		public UnitOfWork(AuctionDbContext context) => Context = context;

		/// <inheritdoc />
		public IUserRepository UserRepository
			=> userRepository ??= new UserRepository(Context);

		/// <inheritdoc />
		public IAuctionItemRepository AuctionItemRepository
			=> auctionItemRepository ??= new AuctionItemRepository(Context);
		
		/// <inheritdoc />
		public IAuctionItemCategoryRepository AuctionItemCategoryRepository
			=> auctionItemCategoryRepository ??= new AuctionItemCategoryRepository(Context);

		/// <inheritdoc />
		public IBidRepository BidRepository
			=> bidRepository ??= new BidRepository(Context);

		public async Task<IDbContextTransaction> BeginTransactionAsync()
			=> await Context.Database.BeginTransactionAsync(IsolationLevel.Serializable);


		/// <inheritdoc />
		public async Task<int> SaveAsync() => await Context.SaveChangesAsync();
	}

}