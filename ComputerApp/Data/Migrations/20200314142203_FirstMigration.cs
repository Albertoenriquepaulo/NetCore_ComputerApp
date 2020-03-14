using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerApp.Data.Migrations
{
    public partial class FirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<double>(nullable: false),
                    Qty = table.Column<int>(maxLength: 1, nullable: false),
                    IsCart = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Component",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Price = table.Column<double>(nullable: false),
                    ComponentTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Component", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Component_CType_ComponentTypeId",
                        column: x => x.ComponentTypeId,
                        principalTable: "CType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Computer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 500, nullable: false),
                    Price = table.Column<double>(nullable: false),
                    IsDesktop = table.Column<bool>(nullable: false),
                    ImgUrl = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    ComponentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Computer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Computer_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Computer_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComputerComponent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComputerId = table.Column<int>(nullable: false),
                    ComponentId = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerComponent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComputerComponent_Component_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Component",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerComponent_Computer_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Computer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComputerComponent_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_OrderId",
                table: "AspNetUsers",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Component_ComponentTypeId",
                table: "Component",
                column: "ComponentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Computer_ComponentId",
                table: "Computer",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Computer_OrderId",
                table: "Computer",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerComponent_ComponentId",
                table: "ComputerComponent",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerComponent_ComputerId",
                table: "ComputerComponent",
                column: "ComputerId");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerComponent_OrderId",
                table: "ComputerComponent",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Order_OrderId",
                table: "AspNetUsers",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Order_OrderId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ComputerComponent");

            migrationBuilder.DropTable(
                name: "Computer");

            migrationBuilder.DropTable(
                name: "Component");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "CType");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_OrderId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "AspNetUsers");
        }
    }
}
