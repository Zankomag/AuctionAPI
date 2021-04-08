using System.Collections.Generic;
using System.Threading.Tasks;
using Auction.Application.Models;

namespace Auction.Application.Services.Abstractions {

	public interface IUserService {
		Task<IEnumerable<UserDetailedModel>> GetAllAsync();

		Task<UserDetailedModel> GetByIdAsync(int id);

		Task<UserDetailedModel> GetByEmailAsync(string email);

		Task<UserDetailedModel> AddAsync(UserInputModel model);
		
		/// <returns>True on success</returns>
		Task<bool> AddAdminRoleAsync(int userId);

		/// <returns>True on success</returns>
		Task<bool> RemoveAdminRoleAsync(int userId);

		/// <returns>True on success</returns>
		Task<bool> DeleteAsync(int userId);
		
		/// <returns>User with Id and Role on success or null</returns>
		Task<UserDetailedModel> GetAuthorizationInfoByEmailAndPasswordAsync(string email, string password);

		Task<bool> UserExistsAsync(int userId);

		Task<bool> UpdatePasswordAsync(PasswordChangeModel model);
	}

}