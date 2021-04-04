using System;
using System.Threading.Tasks;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Extensions;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public static partial class Requirement {
		public abstract partial class IsAdminOrOwnerOf {
			public partial class AuctionItemId {
				public class Handler : AuthorizationHandler<AuctionItemId> {

					private readonly IAuctionItemService auctionItemService;
					private readonly IRequestData requestData;

					public Handler(IAuctionItemService auctionItemService, IRequestData requestData) {
						this.auctionItemService = auctionItemService;
						this.requestData = requestData;
					}

					protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
						AuctionItemId requirement) {

						if(!context.IsAlreadyDetermined<AuctionItemId>()
							&& requestData.RouteIdString != null && requestData.UserId != null
							&& Int32.TryParse(requestData.RouteIdString, out int auctionItemId)
							&& await auctionItemService.IsUserOwner(auctionItemId, requestData.UserId.Value)) {

							context.Succeed(requirement);
						}
					}
				}
			}
		}
	}

}