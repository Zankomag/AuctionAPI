using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionAPI.Infrastructure.Migrations
{
    public partial class AddChildCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentCategoryId",
                table: "AuctionItemCategories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuctionItemCategories_ParentCategoryId",
                table: "AuctionItemCategories",
                column: "ParentCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionItemCategories_AuctionItemCategories_ParentCategoryId",
                table: "AuctionItemCategories",
                column: "ParentCategoryId",
                principalTable: "AuctionItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuctionItemCategories_AuctionItemCategories_ParentCategoryId",
                table: "AuctionItemCategories");

            migrationBuilder.DropIndex(
                name: "IX_AuctionItemCategories_ParentCategoryId",
                table: "AuctionItemCategories");

            migrationBuilder.DropColumn(
                name: "ParentCategoryId",
                table: "AuctionItemCategories");
        }
    }
}
