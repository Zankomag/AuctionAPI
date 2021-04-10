using System;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Extensions;
using Auction.WebApi.Authorization.Types;
using Microsoft.AspNetCore.Authorization;

namespace Auction.WebApi.Authorization.Requirements {

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

		/// <summary>
		/// Checks whether request data and user identity are valid and if requirement still needs validation
		/// </summary>
		/// <param name="userIdentity"></param>
		/// <param name="routeId">'id' value of request route</param>
		/// <param name="context"></param>
		protected bool IsContextValid(AuthorizationHandlerContext context, out UserIdentity userIdentity, out int routeId) {
			userIdentity = null;
			routeId = 0;
			return RequestData.RouteIdValue != null
				&& !context.IsAlreadyDetermined<TRequirement>()
				&& context.User.TryGetUserIdentity(out userIdentity)
				&& Int32.TryParse(RequestData.RouteIdValue, out routeId);
		}
	}

}