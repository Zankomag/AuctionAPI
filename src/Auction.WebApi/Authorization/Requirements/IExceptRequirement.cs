using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     This requirement fails if requirement other handlers succeed requirement inherited from this
	/// </summary>
	public interface IExceptRequirement : IAuthorizationRequirement { }

}