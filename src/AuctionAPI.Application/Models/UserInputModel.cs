using System.ComponentModel.DataAnnotations;

namespace AuctionAPI.Application.Models {
	public class UserInputModel {
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
		public string Password { get; set; }
		
	}
}
