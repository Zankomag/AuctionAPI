using Auction.WebApi.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace Auction.WebApi.Authorization.Extensions {

	public static class RequirementExtensions {

		public static void AddBasePolicy(this AuthorizationOptions options, string policy)
			=> Requirement.AddBasePolicy(options, policy);

		public static void AddOrCombinedPolicy(this AuthorizationOptions options, params string[] policies)
			=> Requirement.AddOrCombinedPolicy(options, policies);

		public static void AddExceptPolicy(this AuthorizationOptions options, params string[] policies)
			=> Requirement.AddExceptPolicy(options, policies);
	}

}