using Microsoft.EntityFrameworkCore.Migrations;

namespace Jewellery.Migrations
{
    public partial class saleUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueAmount",
                table: "Sales");

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "Sales",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Sales");

            migrationBuilder.AddColumn<decimal>(
                name: "DueAmount",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
