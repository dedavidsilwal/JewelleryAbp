using Microsoft.EntityFrameworkCore.Migrations;

namespace Jewellery.Migrations
{
    public partial class SeqStartBy1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RestartSequence(
                name: "OrderNumbers",
                schema: "shared",
                startValue: 1L);

            migrationBuilder.RestartSequence(
                name: "InvoiceNumbers",
                schema: "shared",
                startValue: 1L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RestartSequence(
                name: "OrderNumbers",
                schema: "shared",
                startValue: 0L);

            migrationBuilder.RestartSequence(
                name: "InvoiceNumbers",
                schema: "shared",
                startValue: 0L);
        }
    }
}
