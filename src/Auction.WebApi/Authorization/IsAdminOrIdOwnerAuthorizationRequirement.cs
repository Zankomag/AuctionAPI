using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage



namespace Auction.WebApi.Authorization {

	/// <summary>
	///     Policy requirement that authorizes only Admins or Users that do own 'id' parameter of request (self Id owners)
	/// </summary>
	public class IsAdminOrIdOwnerAuthorizationRequirement : IAuthorizationRequirement { }

}