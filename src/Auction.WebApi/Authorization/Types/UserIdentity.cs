using System.Collections.Generic;
using System.Security.Claims;
using Auction.WebApi.Authorization.Constants;

namespace Auction.WebApi.Authorization.Types {

	public class UserIdentity {
		public int Id { get; set; }
		public string Role { get; set; }

		public List<Claim> GetClaims() {
			var claims = new List<Claim> {
				new(JwtOpenIdProperty.Sub, Id.ToString()),
				new(JwtOpenIdProperty.Role, Role)
			};
			return claims;
		}
	}

}