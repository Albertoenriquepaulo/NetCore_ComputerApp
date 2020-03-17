using Microsoft.EntityFrameworkCore.Migrations;

namespace ComputerApp.Data.Migrations
{
    public partial class CorrectingComponentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Computer_Component_ComponentId",
                table: "Computer");

            migrationBuilder.DropIndex(
                name: "IX_Computer_ComponentId",
                table: "Computer");

            migrationBuilder.DropColumn(
                name: "ComponentId",
                table: "Computer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComponentId",
                table: "Computer",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Computer_ComponentId",
                table: "Computer",
                column: "ComponentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Computer_Component_ComponentId",
                table: "Computer",
                column: "ComponentId",
                principalTable: "Component",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
