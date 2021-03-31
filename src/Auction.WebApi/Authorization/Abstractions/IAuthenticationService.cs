using System.Threading.Tasks;
using Auction.WebApi.Authorization.Models;

namespace Auction.WebApi.Authorization.Abstractions {

	public interface IAuthenticationService {
		/// <returns>JWT Token on success or null if email or password is wrong</returns>
		Task<TokenModel> GetToken(string email, string password);
	}

}