using System.Security.Claims;
using System.Threading.Tasks;
using Auction.Application.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization {

	/// <summary>
	///     Policy requirement that authorizes only Admins or Users that do own 'id' parameter of request (self Id owners)
	/// </summary>
	public class IsAdminOrIdOwnerAuthorizationRequirementHandler
		: AuthorizationHandler<IsAdminOrIdOwnerAuthorizationRequirement> {

		private readonly IHttpContextAccessor httpContextAccessor;

		public IsAdminOrIdOwnerAuthorizationRequirementHandler(IHttpContextAccessor httpContextAccessor)
			=> this.httpContextAccessor = httpContextAccessor;

		/// <inheritdoc />
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
			IsAdminOrIdOwnerAuthorizationRequirement requirement) {

			if(context.User.IsInRole(Role.Admin)) {
				context.Succeed(requirement);
				return Task.CompletedTask;
			}

			var userId = context.User.FindFirstValue(JwtOpenIdProperty.Sub);
			if(userId != null) {
				var routeData = httpContextAccessor.HttpContext!.GetRouteData();

				if(routeData.Values["id"] is string id && id == userId) {
					context.Succeed(requirement);
					return Task.CompletedTask;
				}
			}

			context.Fail();
			return Task.CompletedTask;
		}
	}

}