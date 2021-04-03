﻿using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public static partial class Requirement {
		public abstract partial class IsAdminOrOwnerOf {
			/// <summary>
			///     Policy requirement that authorizes only Admins or Users that do own AuctionItem 'id' parameter of request
			/// </summary>
			public partial class AuctionItemId : IAuthorizationRequirement {
				public const string Policy = IsAdminOrOwnerOf.policy + nameof(AuctionItemId);

				private AuctionItemId() { }
				
				public static IAuthorizationRequirement Get => new AuctionItemId();
				
			}
		}
	}

}