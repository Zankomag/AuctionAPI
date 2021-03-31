using System.Collections.Generic;
using AuctionAPI.Core.Entities;

namespace AuctionAPI.Application.Models {

	public class AuctionItemCategoryDetailedModel : AuctionItemCategoryInputModel {
		
		public AuctionItemCategoryDetailedModel ParentCategory { get; set; }
		public IEnumerable<AuctionItemCategoryDetailedModel> ChildCategories { get; set; }
	}

}