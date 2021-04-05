
// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Policy requirements accessor
	/// </summary>
	public static class Requirement {
		private const string ownerOf = "OwnerOf";
		private const string or = "Or";
		public const string Admin = "Admin";
		public const string OwnerOfUserId = ownerOf + "UserId";
		public const string OwnerOfAuctionItemId = ownerOf + "AuctionItemId";
		public const string AdminOrOwnerOfUserId = Admin + or + OwnerOfUserId;
		public const string AdminOrOwnerOfAuctionItemId = Admin + or + OwnerOfAuctionItemId;
	}

}