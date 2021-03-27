using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AuctionAPI.Core.Entities {

	public class User : Entity<int> {
		[Required]
		[StringLength(50)]
		public string FirstName { get; set; }

		[StringLength(50)]
		public string LastName { get; set; }

		[StringLength(50)]
		public string Email { get; set; }

		[JsonIgnore]
		[StringLength(64)]
		public string PasswordHash { get; set; }


		public Bidder Bidder { get; set; }
		public Seller Seller { get; set; }
	}

}