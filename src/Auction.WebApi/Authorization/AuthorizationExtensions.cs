using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
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
				.AddJwtBearer(options =>
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
					});

			services.AddAuthorization(x => {
				x.AddPolicy(Requirement.IsAdminOrOwnerOf.User.Policy,
					policy => policy.Requirements.Add(new Requirement.IsAdminOrOwnerOf.User()));
				x.AddPolicy(Requirement.IsAdminOrOwnerOf.AuctionItem.Policy, 
					policy => policy.Requirements.Add(new Requirement.IsAdminOrOwnerOf.AuctionItem()));
			});

			//These services are scoped because they use cached RouteData and JWT values at each request
			services.AddScoped<IAuthorizationHandler, Requirement.IsAdminOrOwnerOf.User.Handler>();
			services.AddScoped<IAuthorizationHandler, Requirement.IsAdminOrOwnerOf.AuctionItem.Handler>();
			services.AddHttpContextAccessor();

			return services;
		}
	}

}