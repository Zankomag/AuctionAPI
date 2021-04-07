using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Infrastructure.Migrations
{
    public partial class DeleteLotStatusAndChangeDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionItems_AuctionItemStatusCodes_AuctionItemStatusCodeId",
                table: "AuctionItems");

            migrationBuilder.DropTable(
                name: "AuctionItemStatusCodes");

            migrationBuilder.DropIndex(
                name: "IX_AuctionItems_AuctionItemStatusCodeId",
                table: "AuctionItems");

            migrationBuilder.DropColumn(
                name: "AuctionItemStatusCodeId",
                table: "AuctionItems");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ActualCloseDate",
                table: "AuctionItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ActualCloseDate",
                table: "AuctionItems",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "AuctionItemStatusCodeId",
                table: "AuctionItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_AuctionItems_AuctionItemStatusCodeId",
                table: "AuctionItems",
                column: "AuctionItemStatusCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionItems_AuctionItemStatusCodes_AuctionItemStatusCodeId",
                table: "AuctionItems",
                column: "AuctionItemStatusCodeId",
                principalTable: "AuctionItemStatusCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
