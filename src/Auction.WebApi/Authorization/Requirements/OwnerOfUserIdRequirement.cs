using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Policy requirement that authorizes only Users that do own User 'id' parameter of request (self Id owners)
	/// </summary>
	public interface IOwnerOfUserIdRequirement : IAuthorizationRequirement { }
	
	/// <inheritdoc cref="IOwnerOfUserIdRequirement"/>
	public class OwnerOfUserIdRequirement : IOwnerOfUserIdRequirement { }

}