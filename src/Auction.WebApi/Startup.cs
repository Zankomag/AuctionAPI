using System;
using System.Linq;
using Auction.Infrastructure;
using Auction.WebApi.Authorization.Extensions;
using Auction.WebApi.Authorization.Mapping;
using Auction.WebApi.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;

namespace Auction.WebApi {

	public class Startup {

		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration) => Configuration = configuration;

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services) {
			//Configure all infrastructure services
			services.AddInfrastructure(Configuration, new ApplicationModelToAuthenticationModelProfile());

			services.AddAuthenticationAndAuthorization(Configuration);

			services.AddSwagger();

			services.AddControllers()
				.AddNewtonsoftJson(options => {
					options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
					options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
					options.SerializerSettings.Converters.Add(new StringEnumConverter());
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

			app.UseAuthenticationAndAuthorization();

			app.UseEndpoints(endpoints => endpoints.MapControllers());
		}
	}

}