using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Auction.WebApi.Authorization.Services {

	public class AuthenticationMiddleware {
		/// <summary>
		///     The delegate representing the remaining middleware in the request pipeline
		/// </summary>
		private readonly RequestDelegate next;

		public AuthenticationMiddleware(RequestDelegate next) => this.next = next;

		//While the class is singleton, this method is called every request (like Scoped service)
		public async Task InvokeAsync(HttpContext context, IUserService userService, IRequestData requestData) {
			if(context.User.Identity?.IsAuthenticated != true) {
				//TODO if allowAnon or don't need auth, when we should response with 200, not 401
				//await context.ChallengeAsync();
			} else { //User considered as authenticated at previous stage

				string userIdString = context.User.FindFirstValue(JwtOpenIdProperty.Sub);

				// if 'sub' is null that means either 'sub' doesn't exists or token is not authorized
				if(userIdString != null
					&& Int32.TryParse(userIdString, out int userId)

					//Instead of validating user, you had better save every token Id in db and then check if it exists
					&& await userService.UserExists(userId)) {

					//Authentication passed, set request data to allow using it in other pipeline services
					requestData.UserId = userId;
					requestData.UserIdString = userIdString;

				} else {
					//TODO if allowAnon or don't need auth, when we should response with 200, not 401
					//Authentication failed, set response to 401
					await context.ChallengeAsync();
					return;
				}

			}

			await next(context);
		}
	}

}