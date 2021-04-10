using System;
using Microsoft.AspNetCore.Authorization;

namespace Auction.WebApi.Authorization.Attributes {

	/// <inheritdoc />
	/// <remarks>
	///     Authorization is successful if any of requirements is met
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	public class AuthorizeExceptAttribute : AuthorizeAttribute {

		public string[] Policies { get; }

		public AuthorizeExceptAttribute(string policy, string[] policies) : base(Requirement.GetExceptPolicy(policy))
			=> Policies = policies;
	}

}