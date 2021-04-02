using System;
using System.Threading.Tasks;
using Auction.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public static partial class Requirement {
		public abstract partial class IsAdminOrOwnerOf {
			/// <summary>
			///     Policy requirement that authorizes only Admins or Users that do own AuctionItem 'id' parameter of request
			/// </summary>
			public partial class AuctionItem {
				public class Handler : IsAdminHandler<AuctionItem> {

					private readonly IAuctionItemService auctionItemService;

					public Handler(IHttpContextAccessor httpContextAccessor, IAuctionItemService auctionItemService) :
						base(httpContextAccessor)
						=> this.auctionItemService = auctionItemService;

					/// <inheritdoc />
					protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
						AuctionItem requirement) {

						await base.HandleRequirementAsync(context, requirement);

						if(!context.HasSucceeded && !context.HasFailed) {
							if(RouteData.Values["id"] is string auctionItemIdString
								&& Int32.TryParse(auctionItemIdString, out int auctionItemId)
								&& await auctionItemService.IsUserOwner(auctionItemId, UserId)) {
								
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