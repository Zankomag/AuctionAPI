using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Auction.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Policy requirement
	/// </summary>
	public static partial class Requirement {

		//Inheritdoc does not inherit from interface
		/// <summary>
		///     Requirement category
		/// </summary>
		public abstract class Category { }


		/// <summary>
		///     Validates that the token contains 'sub' and that corresponding to it user exists
		/// </summary>
		public class TokenValidationHandler : IAuthorizationHandler {

			private readonly IHttpContextAccessor httpContextAccessor;
			private readonly IUserService userService;
			private RouteData routeData;

			/// <summary>
			///     'sub' field of JWT
			/// </summary>
			public string UserIdString { get; private set; }

			/// <summary>
			///     'sub' field of JWT
			/// </summary>
			public int UserId { get; private set; }

			//TODO add IRouteRequestService that contains and sets this property
			public RouteData RouteData
				=> routeData ??= httpContextAccessor.HttpContext!.GetRouteData();

			public TokenValidationHandler(IHttpContextAccessor httpContextAccessor, IUserService userService) {
				this.httpContextAccessor = httpContextAccessor;
				this.userService = userService;
			}

			/// <inheritdoc />
			public async Task HandleAsync(AuthorizationHandlerContext context) {
				UserIdString = context.User.FindFirstValue(JwtOpenIdProperty.Sub);
				if(UserIdString == null
					|| !Int32.TryParse(UserIdString, out int userId)
					//Instead of validating user, you had better save every token Id in db and then check if it exists
					|| !await userService.UserExists(userId)) {
					
					context.Fail();
					return;
				}
				UserId = userId;
			}
		}
	}

}