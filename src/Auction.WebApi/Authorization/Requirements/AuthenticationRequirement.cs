using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Fails if user is not authenticated
	/// </summary>
	public class AuthenticationRequirement : IAuthorizationRequirement { }


}