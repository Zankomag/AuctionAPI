using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionAPI.Core.Entities {

	public class AuctionItem : Entity<int> {
		public int SellerId { get; set; }
		public AuctionItemStatusCodeId AuctionItemStatusCodeId { get; set; }
		public int AuctionItemCategoryId { get; set; }
		
		[StringLength(1024)]
		public string Description { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime PlannedCloseDate { get; set; }
		public DateTime? ActualCloseDate { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal StartingPrice { get; set; }
		public int? WinningBidId { get; set; }
		
		
		public Bid WinningBid { get; set; }
		public ICollection<Bid> Bids { get; set; }
		public User Seller { get; set; }
		public AuctionItemCategory AuctionItemCategory { get; set; }
		public AuctionItemStatusCode AuctionItemStatusCode { get; set; }
	}

}