
// ReSharper disable InheritdocConsiderUsage
// ReSharper disable CheckNamespace

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Policy requirement that authorizes only Admins or Users that do own User 'id' parameter of request (self Id owners)
	/// </summary>
	public class AdminOrOwnerOfUserIdRequirement : IOwnerOfUserIdRequirement, IAdminRequirement { }

}