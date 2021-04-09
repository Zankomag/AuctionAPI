using System;
using System.Threading.Tasks;
using Auction.Application.Authorization;
using Auction.WebApi.Authorization.Extensions;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements.Handlers {

	/// <summary>
	///     Succeeds if user role is Admin, does not fail
	/// </summary>
	public class AdminRequirementHandler : AuthorizationHandler<IAdminRequirement> {

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
			IAdminRequirement requirement) {

			if(!context.IsAlreadyDetermined<IAdminRequirement>()
				&& context.User.IsInRole(Role.Admin)) {
				
				context.Succeed(requirement);
			}
			return Task.CompletedTask;
		}
	}

}