using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccsessLayer.Migrations
{
    public partial class Reset3ColumnToCustomerAccountTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivateAttempts",
                table: "CustomerAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ActivateCode",
                table: "CustomerAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "CustomerAccount",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
