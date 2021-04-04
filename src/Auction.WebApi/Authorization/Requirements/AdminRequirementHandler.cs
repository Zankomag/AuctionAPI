using System.Threading.Tasks;
using Auction.Application.Authorization;
using Auction.WebApi.Authorization.Extensions;
using Microsoft.AspNetCore.Authorization;
// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Succeeds if user role is Admin, does not fail
	/// </summary>
	public class AdminRequirementHandler : AuthorizationHandler<AdminRequirement> {

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
			AdminRequirement requirement) {

			if(!context.IsAlreadyDetermined<AdminRequirement>()
				&& context.User.IsInRole(Role.Admin)) {

				context.Succeed(requirement);
			}
			return Task.CompletedTask;
		}
	}

}