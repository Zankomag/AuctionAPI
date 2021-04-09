
// ReSharper disable InheritdocConsiderUsage

using System;
using System.Collections.Generic;
using System.Linq;
using ImpromptuInterface;
using Microsoft.AspNetCore.Authorization;

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Policy requirements accessor
	/// </summary>
	public static class Requirement {
		public const string Admin = nameof(AdminRequirement);
		public const string OwnerOfUserId = nameof(OwnerOfUserIdRequirement);
		public const string OwnerOfAuctionItemId = nameof(OwnerOfAuctionItemIdRequirement);
		public const string OwnerOfAuctionItemImageId = nameof(OwnerOfAuctionItemImageIdRequirement);
		
		private static readonly Dictionary<string, Type> baseRequirementTypes = 
			new Dictionary<string, Type>();
		
		private static readonly Dictionary<string, IAuthorizationRequirement> requirements =
			new Dictionary<string, IAuthorizationRequirement>();

		static Requirement() {
			InitializeRequirements();
			InitializeRequirementTypes();
		}

		private static void InitializeRequirementTypes() {
			AddRequirementType(Admin, typeof(IAdminRequirement));
			AddRequirementType(OwnerOfUserId, typeof(IOwnerOfUserIdRequirement));
			AddRequirementType(OwnerOfAuctionItemId, typeof(IOwnerOfAuctionItemIdRequirement));
			AddRequirementType(OwnerOfAuctionItemImageId, typeof(IOwnerOfAuctionItemImageIdRequirement));
		}

		private static void AddRequirementType(string key, Type type) {
			if(!type.IsInterface) {
				throw new ArgumentException($"Type must be interface", nameof(type));
			}
			if(!type.IsAssignableTo(typeof(IAuthorizationRequirement))) {
				throw new ArgumentException($"Type must be assignable to {nameof(IAuthorizationRequirement)}",
					nameof(type));
			}
			baseRequirementTypes.TryAdd(key, type);
		}
		
		private static void InitializeRequirements() {
			requirements.Add(Admin, new AdminRequirement());
			requirements.Add(OwnerOfUserId, new OwnerOfUserIdRequirement());
			requirements.Add(OwnerOfAuctionItemId, new OwnerOfAuctionItemIdRequirement());
			requirements.Add(OwnerOfAuctionItemImageId, new OwnerOfAuctionItemImageIdRequirement());
		}
		
		/// <summary>
		/// Joins policies with "Or"
		/// </summary>
		public static string GetOrCombinedPolicy(params string[] policies) => String.Join("Or", policies);

		/// <summary>
		/// Adds "Except" before policy
		/// </summary>
		public static string GetExceptPolicy(string policy) => String.Concat("Except", policy);

		private static void AddPolicy(AuthorizationOptions options, string policy,
			IAuthorizationRequirement requirement)
			=> options.AddPolicy(policy, policyBuilder => policyBuilder.AddRequirements(requirement));

		public static void AddBasePolicy(AuthorizationOptions options, string policy) {
			if(!requirements.TryGetValue(policy, out IAuthorizationRequirement requirement))
				throw new ArgumentException($"Requirement for {policy} policy doesn't exist", nameof(policy));
			AddPolicy(options, policy, requirement);
		}
		
		public static void AddOrCombinedPolicy(AuthorizationOptions options, params string[] policies) {
			if(policies.Length != policies.Distinct().Count()) {
				throw new ArgumentException("Policies have duplicates", nameof(policies));
			}
			string orCombinedPolicy = GetOrCombinedPolicy(policies);
			var requirement = GetCombinedRequirement(orCombinedPolicy, policies);
			AddPolicy(options, orCombinedPolicy, requirement);
		}

		private static IAuthorizationRequirement GetCombinedRequirement(string combinedPolicy,
			params string[] policies) {

			if(requirements.TryGetValue(combinedPolicy, out IAuthorizationRequirement requirement))
				return requirement;
			return CreateCombinedRequirement(policies, combinedPolicy);
		}

		private static IAuthorizationRequirement CreateCombinedRequirement(string[] policies, string combinedPolicy) {
			Type[] requirementTypes = new Type[policies.Length];
			for(int i = 0; i < policies.Length; i++) {
				if(!baseRequirementTypes.TryGetValue(policies[i], out Type requirementType))
					throw new ArgumentException($"Requirement for {policies[i]} policy doesn't exist", nameof(policies));
				requirementTypes[i] = requirementType;
			}
			IAuthorizationRequirement newRequirement = new { }.ActLike(requirementTypes);
			requirements.Add(combinedPolicy, newRequirement);
			return newRequirement;
		}

		//TODO register IExceptRequirement is base requirements
		//TODO registed or requirement handlers here
		//TODO add GetExceptPolicy
		//TODO try to get all policies from attributes and register them


	}

}