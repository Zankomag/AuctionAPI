using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Requirements;
using Auction.WebApi.Authorization.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Auction.WebApi.Authorization {

	public static class StartupExtensions {

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
				.AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters {
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(
						Encoding.UTF8.GetBytes(configuration[$"{nameof(JwtSettings)}:{nameof(JwtSettings.Secret)}"])),
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.FromMinutes(5),
					NameClaimType = JwtOpenIdProperty.Username,
					RoleClaimType = JwtOpenIdProperty.Role
				});
		}

		private static void AddAuthorization(this IServiceCollection services) {
			services.AddAuthorization(x => {
				x.AddPolicy(Requirement.IsAdminOrOwnerOf.UserId.Policy,
					policy => policy.AddRequirements(Requirement.IsAdminOrOwnerOf.UserId.Get));
				x.AddPolicy(Requirement.IsAdminOrOwnerOf.AuctionItemId.Policy,
					policy => policy.AddRequirements(Requirement.IsAdminOrOwnerOf.AuctionItemId.Get));
			});

			//These services are scoped because they use cached RouteData and JWT values at each request
			services.AddScoped<Requirement.AuthorizationService>();
			services.AddScoped<IAuthorizationHandler>(x => x.GetRequiredService<Requirement.AuthorizationService>());
			services.AddScoped<IAuthorizationHandler, Requirement.IsAdminOrOwnerOf.UserId.Handler>();
			services.AddScoped<IAuthorizationHandler, Requirement.IsAdminOrOwnerOf.AuctionItemId.Handler>();

			services.AddHttpContextAccessor();
		}

		public static IApplicationBuilder UseAuthenticationAndAuthorization(this IApplicationBuilder app) {

			app.UseAuthentication();
			app.UseAuthorization();
			return app;
		}
	}

}