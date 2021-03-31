using System.ComponentModel.DataAnnotations;
using Auction.Application.Models.Generic;

namespace Auction.Application.Models {

	public class AuctionItemCategoryInputModel : Model<int> {
		[Required(AllowEmptyStrings = false)]
		[StringLength(30)]
		public string Name { get; set; }

		public int? ParentCategoryId { get; set; }
	}

}