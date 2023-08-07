using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccsessLayer.Migrations
{
    public partial class RelationBetweenCreditsandMonthlyPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditID",
                table: "MonthlyPayment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyPayment_CreditID",
                table: "MonthlyPayment",
                column: "CreditID");

            migrationBuilder.AddForeignKey(
                name: "FK_MonthlyPayment_Credits_CreditID",
                table: "MonthlyPayment",
                column: "CreditID",
                principalTable: "Credits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MonthlyPayment_Credits_CreditID",
                table: "MonthlyPayment");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyPayment_CreditID",
                table: "MonthlyPayment");

            migrationBuilder.DropColumn(
                name: "CreditID",
                table: "MonthlyPayment");
        }
    }
}
