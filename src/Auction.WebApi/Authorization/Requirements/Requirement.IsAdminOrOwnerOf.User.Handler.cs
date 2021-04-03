using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public static partial class Requirement {
		public abstract partial class IsAdminOrOwnerOf {
			public partial class UserId {
				public sealed class Handler : IsAdminOrOwnerOfHandler<UserId> {

					public Handler(TokenValidationHandler tokenValidationHandler) : base(tokenValidationHandler) { }

					/// <inheritdoc />
					protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
						UserId requirement) {

						await base.HandleRequirementAsync(context, requirement);

						if(!context.HasSucceeded && !context.HasFailed) {
							if(RouteIdString != null
								&& RouteIdString == TokenValidationHandler.UserIdString) {

								context.Succeed(requirement);
								return;
							}

							context.Fail();
						}
					}
				}
			}
		}
	}

}