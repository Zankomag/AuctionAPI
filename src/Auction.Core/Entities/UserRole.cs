using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Auction.Core.Entities {

	public class UserRole : Entity<int> {
		[Required]
		[StringLength(20)]
		public string Name { get; set; }
		public ICollection<User> Users { get; set; }
	}

}