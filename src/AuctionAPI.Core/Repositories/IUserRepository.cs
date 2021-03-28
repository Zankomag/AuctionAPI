using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionAPI.Core.Entities;

namespace AuctionAPI.Core.Repositories {

	public interface IUserRepository {
		Task<IEnumerable<User>> GetAllAsync();

		Task AddAsync(User user);

		void UpdateRoleAsync(int userId, string role);

		Task<bool> DeleteAsync(int userId);
	}

}