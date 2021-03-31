using System.ComponentModel.DataAnnotations;
using Auction.Application.Models.Generic;

namespace Auction.Application.Models {
	public class UserInputModel : Model<int> {
		[Required]
		[StringLength(50)]
		public string FirstName { get; set; }

		[StringLength(50)]
		public string LastName { get; set; }

		//email is Username
		[EmailAddress]
		[StringLength(50)]
		[Required]
		public string Email { get; set; }
		
		//MinimumLength = 8 only for test purpose
		[StringLength(64, MinimumLength = 8)]
		[Required]
		public string Password { get; set; }
		
	}
}
