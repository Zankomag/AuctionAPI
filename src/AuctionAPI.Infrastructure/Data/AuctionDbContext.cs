using System;
using System.Linq;
using AuctionAPI.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Infrastructure.Data {

	public class AuctionDbContext : DbContext {

		public DbSet<AuctionItem> AuctionItems { get; set; }
		public DbSet<AuctionItemCategory> AuctionItemCategories { get; set; }
		public DbSet<AuctionItemStatusCode> AuctionItemStatusCodes { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Bidder> Bidders { get; set; }
		public DbSet<Seller> Sellers { get; set; }
		public DbSet<Bid> Bids { get; set; }
		
		public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }


		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<AuctionItem>()
				.Property(x => x.AuctionItemStatusCodeId)
				.HasConversion<int>();

			modelBuilder.Entity<AuctionItemStatusCode>()
				.Property(x => x.Id)
				.HasConversion<int>();

			modelBuilder.Entity<AuctionItemStatusCode>()
				.HasData(Enum.GetValues(typeof(AuctionItemStatusCodeId))
					.Cast<AuctionItemStatusCodeId>()
					.Select(x => new AuctionItemStatusCode {
						Id = x,
						Name = x.ToString()
					}));
		}
	}

}