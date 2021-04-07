namespace Auction.Application.Models {

	public class BidModel : BidInputModel {
		public int AuctionItemId { get; set; }
		public int BidderId { get; set; }

		public UserModel Bidder { get; set; }
		public AuctionItemModel AuctionItem { get; set; }
	}

}