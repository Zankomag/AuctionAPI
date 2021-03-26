using System.Collections.Generic;

namespace AuctionAPI.Core.Entities {

	public class Bidder : Entity<int> {
		public int UserId { get; set; }

		
		public User User { get; set; }
		public List<Bid> Bids { get; set; }
		public List<AuctionItem> AuctionItems { get; set; }
	}

}