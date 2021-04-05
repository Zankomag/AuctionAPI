
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable CheckNamespace

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Policy requirement that authorizes only Users that do own AuctionItem 'id' parameter of request
	/// </summary>
	public class AdminOrOwnerOfAuctionItemIdRequirement : IOwnerOfAuctionItemIdRequirement, IAdminRequirement { }

}