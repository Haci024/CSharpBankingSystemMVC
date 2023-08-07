using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccsessLayer.Migrations
{
    public partial class _0s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivateAttempts",
                table: "CustomerAccount");

            migrationBuilder.DropColumn(
                name: "ActivateCode",
                table: "CustomerAccount");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "CustomerAccount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivateAttempts",
                table: "CustomerAccount",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActivateCode",
                table: "CustomerAccount",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "CustomerAccount",
                type: "bit",
                nullable: true);
        }
    }
}
