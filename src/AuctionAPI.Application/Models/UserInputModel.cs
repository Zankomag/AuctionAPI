using System.ComponentModel.DataAnnotations;
using AuctionAPI.Application.Models.Generic;

namespace AuctionAPI.Application.Models {
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

		[StringLength(64)]
		[Required]
		public string Password { get; set; }
		
	}
}
