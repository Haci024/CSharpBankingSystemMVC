using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccsessLayer.Migrations
{
    public partial class AddCCVandExploryDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "CustomerAccount",
                newName: "ExploryDate");

            migrationBuilder.AddColumn<string>(
                name: "CCV",
                table: "CustomerAccount",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CCV",
                table: "CustomerAccount");

            migrationBuilder.RenameColumn(
                name: "ExploryDate",
                table: "CustomerAccount",
                newName: "Currency");
        }
    }
}
