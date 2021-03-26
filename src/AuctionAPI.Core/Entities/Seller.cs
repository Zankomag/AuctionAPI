using System.Collections.Generic;

namespace AuctionAPI.Core.Entities {

	public class Seller : Entity<int> {
		public int UserId { get; set; }

		public User User { get; set; }
		public ICollection<AuctionItem> AuctionItems { get; set; }
	}

}