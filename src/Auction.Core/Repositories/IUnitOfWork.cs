using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using IsolationLevel = System.Data.IsolationLevel;

namespace Auction.Core.Repositories {

	public interface IUnitOfWork {

		IUserRepository UserRepository { get; }
		IAuctionItemRepository AuctionItemRepository { get; }
		IAuctionItemCategoryRepository AuctionItemCategoryRepository { get; }
		IBidRepository BidRepository { get; }

		Task<IDbContextTransaction> BeginTransactionAsync(IsolationLevel isolationLevel);
		
		Task<int> SaveAsync();
	}

}