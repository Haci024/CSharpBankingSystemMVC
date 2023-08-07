using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccsessLayer.Migrations
{
    public partial class AddRelationBetweenCustomerAccountAndCreditTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CustomerAccountID",
                table: "Credits",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Credits_CustomerAccountID",
                table: "Credits",
                column: "CustomerAccountID");

            migrationBuilder.AddForeignKey(
                name: "FK_Credits_CustomerAccount_CustomerAccountID",
                table: "Credits",
                column: "CustomerAccountID",
                principalTable: "CustomerAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credits_CustomerAccount_CustomerAccountID",
                table: "Credits");

            migrationBuilder.DropIndex(
                name: "IX_Credits_CustomerAccountID",
                table: "Credits");

            migrationBuilder.DropColumn(
                name: "CustomerAccountID",
                table: "Credits");
        }
    }
}
