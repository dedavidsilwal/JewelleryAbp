using Microsoft.EntityFrameworkCore.Migrations;

namespace Jewellery.Migrations
{
    public partial class saleProductConflict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "SaleNumbers",
                schema: "shared");

            migrationBuilder.AddColumn<int>(
                name: "SaleNumber",
                table: "Sales",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR shared.SaleNumbers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "SaleNumbers",
                schema: "shared");

            migrationBuilder.DropColumn(
                name: "SaleNumber",
                table: "Sales");
        }
    }
}
