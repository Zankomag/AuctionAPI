using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionAPI.Core.Entities {

	public class Bid : Entity<int> {
		public int BidderId { get; set; }
		public int AuctionItemId { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal Price { get; set; }


		public User Bidder { get; set; }
		public AuctionItem AuctionItem { get; set; }
	}

}