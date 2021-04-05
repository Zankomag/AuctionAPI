using System;
using System.Threading.Tasks;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Types;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements.Handlers {

	public class OwnerOfAuctionItemIdRequirementHandler : OwnerOfHandler<IOwnerOfAuctionItemIdRequirement> {

		private readonly IAuctionItemService auctionItemService;

		public OwnerOfAuctionItemIdRequirementHandler(IAuctionItemService auctionItemService,
			IRequestData requestData) : base(requestData)
			=> this.auctionItemService = auctionItemService;

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
			IOwnerOfAuctionItemIdRequirement requirement) {

			if(IsContextValid(context, out UserIdentity userIdentity)
				&& Int32.TryParse(RequestData.RouteIdValue, out int auctionItemId)
				&& await auctionItemService.IsUserOwner(auctionItemId, userIdentity.Id)) {

				context.Succeed(requirement);
			}
		}
	}

}