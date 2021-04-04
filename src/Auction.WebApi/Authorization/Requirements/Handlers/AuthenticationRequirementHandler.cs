using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements.Handlers {
	
	//If IAuthorizationRequirement is combined with AuthorizationHandler
	//then somehow HandleRequirementAsync is called twice, so we need to separate them
	public class AuthenticationRequirementHandler : AuthorizationHandler<AuthenticationRequirement> {
		/// <inheritdoc />
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
			AuthenticationRequirement requirement) {
			if(context.User.Identity?.IsAuthenticated == true) {
				context.Succeed(requirement);
			} else {
				context.Fail();
			}
			return Task.CompletedTask;
		}
	}


}