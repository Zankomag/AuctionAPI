namespace AuctionAPI.Application.Authorization {

	//TODO roles need to be saved in db and one user can have many roles
	//and all roles have to be retrieved from db on authentication
	public static class Role {
		public const string Admin = "Admin";
		public const string User = "User";
	}

}