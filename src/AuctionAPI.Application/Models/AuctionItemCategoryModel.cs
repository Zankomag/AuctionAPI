using System.ComponentModel.DataAnnotations;
using AuctionAPI.Application.Models.Generic;

namespace AuctionAPI.Application.Models {

	public class AuctionItemCategoryModel : Model<int> {
		[Required(AllowEmptyStrings = false)]
		[StringLength(30)]
		public string Name { get; set; }
	}

}