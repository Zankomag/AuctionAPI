using System;
using System.Linq;
using Auction.Core.Entities;
using Microsoft.EntityFrameworkCore;

// ReSharper disable MemberCanBeMadeStatic.Local
// ReSharper disable ArrangeMethodOrOperatorBody

namespace Auction.Infrastructure.Data {

	public sealed class AuctionDbContext : DbContext {

		public DbSet<AuctionItem> AuctionItems { get; set; }
		public DbSet<AuctionItemCategory> AuctionItemCategories { get; set; }
		public DbSet<AuctionItemStatusCode> AuctionItemStatusCodes { get; set; }
		public DbSet<AuctionItemImage> AuctionItemImages { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Bid> Bids { get; set; }

		public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) 
			=> Database.Migrate();


		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			ConfigureUser(modelBuilder);
			ConfigureAuctionItemCategory(modelBuilder);
			ConfigureAuctionItem(modelBuilder);
			ConfigureBids(modelBuilder);
			ConfigureAuctionItemStatusCode(modelBuilder);
		}

		private void ConfigureAuctionItemStatusCode(ModelBuilder modelBuilder) {
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

		private void ConfigureBids(ModelBuilder modelBuilder) {
			//SQL Server doesn't allow to use Cascade here because of cycles in relationships
			modelBuilder.Entity<Bid>()
				.HasOne(x => x.Bidder)
				.WithMany(x => x.Bids)
				.OnDelete(DeleteBehavior.ClientCascade);
		}

		private void ConfigureAuctionItem(ModelBuilder modelBuilder) {
			//EF core couldn't create this relation somehow, do it's done manually
			modelBuilder.Entity<AuctionItem>()
				.HasOne(x => x.WinningBid)
				.WithOne(x => x.AuctionItem)
				.HasForeignKey<AuctionItem>(x => x.WinningBidId);
		}

		private void ConfigureAuctionItemCategory(ModelBuilder modelBuilder) {
			modelBuilder.Entity<AuctionItemCategory>()
				.HasIndex(x => x.Name).IsUnique();
		}

		private void ConfigureUser(ModelBuilder modelBuilder) {
			modelBuilder.Entity<User>()
				.HasIndex(x => x.Email).IsUnique();
		}
	}

}