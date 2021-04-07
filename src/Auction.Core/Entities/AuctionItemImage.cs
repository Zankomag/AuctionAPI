using System.ComponentModel.DataAnnotations;

namespace Auction.Core.Entities {

	public class AuctionItemImage : Entity<int> {
		public int AuctionItemId { get; set; }
		
		//TODO Try to implement filestream https://stackoverflow.com/a/44540717/11101834
		public byte[] Image { get; set; }

		[Required]
		public string FileExtension { get; set; }

		public AuctionItem AuctionItem { get; set; }
	}

}