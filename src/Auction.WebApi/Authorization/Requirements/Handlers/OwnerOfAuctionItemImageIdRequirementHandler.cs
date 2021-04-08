using System.Threading.Tasks;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Types;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements.Handlers {

	public class OwnerOfAuctionItemImageIdRequirementHandler : OwnerOfHandler<IOwnerOfAuctionItemImageIdRequirement> {

		private readonly IAuctionItemService auctionItemService;

		public OwnerOfAuctionItemImageIdRequirementHandler(IAuctionItemService auctionItemService,
			IRequestData requestData) : base(requestData)
			=> this.auctionItemService = auctionItemService;

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
			IOwnerOfAuctionItemImageIdRequirement requirement) {

			if(IsContextValid(context, out UserIdentity userIdentity, out int auctionItemImageId)
				&& await auctionItemService.IsUserImageOwner(auctionItemImageId, userIdentity.Id)) {

				context.Succeed(requirement);
			}
		}
	}

}