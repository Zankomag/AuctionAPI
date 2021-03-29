using System.Threading.Tasks;

namespace AuctionAPI.Core.Repositories {

	public interface IUnitOfWork {

		IUserRepository UserRepository { get; }

		IAuctionItemCategoryRepository AuctionItemCategoryRepository { get; }

		Task<int> SaveAsync();
	}

}