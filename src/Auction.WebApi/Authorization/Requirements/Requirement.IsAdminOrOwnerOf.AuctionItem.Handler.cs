﻿using System;
using System.Threading.Tasks;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization.Abstractions;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public static partial class Requirement {
		public abstract partial class IsAdminOrOwnerOf {
			public partial class AuctionItemId {
				public class Handler : IsAdminOrOwnerOfHandler<AuctionItemId> {

					private readonly IAuctionItemService auctionItemService;
					private readonly IRequestData requestData;

					public Handler(IAuctionItemService auctionItemService, IRequestData requestData) {
						this.auctionItemService = auctionItemService;
						this.requestData = requestData;
					}

					/// <inheritdoc />
					protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
						AuctionItemId requirement) {

						await base.HandleRequirementAsync(context, requirement);

						if(!context.HasSucceeded && !context.HasFailed) {
							if(requestData.RouteIdString != null && requestData.UserId != null
								&& Int32.TryParse(requestData.RouteIdString, out int auctionItemId)
								&& await auctionItemService.IsUserOwner(auctionItemId, requestData.UserId.Value)) {

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