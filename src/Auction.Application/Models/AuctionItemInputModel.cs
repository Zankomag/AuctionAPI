using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Auction.Application.Models.Generic;

namespace Auction.Application.Models {

	public class AuctionItemInputModel : Model<int> {
		public int SellerId { get; set; }
		public int? AuctionItemCategoryId { get; set; }

		[Required]
		[StringLength(256)]
		public string Name { get; set; }

		[StringLength(1024)]
		public string Description { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime PlannedCloseDate { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal StartingPrice { get; set; }
	}

}