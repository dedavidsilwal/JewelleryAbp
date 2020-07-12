using Microsoft.EntityFrameworkCore.Migrations;

namespace Jewellery.Migrations
{
    public partial class todayMetalCostSale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TodayMetalPrice",
                table: "SaleDetails");

            migrationBuilder.AddColumn<decimal>(
                name: "TodayMetalCost",
                table: "SaleDetails",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TodayMetalCost",
                table: "SaleDetails");

            migrationBuilder.AddColumn<decimal>(
                name: "TodayMetalPrice",
                table: "SaleDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
