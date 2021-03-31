using System;

namespace Auction.WebApi.Authorization.Models {

	public class TokenModel {
		public string Token { get; set; }
		public DateTime Expiration { get; set; }
	}

}