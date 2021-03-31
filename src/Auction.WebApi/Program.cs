using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Auction.WebApi {

	public static class Program {
		public static void Main(string[] args) {
			var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

			string configFileName = environment == Environments.Development
				? "appsettings.development.json"
				: "appsettings.json";

			var config = new ConfigurationBuilder()
				.AddJsonFile(configFileName)
				.Build();

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(config)
				//This filter cannot be applied in config due to syntax limitations
				.Filter.ByExcluding(x => x.Exception is Microsoft.EntityFrameworkCore.DbUpdateException)
				.CreateLogger();

			Log.Information("Application starting");
			try {
				CreateHostBuilder(args).Build().Run();
			} catch(Exception ex) {
				Log.Fatal(ex, "Application failed to start");
			} finally {
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog()
				.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
	}

}