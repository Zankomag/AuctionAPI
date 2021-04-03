using System;
using System.Threading.Tasks;
using Auction.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public static partial class Requirement {
		public abstract partial class IsAdminOrOwnerOf {
			/// <summary>
			///     Policy requirement that authorizes only Admins or Users that do own AuctionItem 'id' parameter of request
			/// </summary>
			public partial class AuctionItem {
				public class Handler : IsAdminOrOwnerOfHandler<AuctionItem> {

					private readonly TokenValidationHandler tokenValidationHandler;
					private readonly IAuctionItemService auctionItemService;

					public Handler(TokenValidationHandler tokenValidationHandler,
						IAuctionItemService auctionItemService) {
						
						this.tokenValidationHandler = tokenValidationHandler;
						this.auctionItemService = auctionItemService;
					}

					/// <inheritdoc />
					protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
						AuctionItem requirement) {

						await base.HandleRequirementAsync(context, requirement);

						if(!context.HasSucceeded && !context.HasFailed) {
							if(tokenValidationHandler.RouteData.Values["id"] is string auctionItemIdString
								&& Int32.TryParse(auctionItemIdString, out int auctionItemId)
								&& await auctionItemService.IsUserOwner(auctionItemId, tokenValidationHandler.UserId)) {

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