﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization.Constants;
using Auction.WebApi.Authorization.Types;
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
		public async Task InvokeAsync(HttpContext context, IUserService userService) {
			if(context.User.Identity?.IsAuthenticated == true) {
				string userIdString = context.User.FindFirstValue(JwtOpenIdProperty.Sub);

				if(userIdString == null || !Int32.TryParse(userIdString, out int userId)

					//Instead of validating user, you had better save every token Id in db and then check if it exists
					|| !await userService.UserExistsAsync(userId)) {

					//Authentication failed
					await context.Response.WriteAsync(AuthenticationMessage.InvalidToken);

					//set response to 401
					await context.ChallengeAsync();
					return;
				}

				//Authentication passed, set user identity to allow using it in other services
				context.User.AddIdentity(new UserIdentity {
					Id = userId,
					IdString = userIdString,
					Roles = context.User.FindAll(x => x.Type == JwtOpenIdProperty.Role)
						.Select(x => x.Value)
						.ToList()
				});
			}

			await next(context);
		}
	}

}