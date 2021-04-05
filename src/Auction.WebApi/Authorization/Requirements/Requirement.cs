
// ReSharper disable InheritdocConsiderUsage

using System;

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Policy requirements accessor
	/// </summary>
	public static class Requirement {
		public const string Admin = nameof(AdminRequirement);
		public const string OwnerOfUserId = nameof(OwnerOfUserIdRequirement);
		public const string OwnerOfAuctionItemId = nameof(OwnerOfAuctionItemIdRequirement);

		/// <summary>
		/// Joins policies with "Or"
		/// </summary>
		public static string GetOrCombinedPolicy(params string[] policies) => String.Join("Or", policies);
		
		//TODO add requirement factory that accepts name of requirement and returns type
		//TODO add base class for ownerOf handlers
		
	}

}