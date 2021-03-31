using System;

namespace Auction.WebApi.Authentication {

	public class TokenModel {
		public string Token { get; set; }
		public DateTime Expiration { get; set; }
	}

}