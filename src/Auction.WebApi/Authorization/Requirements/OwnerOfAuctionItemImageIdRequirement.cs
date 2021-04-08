using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Policy requirement that authorizes only Users that do own AuctionItemImage 'id' parameter of request
	/// </summary>
	public interface IOwnerOfAuctionItemImageIdRequirement : IAuthorizationRequirement { }
	
	/// <inheritdoc cref="IOwnerOfAuctionItemImageIdRequirement"/>
	public class OwnerOfAuctionItemImageIdRequirement : IOwnerOfAuctionItemImageIdRequirement { }
	

}