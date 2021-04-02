using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public static partial class Requirement {
		public abstract partial class IsAdminOrOwnerOf {
			public partial class User {

				/// <inheritdoc />
				public sealed class Handler : IsAdminHandler<User> {

					public Handler(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor) { }

					/// <inheritdoc />
					protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
						User requirement) {

						await base.HandleRequirementAsync(context, requirement);

						if(!context.HasSucceeded && !context.HasFailed) {
							if(RouteData.Values["id"] is string userIdString
								&& userIdString == UserIdString) {

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