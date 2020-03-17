using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerApp.Data.Migrations
{
    public partial class AddingComputerOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Computer_Order_OrderId",
                table: "Computer");

            migrationBuilder.DropIndex(
                name: "IX_Computer_OrderId",
                table: "Computer");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Computer");

            migrationBuilder.CreateTable(
                name: "ComputerOrder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(nullable: false),
                    ComputerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComputerOrder_Computer_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerOrder_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComputerOrder_ComputerId",
                table: "ComputerOrder",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerOrder_OrderId",
                table: "ComputerOrder",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComputerOrder");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Computer",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Computer_OrderId",
                table: "Computer",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Computer_Order_OrderId",
                table: "Computer",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
