// ReSharper disable InheritdocConsiderUsage

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Attributes;
using Auction.WebApi.Authorization.Requirements;
using Auction.WebApi.Authorization.Services;
using ImpromptuInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.WebApi.Authorization {

	/// <summary>
	///     Policy requirements accessor and registrar
	/// </summary>
	public static class Requirement {
		private const string except = "Except";
		public const string Admin = nameof(IAdminRequirement);
		public const string OwnerOfUserId = nameof(IOwnerOfUserIdRequirement);
		public const string OwnerOfAuctionItemId = nameof(IOwnerOfAuctionItemIdRequirement);
		public const string OwnerOfAuctionItemImageId = nameof(IOwnerOfAuctionItemImageIdRequirement);

		private static readonly Dictionary<string, Type> baseRequirementTypes = new();

		private static readonly Dictionary<string, IAuthorizationRequirement> requirements = new();

		static Requirement() => InitializeBaseRequirementTypes();

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

		private static void AddBasePolicy(AuthorizationOptions options, string policy)
			=> AddCombinedRequirement(options, policy, new[] {policy});

		private static void AddExceptPolicy(AuthorizationOptions options, params string[] policies) {
			ValidatePolicies(policies);
			string exceptPolicy = GetExceptPolicy(policies);
			AddCombinedRequirement(options, exceptPolicy, policies.Append(except).ToArray());
		}

		private static void AddOrCombinedPolicy(AuthorizationOptions options, params string[] policies) {
			ValidatePolicies(policies);
			string orCombinedPolicy = GetOrCombinedPolicy(policies);
			AddCombinedRequirement(options, orCombinedPolicy, policies);
		}

		private static void AddCombinedRequirement(AuthorizationOptions options, string combinedPolicy,
			string[] policies) {

			if(!requirements.ContainsKey(combinedPolicy)) {
				var newRequirement = CreateCombinedRequirement(policies);
				requirements.Add(combinedPolicy, newRequirement);
				AddPolicy(options, combinedPolicy, newRequirement);
			}
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

		private static void RegisterAuthorizationHandlers(IServiceCollection services) {
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
				RegisterAllRequirements(options, Assembly.GetExecutingAssembly());
			});

			RegisterAuthorizationHandlers(services);

		}

		private static void RegisterAllRequirements(AuthorizationOptions options, Assembly assembly) {
			var authorizeAttributes = GetAuthorizeAttributes(assembly);

			var baseAuthorizeAttributes = authorizeAttributes
				.Where(x => x.GetType() == typeof(AuthorizeAttribute));
			foreach(var attribute in baseAuthorizeAttributes) {
				AddBasePolicy(options, attribute.Policy);
			}

			var authorizeAnyAttributes = authorizeAttributes.OfType<AuthorizeAnyAttribute>();
			foreach(var attribute in authorizeAnyAttributes) {
				AddOrCombinedPolicy(options, attribute.Policies);
			}

			var authorizeExceptAttributes = authorizeAttributes.OfType<AuthorizeExceptAttribute>();
			foreach(var attribute in authorizeExceptAttributes) {
				AddExceptPolicy(options, attribute.Policies);
			}

		}

		private static List<AuthorizeAttribute> GetAuthorizeAttributes(Assembly assembly) {
			List<AuthorizeAttribute> result = new List<AuthorizeAttribute>();

			foreach(Type type in assembly.GetTypes().Where(type => type.IsAssignableTo(typeof(ControllerBase)))) {
				AddAttributes(type);
				foreach(var methodInfo in type.GetMethods()) {
					AddAttributes(methodInfo);
				}
			}

			return result;

			void AddAttributes(MemberInfo memberInfo) {
				var classAttributes = Attribute.GetCustomAttributes(memberInfo, typeof(AuthorizeAttribute))
					.Cast<AuthorizeAttribute>()
					.Where(x => x.Policy != null);
				result.AddRange(classAttributes);
			}
		}
	}

}