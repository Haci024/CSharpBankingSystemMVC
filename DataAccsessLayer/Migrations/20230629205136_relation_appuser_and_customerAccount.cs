using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccsessLayer.Migrations
{
    public partial class relation_appuser_and_customerAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppUserId",
                table: "CustomerAccount",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccount_AppUserId",
                table: "CustomerAccount",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAccount_AspNetUsers_AppUserId",
                table: "CustomerAccount",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAccount_AspNetUsers_AppUserId",
                table: "CustomerAccount");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAccount_AppUserId",
                table: "CustomerAccount");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "CustomerAccount");
        }
    }
}
