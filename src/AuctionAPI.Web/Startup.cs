using System;
using System.Collections.Generic;
using System.Linq;
using AuctionAPI.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;

namespace AuctionAPI.Web {
    public class Startup {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            string connection = Configuration.GetConnectionString("AuctionConnectionString");
            services.AddDbContext<AuctionDbContext>(options => options.UseSqlServer(connection));

            
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuctionAPI", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme() {
                    Description = @"JWT Authorization header using the Bearer scheme.
						Enter 'Bearer [token]' in the text input below",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme() {
                            Reference = new OpenApiReference() {
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
            });

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
            if (env.IsDevelopment()) {
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
