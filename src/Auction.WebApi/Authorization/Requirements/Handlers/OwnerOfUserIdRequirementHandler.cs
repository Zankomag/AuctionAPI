using System.Threading.Tasks;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Extensions;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements.Handlers {

	public sealed class OwnerOfUserIdRequirementHandler : AuthorizationHandler<IOwnerOfUserIdRequirement> {
		private readonly IRequestData requestData;

		public OwnerOfUserIdRequirementHandler(IRequestData requestData) => this.requestData = requestData;

		/// <inheritdoc />
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
			IOwnerOfUserIdRequirement requirement) {

			if(!context.IsAlreadyDetermined<IOwnerOfUserIdRequirement>()
				&& requestData.RouteIdString != null
				&& requestData.RouteIdString == requestData.UserIdString) {

				context.Succeed(requirement);
			}
			return Task.CompletedTask;
		}
	}

}