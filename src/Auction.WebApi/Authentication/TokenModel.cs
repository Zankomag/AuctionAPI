using System;

namespace AuctionAPI.Web.Authentication {

	public class TokenModel {
		public string Token { get; set; }
		public DateTime Expiration { get; set; }
	}

}