using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.Core.Entities {

	public class User : Entity<int> {
		[Required]
		[StringLength(50)]
		public string FirstName { get; set; }

		[StringLength(50)]
		public string LastName { get; set; }

		//email is Username
		[StringLength(50)]
		[Required]
		public string Email { get; set; }
		
		[StringLength(128)]
		[Required]
		public string PasswordHash { get; set; }

		[MaxLength(32)]
		[Required]
		public byte[] PasswordSalt { get; set; }



		public ICollection<UserRole> Roles { get; set; }
		public ICollection<Bid> Bids { get; set; }
		public ICollection<AuctionItem> AuctionItems { get; set; }
	}

}