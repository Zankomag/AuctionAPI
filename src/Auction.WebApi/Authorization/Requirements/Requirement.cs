
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
		public const string OwnerOfAuctionItemImageId = nameof(OwnerOfAuctionItemImageIdRequirement);

		/// <summary>
		/// Joins policies with "Or"
		/// </summary>
		public static string GetOrCombinedPolicy(params string[] policies) => String.Join("Or", policies);

		/// <summary>
		/// Adds "Except" before policy
		/// </summary>
		public static string GetExceptPolicy(string policy) => String.Concat("Except", policy);

		//TODO add requirement factory that accepts name of requirement and returns type
		//TODO add dynamic OR-combined requirement creation (based on AuthorizeAny attribute usage)

	}

}