using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionAPI.Application.Models;

namespace AuctionAPI.Application.Services.Abstractions {

	public interface IUserService {
		Task<IEnumerable<UserModel>> GetAllAsync();

		Task<UserModel> GetByIdAsync(int id);

		Task<UserModel> GetByEmailAsync(string email);

		Task<UserModel> AddAsync(UserInputModel model);
		
		/// <returns>True on success</returns>
		Task<bool> UpdateRoleAsync(int userId, string role);

		/// <returns>True on success</returns>
		Task<bool> DeleteAsync(int userId);
	}

}