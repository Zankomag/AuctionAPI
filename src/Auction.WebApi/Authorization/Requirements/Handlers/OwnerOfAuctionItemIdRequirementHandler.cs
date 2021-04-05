using System;
using System.Threading.Tasks;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Extensions;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements.Handlers {

	public class OwnerOfAuctionItemIdRequirementHandler : AuthorizationHandler<IOwnerOfAuctionItemIdRequirement> {

		private readonly IAuctionItemService auctionItemService;
		private readonly IRequestData requestData;

		public OwnerOfAuctionItemIdRequirementHandler(IAuctionItemService auctionItemService,
			IRequestData requestData) {
			this.auctionItemService = auctionItemService;
			this.requestData = requestData;
		}

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
			IOwnerOfAuctionItemIdRequirement requirement) {

			if(!context.IsAlreadyDetermined<IOwnerOfAuctionItemIdRequirement>()
				&& requestData.RouteIdString != null && requestData.UserId != null
				&& Int32.TryParse(requestData.RouteIdString, out int auctionItemId)
				&& await auctionItemService.IsUserOwner(auctionItemId, requestData.UserId.Value)) {

				context.Succeed(requirement);
			}
		}
	}

}