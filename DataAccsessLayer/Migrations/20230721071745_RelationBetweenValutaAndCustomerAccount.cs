using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccsessLayer.Migrations
{
    public partial class RelationBetweenValutaAndCustomerAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ValutaID",
                table: "CustomerAccount",
                type: "int",
                nullable: true,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAccount_ValutaID",
                table: "CustomerAccount",
                column: "ValutaID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerAccount_Valuta_ValutaID",
                table: "CustomerAccount",
                column: "ValutaID",
                principalTable: "Valuta",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerAccount_Valuta_ValutaID",
                table: "CustomerAccount");

            migrationBuilder.DropIndex(
                name: "IX_CustomerAccount_ValutaID",
                table: "CustomerAccount");

            migrationBuilder.DropColumn(
                name: "ValutaID",
                table: "CustomerAccount");
        }
    }
}
