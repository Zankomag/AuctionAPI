using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuctionAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuctionAPI.Infrastructure {
	public static class DependencyInjection {
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config) {
			string connection = config.GetConnectionString("AuctionDb");
			services.AddDbContext<AuctionDbContext>(options => options.UseSqlServer(connection));

			return services;
		}
	}
}
