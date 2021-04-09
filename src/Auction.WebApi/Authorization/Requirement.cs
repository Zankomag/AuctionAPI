// ReSharper disable InheritdocConsiderUsage

using System;
using System.Collections.Generic;
using System.Linq;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Extensions;
using Auction.WebApi.Authorization.Requirements;
using Auction.WebApi.Authorization.Requirements.Handlers;
using Auction.WebApi.Authorization.Services;
using ImpromptuInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.WebApi.Authorization {

	/// <summary>
	///     Policy requirements accessor and registrar
	/// </summary>
	public static class Requirement {
		private const string except = "Except";
		public const string Admin = nameof(AdminRequirement);
		public const string OwnerOfUserId = nameof(OwnerOfUserIdRequirement);
		public const string OwnerOfAuctionItemId = nameof(OwnerOfAuctionItemIdRequirement);
		public const string OwnerOfAuctionItemImageId = nameof(OwnerOfAuctionItemImageIdRequirement);

		private static readonly Dictionary<string, Type> baseRequirementTypes = new();

		private static readonly Dictionary<string, IAuthorizationRequirement> requirements = new();

		static Requirement() {
			InitializeBaseRequirements();
			InitializeBaseRequirementTypes();
		}

		private static void InitializeBaseRequirementTypes() {
			AddRequirementType(except, typeof(IExceptRequirement));
			AddRequirementType(Admin, typeof(IAdminRequirement));
			AddRequirementType(OwnerOfUserId, typeof(IOwnerOfUserIdRequirement));
			AddRequirementType(OwnerOfAuctionItemId, typeof(IOwnerOfAuctionItemIdRequirement));
			AddRequirementType(OwnerOfAuctionItemImageId, typeof(IOwnerOfAuctionItemImageIdRequirement));
		}

		private static void AddRequirementType(string key, Type type) {
			if(!type.IsInterface) {
				throw new ArgumentException("Type must be interface", nameof(type));
			}
			if(!type.IsAssignableTo(typeof(IAuthorizationRequirement))) {
				throw new ArgumentException($"Type must be assignable to {nameof(IAuthorizationRequirement)}",
					nameof(type));
			}
			baseRequirementTypes.TryAdd(key, type);
		}

		private static void InitializeBaseRequirements() {
			requirements.Add(Admin, new AdminRequirement());
			requirements.Add(OwnerOfUserId, new OwnerOfUserIdRequirement());
			requirements.Add(OwnerOfAuctionItemId, new OwnerOfAuctionItemIdRequirement());
			requirements.Add(OwnerOfAuctionItemImageId, new OwnerOfAuctionItemImageIdRequirement());
		}

		/// <summary>
		///     Joins policies with "Or"
		/// </summary>
		public static string GetOrCombinedPolicy(params string[] policies) => String.Join("Or", policies);

		/// <summary>
		///     Adds "Except" before policy
		/// </summary>
		public static string GetExceptPolicy(params string[] policies)
			=> String.Concat(except, GetOrCombinedPolicy(policies));

		// ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
		private static void ValidatePolicies(string[] policies) {
			if(policies is null) throw new ArgumentNullException(nameof(policies));
			if(policies.Length != policies.Distinct().Count()) {
				throw new ArgumentException("Policies have duplicates", nameof(policies));
			}
		}

		private static void AddPolicy(AuthorizationOptions options, string policy,
			IAuthorizationRequirement requirement)
			=> options.AddPolicy(policy, policyBuilder => policyBuilder.AddRequirements(requirement));

		public static void AddBasePolicy(AuthorizationOptions options, string policy) {
			if(!requirements.TryGetValue(policy, out IAuthorizationRequirement requirement))
				throw new ArgumentException($"Requirement for {policy} policy doesn't exist", nameof(policy));
			AddPolicy(options, policy, requirement);
		}

		public static void AddExceptPolicy(AuthorizationOptions options, params string[] policies) {
			ValidatePolicies(policies);
			string exceptPolicy = GetExceptPolicy(policies);
			var requirement = GetCombinedRequirement(exceptPolicy, policies.Append(except).ToArray());
			AddPolicy(options, exceptPolicy, requirement);
		}

		public static void AddOrCombinedPolicy(AuthorizationOptions options, params string[] policies) {
			ValidatePolicies(policies);
			string orCombinedPolicy = GetOrCombinedPolicy(policies);
			var requirement = GetCombinedRequirement(orCombinedPolicy, policies);
			AddPolicy(options, orCombinedPolicy, requirement);
		}

		private static IAuthorizationRequirement GetCombinedRequirement(string combinedPolicy,
			params string[] policies) {

			if(requirements.TryGetValue(combinedPolicy, out IAuthorizationRequirement requirement))
				return requirement;
			var newRequirement = CreateCombinedRequirement(policies);
			requirements.Add(combinedPolicy, newRequirement);
			return newRequirement;
		}

		private static IAuthorizationRequirement CreateCombinedRequirement(string[] policies) {
			Type[] requirementTypes = new Type[policies.Length];
			for(int i = 0; i < policies.Length; i++) {
				if(!baseRequirementTypes.TryGetValue(policies[i], out Type requirementType))
					throw new ArgumentException($"Requirement for {policies[i]} policy doesn't exist",
						nameof(policies));
				requirementTypes[i] = requirementType;
			}
			IAuthorizationRequirement newRequirement = new { }.ActLike(requirementTypes);
			return newRequirement;
		}

		public static void RegisterAuthorizationHandlers(IServiceCollection services) {
			//This handler must be registered as the very first among all handlers
			services.AddSingleton<IAuthorizationHandler, AuthenticationRequirementHandler>();

			//Authorization handlers use request data
			services.AddScoped<IRequestData, RequestData>();
			services.AddHttpContextAccessor();

			//Order of handlers is important - it determines their execution order in request pipeline
			//
			//These services are scoped because they use scoped IRequestData and other scoped services,
			//otherwise they'd be singletons
			services.AddScoped<IAuthorizationHandler, AdminRequirementHandler>();
			services.AddScoped<IAuthorizationHandler, OwnerOfUserIdRequirementHandler>();
			services.AddScoped<IAuthorizationHandler, OwnerOfAuctionItemIdRequirementHandler>();
			services.AddScoped<IAuthorizationHandler, OwnerOfAuctionItemImageIdRequirementHandler>();

			//ExceptRequirement handler must be registered last
			//because requirement that he except-handles must be handled before
			services.AddSingleton<IAuthorizationHandler, ExceptRequirementHandler>();
		}

		public static void AddAuthorization(IServiceCollection services) {
			services.AddAuthorization(options => {
				//Override default 'DenyAnonymousAuthorizationRequirement' with similar policy
				//that fails context if user is not authenticated
				options.DefaultPolicy = new AuthorizationPolicyBuilder()
					.AddRequirements(new AuthenticationRequirement())
					.Build();
				options.AddExceptPolicy(OwnerOfAuctionItemId);
				options.AddBasePolicy(Admin);
				options.AddBasePolicy(OwnerOfAuctionItemId);
				options.AddOrCombinedPolicy(Admin, OwnerOfUserId);
				options.AddOrCombinedPolicy(Admin, OwnerOfAuctionItemId);
				options.AddOrCombinedPolicy(Admin, OwnerOfAuctionItemImageId);
			});

			RegisterAuthorizationHandlers(services);
		}

		//TODO try to get all policies from attributes and register them
	}

}