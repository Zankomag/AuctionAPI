using System.ComponentModel.DataAnnotations;
using Auction.Application.Models.Generic;

namespace Auction.Application.Models {

	public class UserDetailedModel : Model<int> {
		[Required]
		[StringLength(50)]
		public string FirstName { get; set; }

		[StringLength(50)]
		public string LastName { get; set; }

		//email is Username
		[StringLength(50)]
		[Required]
		public string Email { get; set; }

		[StringLength(20)]
		[Required]
		public string Role { get; set; }
	}

}