using System.Collections.Generic;

namespace AuctionAPI.Core.Entities {

	public class Bidder : Entity<int> {
		public int UserId { get; set; }

		
		public User User { get; set; }
		public ICollection<Bid> Bids { get; set; }
		public ICollection<AuctionItem> AuctionItems { get; set; }
	}

}