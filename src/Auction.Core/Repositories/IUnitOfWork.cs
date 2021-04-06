using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace Auction.Core.Repositories {

	public interface IUnitOfWork {

		IUserRepository UserRepository { get; }
		IAuctionItemRepository AuctionItemRepository { get; }
		IAuctionItemCategoryRepository AuctionItemCategoryRepository { get; }

		Task<IDbContextTransaction> BeginTransactionAsync();
		
		Task<int> SaveAsync();
	}

}