using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerApp.Data.Migrations
{
    public partial class CorrectingOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComputerComponent_Order_OrderId",
                table: "ComputerComponent");

            migrationBuilder.DropIndex(
                name: "IX_ComputerComponent_OrderId",
                table: "ComputerComponent");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "ComputerComponent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "ComputerComponent",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComputerComponent_OrderId",
                table: "ComputerComponent",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComputerComponent_Order_OrderId",
                table: "ComputerComponent",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
