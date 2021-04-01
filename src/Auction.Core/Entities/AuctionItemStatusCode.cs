using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.Core.Entities {

	//TODO do with class with string constants:
	//one class represents AuctionItemStatusCodeId with
	//ToString override (private field created in constructor)
	//and Id init; field
	public enum AuctionItemStatusCodeId {
		Scheduled = 0,
		Started = 1,
		Finished = 2
	}


	public class AuctionItemStatusCode : Entity<AuctionItemStatusCodeId> {
		[Required]
		[StringLength(30)]
		public string Name { get; set; }


		public ICollection<AuctionItem> AuctionItems { get; set; }
	}

}