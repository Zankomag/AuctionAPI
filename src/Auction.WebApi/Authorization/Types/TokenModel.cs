using System;

namespace Auction.WebApi.Authorization.Types {

	public class TokenModel {
		public string Token { get; init; }
		public DateTime Expiration { get; init; }
	}

}