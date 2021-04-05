
// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Policy requirements accessor
	/// </summary>
	public static class Requirement {
		public const string Admin = nameof(AdminRequirement);
		public const string OwnerOfUserId = nameof(OwnerOfUserIdRequirement);
		public const string OwnerOfAuctionItemId = nameof(OwnerOfAuctionItemIdRequirement);
		public const string AdminOrOwnerOfUserId = nameof(AdminOrOwnerOfUserIdRequirement);
		public const string AdminOrOwnerOfAuctionItemId = nameof(AdminOrOwnerOfAuctionItemIdRequirement);

		//TODO add factory that accepts name of requirement and returns type
	}

}