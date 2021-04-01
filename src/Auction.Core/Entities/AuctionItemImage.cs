namespace Auction.Core.Entities {

	public class AuctionItemImage : Entity<int> {
		public int AuctionItemId { get; set; }
		public byte[] Image { get; set; }
	}

}