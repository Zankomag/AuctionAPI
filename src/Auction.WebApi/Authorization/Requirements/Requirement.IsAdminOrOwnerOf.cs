using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Auction.Application.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	public static partial class Requirement {

		/// <inheritdoc />
		public abstract partial class IsAdminOrOwnerOf : Category {

			private const string policy = nameof(IsAdminOrOwnerOf);


			/// <inheritdoc />
			public abstract class IsAdminHandler<TRequirement> : HandlerBase<TRequirement>
				where TRequirement : IAuthorizationRequirement {

				/// <summary>
				///     'sub' field of JWT
				/// </summary>
				protected string UserIdString { get; private set; }

				protected int UserId { get; private set; }

				protected IsAdminHandler(IHttpContextAccessor httpContextAccessor) :
					base(httpContextAccessor) { }

				private void SetUserId(AuthorizationHandlerContext context) {
					UserIdString = context.User.FindFirstValue(JwtOpenIdProperty.Sub);
					if(UserIdString == null) {
						context.Fail();
						throw new ArgumentException("JWT doesn't have 'sub' field", nameof(context));
					}
					if(Int32.TryParse(UserIdString, out int userId)) {
						UserId = userId;
					} else {
						context.Fail();
						throw new ArgumentException("'sub' JWT field is not an integer", nameof(context));
					}
				}

				/// <inheritdoc />
				/// <remarks>
				/// Checks for Admin role and Initializes UserId
				/// </remarks>
				protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
					TRequirement requirement) {

					if(context.User.IsInRole(Role.Admin)) {
						context.Succeed(requirement);
						return Task.CompletedTask;
					}

					SetUserId(context);
					return Task.CompletedTask;
				}
			}
		}
	}

}