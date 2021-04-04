using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Constants;
using Auction.WebApi.Authorization.Requirements;
using Auction.WebApi.Authorization.Services;
using Auction.WebApi.Authorization.Types;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Auction.WebApi.Authorization.Extensions {

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
			services.AddAuthorization(options => {
				//Override default 'DenyAnonymousAuthorizationRequirement' with similar policy
				//that fails context if user is not authenticated
				options.DefaultPolicy = new AuthorizationPolicyBuilder()
					.AddRequirements(new Requirement.IsAuthenticated())
					.Build();
				options.AddPolicy(Requirement.IsAdmin.UserId.Policy,
					policy => policy.AddRequirements(Requirement.IsAdmin.UserId.Get));
				options.AddPolicy(Requirement.IsAdmin.AuctionItemId.Policy,
					policy => policy.AddRequirements(Requirement.IsAdmin.AuctionItemId.Get));
			});

			services.AddScoped<IRequestData, RequestData>();
			
			//These services are scoped because they use scoped IRequestData, otherwise they'd be singletons
			services.AddScoped<IAuthorizationHandler, Requirement.IsAuthenticated.Handler>();
			services.AddScoped<IAuthorizationHandler, Requirement.IsAdmin.Handler>();
			services.AddScoped<IAuthorizationHandler, Requirement.IsAdmin.UserId.Handler>();
			services.AddScoped<IAuthorizationHandler, Requirement.IsAdmin.AuctionItemId.Handler>();

			services.AddHttpContextAccessor();
		}

		public static IApplicationBuilder UseAuthenticationAndAuthorization(this IApplicationBuilder app) {

			app.UseAuthentication();
			app.UseMiddleware<AuthenticationMiddleware>();
			app.UseAuthorization();
			return app;
		}
	}

}