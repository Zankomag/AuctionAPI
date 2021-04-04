using System.Threading.Tasks;
using Auction.WebApi.Authorization.Types;

namespace Auction.WebApi.Authorization.Abstractions {

	/// <summary>
	/// Provides methods to retrieve API authentication credentials (like JWT)
	/// </summary>
	public interface IAuthenticationService {
		/// <returns>JWT Token on success or null if email or password is wrong</returns>
		Task<TokenModel> GetToken(string email, string password);
	}

}