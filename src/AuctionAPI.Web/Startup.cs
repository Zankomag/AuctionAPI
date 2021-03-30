using System;
using System.Linq;
using System.Text;
using AuctionAPI.Infrastructure;
using AuctionAPI.Web.Authentication;
using AuctionAPI.Web.Authentication.Abstractions;
using AuctionAPI.Web.Mapping;
using AuctionAPI.Web.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;

namespace AuctionAPI.Web {

	public class Startup {

		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration) => Configuration = configuration;

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			//Configure all infrastructure services
			services.AddInfrastructure(Configuration, new ApplicationModelToWebApiModelProfile());

			services.AddScoped<IAuthenticationService, AuthenticationService>();
			var jwtSettingsConfigSection = Configuration.GetSection(nameof(JwtSettings));
			services.Configure<JwtSettings>(jwtSettingsConfigSection);

			services.AddAuthentication(options => {
					options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
					options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				})
				.AddJwtBearer(options =>
					options.TokenValidationParameters = new TokenValidationParameters {
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
							Configuration[$"{nameof(JwtSettings)}:{nameof(JwtSettings.Secret)}"])),
						ValidateIssuer = false,
						ValidateAudience = false,
						ValidateLifetime = true,
						ClockSkew = TimeSpan.FromMinutes(5)
					});

			services.AddSwagger();

			services.AddControllers()
				.AddNewtonsoftJson(options => {
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
				})
				.ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = context => {
					if(context.ModelState.ErrorCount > 0) {
						string messages = String.Join("; ", context.ModelState.Values
							.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
						return new BadRequestObjectResult(messages);
					}
					return new StatusCodeResult(500);
				});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if(env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AuctionAPI v1"));
			}

			app.UseHttpsRedirection();
			app.UseHsts();

			//To log only Warning or greater requests
			//Set "Serilog.AspNetCore": "Warning" in Serilog MinimumLevel Config
			app.UseSerilogRequestLogging();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => endpoints.MapControllers());
		}
	}

}