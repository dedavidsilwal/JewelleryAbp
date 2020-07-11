using Microsoft.EntityFrameworkCore.Migrations;

namespace Jewellery.Migrations
{
    public partial class todayMetalCost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MetalCostThisDay",
                table: "OrderDetails");

            migrationBuilder.AddColumn<decimal>(
                name: "TodayMetalCost",
                table: "OrderDetails",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TodayMetalCost",
                table: "OrderDetails");

            migrationBuilder.AddColumn<decimal>(
                name: "MetalCostThisDay",
                table: "OrderDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
