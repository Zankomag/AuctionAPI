using System.Threading.Tasks;
using Auction.Application.Authorization;
using Auction.WebApi.Authorization.Extensions;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public static partial class Requirement {


		/// <inheritdoc />
		public partial class IsAdmin : IAuthorizationRequirement {

			public const string Policy = nameof(IsAdmin);
			
			/// <summary>
			///     Succeeds if user role is Admin, does not fail
			/// </summary>
			public class Handler : AuthorizationHandler<IsAdmin> {

				protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
					IsAdmin requirement) {

					if(!context.IsAlreadyDetermined<IsAdmin>()
						&& context.User.IsInRole(Role.Admin)) {

						context.Succeed(requirement);
					}
					return Task.CompletedTask;
				}
			}
		}
	}

}