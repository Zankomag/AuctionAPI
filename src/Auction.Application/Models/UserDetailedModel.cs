using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.Application.Models {

	public class UserDetailedModel : UserModel {

		//email is Username
		[StringLength(50)]
		[Required]
		public string Email { get; set; }

		[StringLength(20)]
		[Required]
		public List<string> Roles { get; set; }
	}

}