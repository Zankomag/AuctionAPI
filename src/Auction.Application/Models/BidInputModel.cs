using System.ComponentModel.DataAnnotations;
using Auction.Application.Models.Generic;

namespace Auction.Application.Models {

	public class BidInputModel : Model<int> {
		public int AuctionItemId { get; set; }

		[RegularExpression(@"^\d+(\.?\d{0,2})$")]
		[Range(0.1, 9999999999999999.99)]
		public decimal Price { get; set; }
	}

}