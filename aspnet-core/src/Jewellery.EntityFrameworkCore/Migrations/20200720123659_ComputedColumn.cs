using Microsoft.EntityFrameworkCore.Migrations;

namespace Jewellery.Migrations
{
    public partial class ComputedColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "SaleDetails",
                nullable: false,
                computedColumnSql: "(((\"Wastage\"+\"Weight\") * \"TodayMetalCost\") + \"MakingCharge\")*\"Quantity\"");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalWeight",
                table: "SaleDetails",
                nullable: false,
                computedColumnSql: "\"Wastage\"+\"Weight\"");

            migrationBuilder.AddColumn<decimal>(
                name: "SubTotal",
                table: "OrderDetails",
                nullable: false,
                computedColumnSql: "(((\"Wastage\"+\"Weight\") * \"TodayMetalCost\") + \"MakingCharge\")*\"Quantity\"");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalWeight",
                table: "OrderDetails",
                nullable: false,
                computedColumnSql: "\"Wastage\"+\"Weight\"");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "Customers",
                nullable: true,
                computedColumnSql: "\"FirstName\" || ' ' || \"LastName\"");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "SaleDetails");

            migrationBuilder.DropColumn(
                name: "TotalWeight",
                table: "SaleDetails");

            migrationBuilder.DropColumn(
                name: "SubTotal",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "TotalWeight",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Customers",
                type: "text",
                nullable: true);
        }
    }
}
