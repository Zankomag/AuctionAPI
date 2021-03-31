using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuctionItemCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionItemCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuctionItemStatusCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionItemStatusCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bidders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bidders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bidders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sellers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuctionItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    AuctionItemStatusCodeId = table.Column<int>(type: "int", nullable: false),
                    AuctionItemCategoryId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PlannedCloseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualCloseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WinningBidId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctionItems_AuctionItemCategories_AuctionItemCategoryId",
                        column: x => x.AuctionItemCategoryId,
                        principalTable: "AuctionItemCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctionItems_AuctionItemStatusCodes_AuctionItemStatusCodeId",
                        column: x => x.AuctionItemStatusCodeId,
                        principalTable: "AuctionItemStatusCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuctionItems_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BidderId = table.Column<int>(type: "int", nullable: false),
                    AuctionItemId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bids_AuctionItems_AuctionItemId",
                        column: x => x.AuctionItemId,
                        principalTable: "AuctionItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bids_Bidders_BidderId",
                        column: x => x.BidderId,
                        principalTable: "Bidders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AuctionItemStatusCodes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 0, "Scheduled" });

            migrationBuilder.InsertData(
                table: "AuctionItemStatusCodes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Started" });

            migrationBuilder.InsertData(
                table: "AuctionItemStatusCodes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Finished" });

            migrationBuilder.CreateIndex(
                name: "IX_AuctionItemCategories_Name",
                table: "AuctionItemCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionItems_AuctionItemCategoryId",
                table: "AuctionItems",
                column: "AuctionItemCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionItems_AuctionItemStatusCodeId",
                table: "AuctionItems",
                column: "AuctionItemStatusCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionItems_SellerId",
                table: "AuctionItems",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionItems_WinningBidId",
                table: "AuctionItems",
                column: "WinningBidId",
                unique: true,
                filter: "[WinningBidId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Bidders_UserId",
                table: "Bidders",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bids_AuctionItemId",
                table: "Bids",
                column: "AuctionItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_BidderId",
                table: "Bids",
                column: "BidderId");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_UserId",
                table: "Sellers",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionItems_Bids_WinningBidId",
                table: "AuctionItems",
                column: "WinningBidId",
                principalTable: "Bids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionItems_AuctionItemCategories_AuctionItemCategoryId",
                table: "AuctionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AuctionItems_AuctionItemStatusCodes_AuctionItemStatusCodeId",
                table: "AuctionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AuctionItems_Bids_WinningBidId",
                table: "AuctionItems");

            migrationBuilder.DropTable(
                name: "AuctionItemCategories");

            migrationBuilder.DropTable(
                name: "AuctionItemStatusCodes");

            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "AuctionItems");

            migrationBuilder.DropTable(
                name: "Bidders");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
