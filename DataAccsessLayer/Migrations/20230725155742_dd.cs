using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccsessLayer.Migrations
{
    public partial class dd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CreditAmount",
                table: "Credits",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Percent",
                table: "Credits",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Period",
                table: "Credits",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalPayment",
                table: "Credits",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "MonthlyPayment",
                table: "CreditDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RemainingPayment",
                table: "CreditDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditAmount",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "Percent",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "Period",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "TotalPayment",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "MonthlyPayment",
                table: "CreditDetails");

            migrationBuilder.DropColumn(
                name: "RemainingPayment",
                table: "CreditDetails");
        }
    }
}
