﻿using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace AuctionAPI.Web.Swagger {

	public static class SwaggerExtensions {
		public static IServiceCollection AddSwagger(this IServiceCollection services) {
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo {Title = "AuctionAPI", Version = "v1"});
				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
					Description = @"JWT Authorization header using the Bearer scheme.
						Enter 'Bearer [token]' in the text input below",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.ApiKey,
					Scheme = "Bearer"
				});
				c.AddSecurityRequirement(new OpenApiSecurityRequirement {
					{
						new OpenApiSecurityScheme {
							Reference = new OpenApiReference {
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							},
							Scheme = "oauth2",
							Name = "Bearer",
							In = ParameterLocation.Header
						},
						new List<string>()
					}
				});
				c.SchemaFilter<ExcludeIdFieldFromInputModelSwaggerFilter>();
			});
			return services;
		}
	}

}