using Microsoft.EntityFrameworkCore.Migrations;

namespace Jewellery.Migrations
{
    public partial class updatedPhoneNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNmber",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Customers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNmber",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
