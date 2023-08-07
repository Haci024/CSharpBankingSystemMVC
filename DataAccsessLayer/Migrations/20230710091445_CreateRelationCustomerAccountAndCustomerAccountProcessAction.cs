using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccsessLayer.Migrations
{
    public partial class CreateRelationCustomerAccountAndCustomerAccountProcessAction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiverID",
                table: "CustomerActionProcess",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SenderID",
                table: "CustomerActionProcess",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerActionProcess_ReceiverID",
                table: "CustomerActionProcess",
                column: "ReceiverID");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerActionProcess_SenderID",
                table: "CustomerActionProcess",
                column: "SenderID");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerActionProcess_CustomerAccount_ReceiverID",
                table: "CustomerActionProcess",
                column: "ReceiverID",
                principalTable: "CustomerAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerActionProcess_CustomerAccount_SenderID",
                table: "CustomerActionProcess",
                column: "SenderID",
                principalTable: "CustomerAccount",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerActionProcess_CustomerAccount_ReceiverID",
                table: "CustomerActionProcess");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerActionProcess_CustomerAccount_SenderID",
                table: "CustomerActionProcess");

            migrationBuilder.DropIndex(
                name: "IX_CustomerActionProcess_ReceiverID",
                table: "CustomerActionProcess");

            migrationBuilder.DropIndex(
                name: "IX_CustomerActionProcess_SenderID",
                table: "CustomerActionProcess");

            migrationBuilder.DropColumn(
                name: "ReceiverID",
                table: "CustomerActionProcess");

            migrationBuilder.DropColumn(
                name: "SenderID",
                table: "CustomerActionProcess");
        }
    }
}
