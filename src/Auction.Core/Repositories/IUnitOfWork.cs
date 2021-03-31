using System.Threading.Tasks;

namespace Auction.Core.Repositories {

	public interface IUnitOfWork {

		IUserRepository UserRepository { get; }

		IAuctionItemCategoryRepository AuctionItemCategoryRepository { get; }

		Task<int> SaveAsync();
	}

}