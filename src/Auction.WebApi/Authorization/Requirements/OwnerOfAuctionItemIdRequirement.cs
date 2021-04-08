using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Policy requirement that authorizes only Users that do own AuctionItem 'id' parameter of request
	/// </summary>
	public interface IOwnerOfAuctionItemIdRequirement : IAuthorizationRequirement { }
	
	/// <inheritdoc cref="IOwnerOfAuctionItemIdRequirement"/>
	public class OwnerOfAuctionItemIdRequirement : IOwnerOfAuctionItemIdRequirement { }

}