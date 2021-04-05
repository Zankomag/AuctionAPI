using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public interface IAdminRequirement : IAuthorizationRequirement { }
	
	/// <inheritdoc cref="IAdminRequirement"/>
	public class AdminRequirement : IAdminRequirement { }

}