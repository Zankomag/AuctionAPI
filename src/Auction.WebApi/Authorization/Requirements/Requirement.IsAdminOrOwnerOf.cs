#nullable enable
using System.Threading.Tasks;
using Auction.Application.Authorization;
using Auction.WebApi.Authorization.Extensions;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public static partial class Requirement {

		public class IsAdmin : IAuthorizationRequirement { }

		/// <inheritdoc />
		public abstract partial class IsAdminOrOwnerOf : Category {

			private const string policy = nameof(IsAdminOrOwnerOf);

			
			
			/// <summary>
			///     Succeeds if user role is Admin, does not fail
			/// </summary>
			public class IsAdminOrOwnerOfHandler : AuthorizationHandler<IsAdmin> {

				protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
					IsAdmin requirement) {

					if(!context.IsAlreadyDetermined()
						&& context.User.IsInRole(Role.Admin)) {
						
						context.Succeed(requirement);
					}
					return Task.CompletedTask;
				}
			}
		}
	}

}