using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.Core.Entities {

	public class AuctionItemCategory : Entity<int> {
		[Required]
		[StringLength(30)]
		public string Name { get; set; }
		public int? ParentCategoryId { get; set; }
		
		
		public AuctionItemCategory ParentCategory { get; set; }
		public ICollection<AuctionItemCategory> ChildCategories { get; set; }
	}

}