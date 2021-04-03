using System;
using System.Threading.Tasks;
using Auction.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public static partial class Requirement {
		public abstract partial class IsAdminOrOwnerOf {
			public partial class AuctionItemId {
				public class Handler : IsAdminOrOwnerOfHandler<AuctionItemId> {

					private readonly IAuctionItemService auctionItemService;

					public Handler(IAuctionItemService auctionItemService,
						TokenValidationHandler tokenValidationHandler) : base(tokenValidationHandler)
						=> this.auctionItemService = auctionItemService;

					/// <inheritdoc />
					protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
						AuctionItemId requirement) {

						await base.HandleRequirementAsync(context, requirement);

						if(!context.HasSucceeded && !context.HasFailed) {
							if(RouteIdString != null
								&& Int32.TryParse(RouteIdString, out int auctionItemId)
								&& await auctionItemService.IsUserOwner(auctionItemId, TokenValidationHandler.UserId)) {

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