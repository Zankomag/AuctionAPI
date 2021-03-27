using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AuctionAPI.Application.Models.Generic;
using AuctionAPI.Core.Entities;

namespace AuctionAPI.Application.Models {

	public class AuctionItemCategoryModel : Model<int> {
		[Required(AllowEmptyStrings = false)]
		[StringLength(30)]
		public string Name { get; set; }

		public int? ParentCategoryId { get; set; }


		public AuctionItemCategory ParentCategory { get; set; }
		public IEnumerable<AuctionItemCategory> ChildCategories { get; set; }
	}

}