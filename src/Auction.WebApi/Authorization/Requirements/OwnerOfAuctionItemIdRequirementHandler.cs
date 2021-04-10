using System.Threading.Tasks;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Types;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Policy requirement that authorizes only Users that do own AuctionItem 'id' parameter of request
	/// </summary>
	public interface IOwnerOfAuctionItemIdRequirement : IAuthorizationRequirement { }
	
	public class OwnerOfAuctionItemIdRequirementHandler : OwnerOfHandler<IOwnerOfAuctionItemIdRequirement> {

		private readonly IAuctionItemService auctionItemService;

		public OwnerOfAuctionItemIdRequirementHandler(IAuctionItemService auctionItemService,
			IRequestData requestData) : base(requestData)
			=> this.auctionItemService = auctionItemService;

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
			IOwnerOfAuctionItemIdRequirement requirement) {

			if(IsContextValid(context, out UserIdentity userIdentity, out int auctionItemId)
				&& await auctionItemService.IsUserOwner(auctionItemId, userIdentity.Id)) {

				context.Succeed(requirement);
			}
		}
	}

}