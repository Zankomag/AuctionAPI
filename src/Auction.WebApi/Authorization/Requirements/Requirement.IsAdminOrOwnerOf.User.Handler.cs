using System.Threading.Tasks;
using Auction.WebApi.Authorization.Abstractions;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public static partial class Requirement {
		public abstract partial class IsAdminOrOwnerOf {
			public partial class UserId {
				public sealed class Handler : AuthorizationHandler<UserId> {
					private readonly IRequestData requestData;

					public Handler(IRequestData requestData) => this.requestData = requestData;

					/// <inheritdoc />
					protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
						UserId requirement) {

						if(!context.HasSucceeded && !context.HasFailed) {
							if(requestData.RouteIdString != null
								&& requestData.RouteIdString == requestData.UserIdString) {

								context.Succeed(requirement);
								return;
							}

						//	context.Fail();
						}
					}
				}
			}
		}
	}

}