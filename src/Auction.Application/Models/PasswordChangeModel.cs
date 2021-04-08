using System.ComponentModel.DataAnnotations;

namespace Auction.Application.Models {

	public class PasswordChangeModel : AuthenticationModel {
		[StringLength(64, MinimumLength = 8)]
		[Required]
		public string NewPassword { get; set; }
	}

}