using Microsoft.EntityFrameworkCore.Migrations;

namespace Jewellery.Migrations
{
    public partial class SequenceNumberAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "shared");

            migrationBuilder.CreateSequence<int>(
                name: "InvoiceNumbers",
                schema: "shared",
                startValue: 0L);

            migrationBuilder.CreateSequence<int>(
                name: "OrderNumbers",
                schema: "shared",
                startValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "OrderNumber",
                table: "Orders",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR shared.OrderNumbers");

            migrationBuilder.AddColumn<int>(
                name: "InvoiceNumber",
                table: "Invoices",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR shared.InvoiceNumbers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "InvoiceNumbers",
                schema: "shared");

            migrationBuilder.DropSequence(
                name: "OrderNumbers",
                schema: "shared");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "Invoices");
        }
    }
}
