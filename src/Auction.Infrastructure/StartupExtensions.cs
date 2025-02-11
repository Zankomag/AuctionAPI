﻿using System;
using Auction.Application.Mapping;
using Auction.Application.Services;
using Auction.Application.Services.Abstractions;
using Auction.Core.Repositories;
using Auction.Infrastructure.Data;
using Auction.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auction.Infrastructure {

	public static class StartupExtensions {
		public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config,
			Profile webApiMappingProfile) {
			string connection = config.GetConnectionString("AuctionDb");
			services.AddDbContext<AuctionDbContext>(options => options.UseSqlServer(connection));

			services.BuildServiceProvider().ApplyMigration<AuctionDbContext>();

			services.AddScoped<IUnitOfWork, UnitOfWork>();

			services.AddScoped<IAuctionItemCategoryService, AuctionItemCategoryService>();
			services.AddScoped<IAuctionItemService, AuctionItemService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IBidService, BidService>();

			MapperConfiguration mapperConfig = new MapperConfiguration(x => {
				x.AddProfile<EntityToModelProfile>();
				x.AddProfile(webApiMappingProfile);
			});

			services.AddSingleton(mapperConfig.CreateMapper());

			return services;
		}

		private static void ApplyMigration<TDbContext>(this IServiceProvider serviceProvider) where TDbContext : DbContext {
			var dbContext = serviceProvider.GetService<TDbContext>();
			dbContext?.Database.Migrate();
		}

	}

}