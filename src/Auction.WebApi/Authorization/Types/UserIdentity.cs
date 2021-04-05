using System.Collections.Generic;
using System.Security.Claims;
using Auction.WebApi.Authorization.Constants;

namespace Auction.WebApi.Authorization.Types {

	public class UserIdentity : ClaimsIdentity {
		public int Id { get; init; }

		public List<string> Roles { get; init; }

		public string IdString { get; init; }

		/// <summary>
		///     Creates new claims that includes <see cref="Id" /> and <see cref="Roles" />
		/// </summary>
		public List<Claim> GetClaims() {
			var claims = new List<Claim> {
				new(JwtOpenIdProperty.Sub, Id.ToString())
			};
			Roles.ForEach(role => claims.Add(new Claim(JwtOpenIdProperty.Role, role)));
			return claims;
		}
	}

}