using System;
using System.Collections.Generic;

namespace Auction.Application.Models {

	public enum AuctionItemStatusCode {
		Scheduled = 0,
		Started = 1,
		Finished = 2
	}
	
	public class AuctionItemModel : AuctionItemInputModel {
		public AuctionItemStatusCode Status { get; set; }

		public DateTime? ActualCloseDate { get; set; }
		public int? WinningBidId { get; set; }


		public IEnumerable<FileModel> ImageFiles { get; set; }
		public BidModel WinningBid { get; set; }
		public IEnumerable<BidModel> Bids { get; set; }
		public UserModel Seller { get; set; }
		public string CategoryName { get; set; }
	}

}