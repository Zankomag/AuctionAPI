using Microsoft.EntityFrameworkCore.Migrations;

namespace AuctionAPI.Infrastructure.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Bidders_BidderId",
                table: "Bids");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Bidders_BidderId",
                table: "Bids",
                column: "BidderId",
                principalTable: "Bidders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Bidders_BidderId",
                table: "Bids");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Bidders_BidderId",
                table: "Bids",
                column: "BidderId",
                principalTable: "Bidders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
