using System.ComponentModel.DataAnnotations;

namespace Auction.Application.Models {

	public class AuthenticationModel {
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }
	}

}