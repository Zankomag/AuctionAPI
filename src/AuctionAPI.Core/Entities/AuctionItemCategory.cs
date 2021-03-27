using System.ComponentModel.DataAnnotations;

namespace AuctionAPI.Core.Entities {

	public class AuctionItemCategory : Entity<int> {
		[Required]
		[StringLength(30)]
		public string Name { get; set; }
	}

}