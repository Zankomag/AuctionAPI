using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <inheritdoc />
	public class AdminRequirement : IAuthorizationRequirement {

		public const string Policy = nameof(AdminRequirement);
	}

}