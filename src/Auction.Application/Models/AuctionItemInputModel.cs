using System;
using System.ComponentModel.DataAnnotations;
using Auction.Application.Models.Generic;
using Newtonsoft.Json;

namespace Auction.Application.Models {

	public class AuctionItemInputModel : Model<int> {
		public int? AuctionItemCategoryId { get; set; }
		//TODO fix jsonignore doesn't work
		[JsonIgnore]
		public int SellerId { get; set; }
		
		[Required]
		[StringLength(256)]
		public string Name { get; set; }

		[StringLength(1024)]
		public string Description { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime PlannedCloseDate { get; set; }

		[RegularExpression(@"^\d+(\.?\d{0,2})$")]
		[Range(0.1, 9999999999999999.99)]
		public decimal StartingPrice { get; set; }
	}

}