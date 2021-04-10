using System.Threading.Tasks;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Types;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements.Handlers {

	/// <summary>
	///     Policy requirement that authorizes only Users that do own User 'id' parameter of request (self Id owners)
	/// </summary>
	public interface IOwnerOfUserIdRequirement : IAuthorizationRequirement { }

	public sealed class OwnerOfUserIdRequirementHandler : OwnerOfHandler<IOwnerOfUserIdRequirement> {

		public OwnerOfUserIdRequirementHandler(IRequestData requestData) : base(requestData) { }

		/// <inheritdoc />
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
			IOwnerOfUserIdRequirement requirement) {

			if(IsContextValid(context, out UserIdentity userIdentity)
				&& RequestData.RouteIdValue == userIdentity.IdString) {

				context.Succeed(requirement);
			}
			return Task.CompletedTask;
		}
	}

}