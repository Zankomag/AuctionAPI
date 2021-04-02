using Microsoft.EntityFrameworkCore.Migrations;

namespace Auction.Infrastructure.Migrations
{
    public partial class AddAuctionItemName_and_MakeCategoryNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionItems_AuctionItemCategories_AuctionItemCategoryId",
                table: "AuctionItems");

            migrationBuilder.AlterColumn<int>(
                name: "AuctionItemCategoryId",
                table: "AuctionItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AuctionItems",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionItems_AuctionItemCategories_AuctionItemCategoryId",
                table: "AuctionItems",
                column: "AuctionItemCategoryId",
                principalTable: "AuctionItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionItems_AuctionItemCategories_AuctionItemCategoryId",
                table: "AuctionItems");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AuctionItems");

            migrationBuilder.AlterColumn<int>(
                name: "AuctionItemCategoryId",
                table: "AuctionItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionItems_AuctionItemCategories_AuctionItemCategoryId",
                table: "AuctionItems",
                column: "AuctionItemCategoryId",
                principalTable: "AuctionItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
