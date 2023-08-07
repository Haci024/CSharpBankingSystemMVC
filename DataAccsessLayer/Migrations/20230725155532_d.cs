using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccsessLayer.Migrations
{
    public partial class d : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CreditAmount",
                table: "Credits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<float>(
                name: "Percent",
                table: "Credits",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<decimal>(
                name: "Period",
                table: "Credits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPayment",
                table: "Credits",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlyPayment",
                table: "CreditDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "RemainingPayment",
                table: "CreditDetails",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
