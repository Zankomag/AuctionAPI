using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionAPI.Core.Entities;

namespace AuctionAPI.Core.Repositories {

	public interface IUserRepository {
		Task<IEnumerable<User>> GetAllAsync();

		Task<User> GetByIdAsync(int id);

		Task<User> GetByEmailAsync(string email);

		Task AddAsync(User user);

		void UpdateRoleAsync(int userId, string role);

		/// <returns>True on success</returns>
		Task<bool> DeleteAsync(int userId);
	}

}