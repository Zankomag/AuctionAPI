using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Auction.Core.Repositories {

	public interface IUnitOfWork {

		IUserRepository UserRepository { get; }
		IAuctionItemRepository AuctionItemRepository { get; }
		IAuctionItemCategoryRepository AuctionItemCategoryRepository { get; }
		IBidRepository BidRepository { get; }

		Task<IDbContextTransaction> BeginTransactionAsync();
		
		Task<int> SaveAsync();
	}

}