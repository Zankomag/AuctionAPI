using System;
using System.Linq;
using Auction.Application.Authorization;
using Auction.Core.Entities;
using Microsoft.EntityFrameworkCore;

// ReSharper disable MemberCanBeMadeStatic.Local
// ReSharper disable ArrangeMethodOrOperatorBody

namespace Auction.Infrastructure.Data {

	public sealed class AuctionDbContext : DbContext {

		public DbSet<AuctionItem> AuctionItems { get; set; }
		public DbSet<AuctionItemCategory> AuctionItemCategories { get; set; }
		public DbSet<AuctionItemImage> AuctionItemImages { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserRole> Roles { get; set; }
		public DbSet<Bid> Bids { get; set; }
		public DbSet<UserUserRole> UserUserRoles { get; set; }

		public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }


		/// <inheritdoc />
		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			ConfigureUser(modelBuilder);
			ConfigureAuctionItemCategory(modelBuilder);
			ConfigureAuctionItem(modelBuilder);
			ConfigureBids(modelBuilder);
			ConfigureRoles(modelBuilder);
			ConfigureUserUserRole(modelBuilder);
		}

		private void ConfigureRoles(ModelBuilder modelBuilder) {
			modelBuilder.Entity<UserRole>()
				.HasIndex(x => x.Name).IsUnique();
			
			modelBuilder.Entity<UserRole>()
				.HasData(Role.AllRoles);
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

		private void ConfigureUserUserRole(ModelBuilder modelBuilder) {
			modelBuilder.Entity<UserUserRole>()
				.HasKey(x => new {x.UserId, x.UserRoleId});
			
			modelBuilder.Entity<UserUserRole>()
				.HasOne(x => x.User)
				.WithMany(x => x.UserUserRoles)
				.HasForeignKey(x => x.UserId);

			modelBuilder.Entity<UserUserRole>()
				.HasOne(x => x.UserRole)
				.WithMany(x => x.UserUserRoles)
				.HasForeignKey(x => x.UserRoleId);
		}
	}

}