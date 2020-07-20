using Microsoft.EntityFrameworkCore.Migrations;

namespace Jewellery.Migrations
{
    public partial class addSequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "InvoiceNumbers");

            migrationBuilder.CreateSequence<int>(
                name: "OrderNumbers");

            migrationBuilder.CreateSequence<int>(
                name: "SaleNumbers");

            migrationBuilder.AddColumn<int>(
                name: "SaleNumber",
                table: "Sales",
                nullable: false,
                defaultValueSql: "nextval('\"SaleNumbers\"')");

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "Orders",
                nullable: false,
                defaultValueSql: "nextval('\"OrderNumbers\"')");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceNumber",
                table: "Invoices",
                nullable: false,
                defaultValueSql: "nextval('\"InvoiceNumbers\"')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "InvoiceNumbers");

            migrationBuilder.DropSequence(
                name: "OrderNumbers");

            migrationBuilder.DropSequence(
                name: "SaleNumbers");

            migrationBuilder.DropColumn(
                name: "SaleNumber",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Invoices");
        }
    }
}
