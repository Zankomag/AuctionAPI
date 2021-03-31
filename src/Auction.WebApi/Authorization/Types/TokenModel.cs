using System;

namespace Auction.WebApi.Authorization.Types {

	public class TokenModel {
		public string Token { get; set; }
		public DateTime Expiration { get; set; }
	}

}