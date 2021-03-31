using System.Collections.Generic;

namespace Auction.Application.Models {

	public class AuctionItemCategoryDetailedModel : AuctionItemCategoryInputModel {
		
		public AuctionItemCategoryDetailedModel ParentCategory { get; set; }
		public IEnumerable<AuctionItemCategoryDetailedModel> ChildCategories { get; set; }
	}

}