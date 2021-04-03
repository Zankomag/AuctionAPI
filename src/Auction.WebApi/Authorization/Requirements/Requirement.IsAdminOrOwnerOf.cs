﻿#nullable enable
using System.Threading.Tasks;
using Auction.Application.Authorization;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public static partial class Requirement {

		/// <inheritdoc />
		public abstract partial class IsAdminOrOwnerOf : Category {

			private const string policy = nameof(IsAdminOrOwnerOf);


			/// <summary>
			///     Succeeds if user role is Admin, does not fail
			/// </summary>
			/// <typeparam name="TRequirement"></typeparam>
			public abstract class IsAdminOrOwnerOfHandler<TRequirement> : AuthorizationHandler<TRequirement>
				where TRequirement : IAuthorizationRequirement {
				protected readonly TokenValidationHandler TokenValidationHandler;

				protected IsAdminOrOwnerOfHandler(TokenValidationHandler tokenValidationHandler)
					=> this.TokenValidationHandler = tokenValidationHandler;

				protected string? RouteIdString => TokenValidationHandler.RouteData.Values["id"] as string;

				protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
					TRequirement requirement) {

					if(context.User.IsInRole(Role.Admin)) {
						context.Succeed(requirement);
						return Task.CompletedTask;
					}

					return Task.CompletedTask;
				}
			}
		}
	}

}