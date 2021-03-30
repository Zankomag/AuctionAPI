using System;

namespace AuctionAPI.Web.Models {

	public class TokenModel {
		public string Token { get; set; }
		public DateTime Expiration { get; set; }
	}

}