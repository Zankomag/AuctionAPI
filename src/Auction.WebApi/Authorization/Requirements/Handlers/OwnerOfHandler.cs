using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Extensions;
using Auction.WebApi.Authorization.Types;
using Microsoft.AspNetCore.Authorization;

namespace Auction.WebApi.Authorization.Requirements.Handlers {

	public abstract class OwnerOfHandler<TRequirement> : AuthorizationHandler<TRequirement>
		where TRequirement : IAuthorizationRequirement {

		protected readonly IRequestData RequestData;

		protected OwnerOfHandler(IRequestData requestData) => this.RequestData = requestData;

		/// <summary>
		/// Checks whether request data and user identity are valid and if requirement still needs validation
		/// </summary>
		protected bool IsContextValid(AuthorizationHandlerContext context, out UserIdentity userIdentity) {
			userIdentity = null;
			return RequestData.RouteIdValue != null 
				&& !context.IsAlreadyDetermined<TRequirement>()
				&& context.User.TryGetUserIdentity(out userIdentity);
		}
	}

}