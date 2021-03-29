using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuctionAPI.Core.Entities {

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
		
		[StringLength(64)]
		[Required]
		public string PasswordHash { get; set; }
		
		[StringLength(20)]
		[Required]
		public string Role { get; set; }


		public ICollection<Bid> Bids { get; set; }
		public ICollection<AuctionItem> AuctionItems { get; set; }
	}

}