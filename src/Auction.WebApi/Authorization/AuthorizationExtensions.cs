﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Requirements;
using Auction.WebApi.Authorization.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Auction.WebApi.Authorization {

	public static class AuthorizationExtensions {

		public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services,
			IConfiguration configuration) {

			services.AddAuthentication(configuration);
			services.AddAuthorization();
			return services;
		}

		private static void AddAuthentication(this IServiceCollection services,
			IConfiguration configuration) {

			services.AddScoped<IAuthenticationService, AuthenticationService>();
			var jwtSettingsConfigSection = configuration.GetSection(nameof(JwtSettings));
			services.Configure<JwtSettings>(jwtSettingsConfigSection);


			//This disables mapping normal claim names to urls microsoft uses
			JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
			services.AddAuthentication(options => {
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options => {
					options.TokenValidationParameters = new TokenValidationParameters {
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(
							Encoding.UTF8.GetBytes(
								configuration[$"{nameof(JwtSettings)}:{nameof(JwtSettings.Secret)}"])),
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateLifetime = true,
						ClockSkew = TimeSpan.FromMinutes(5),
						NameClaimType = JwtOpenIdProperty.Username,
						RoleClaimType = JwtOpenIdProperty.Role
					};
					options.Events = new JwtBearerEvents {
						OnTokenValidated = context => {
							//Check if token contains 'sub' and that sub is integer, otherwise authorization fails
							var userIdString = context.Principal.FindFirstValue(JwtOpenIdProperty.Sub);
							if(userIdString == null)
								context.Fail(JwtMessage.SubPropertyDoesntExist);
							if(!Int32.TryParse(userIdString, out _)) {
								context.Fail(JwtMessage.SubPropertyIsNotInteger);
							}
							return Task.CompletedTask;
						}

					};
				});
		}

		public static void AddAuthorization(this IServiceCollection services) {
			services.AddAuthorization(x => {
				x.AddPolicy(Requirement.IsAdminOrOwnerOf.User.Policy,
					policy => policy.AddRequirements(new Requirement.IsAdminOrOwnerOf.User()));
				x.AddPolicy(Requirement.IsAdminOrOwnerOf.AuctionItem.Policy,
					policy => policy.AddRequirements(new Requirement.IsAdminOrOwnerOf.AuctionItem()));
			});

			//These services are scoped because they use cached RouteData and JWT values at each request
			services.AddScoped<Requirement.TokenValidationHandler>();
			services.AddScoped<IAuthorizationHandler>(x => x.GetRequiredService<Requirement.TokenValidationHandler>());
			services.AddScoped<IAuthorizationHandler, Requirement.IsAdminOrOwnerOf.User.Handler>();
			services.AddScoped<IAuthorizationHandler, Requirement.IsAdminOrOwnerOf.AuctionItem.Handler>();

			services.AddHttpContextAccessor();
		}
	}

}