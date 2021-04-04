using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Policy requirement that authorizes only Admins or Users that do own AuctionItem 'id' parameter of request
	/// </summary>
	public class OwnerOfAuctionItemIdRequirement : AdminRequirement {
		public const string Policy = AdminRequirement.Policy + nameof(OwnerOfAuctionItemIdRequirement);

		public static IAuthorizationRequirement Get => new OwnerOfAuctionItemIdRequirement();

		private OwnerOfAuctionItemIdRequirement() { }
	}

}