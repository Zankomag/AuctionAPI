namespace Auction.Core.Entities {

	public class UserUserRole {
		public int UserId { get; set; }
		public int UserRoleId { get; set; }
		
		public User User { get; set; }
		public UserRole UserRole { get; set; }
	}

}