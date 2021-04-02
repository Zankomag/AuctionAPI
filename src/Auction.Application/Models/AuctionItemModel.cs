using System;
using System.Collections.Generic;
using Auction.Core.Entities;

namespace Auction.Application.Models {

	public class AuctionItemModel : AuctionItemInputModel {
		public AuctionItemStatusCodeId AuctionItemStatusCodeId { get; set; }

		public DateTime? ActualCloseDate { get; set; }
		public int? WinningBidId { get; set; }


		public IEnumerable<FileModel> Images { get; set; }
		public BidModel WinningBid { get; set; }
		public IEnumerable<BidModel> Bids { get; set; }
		public UserModel Seller { get; set; }
		public AuctionItemCategoryInputModel AuctionItemCategory { get; set; }
	}

}