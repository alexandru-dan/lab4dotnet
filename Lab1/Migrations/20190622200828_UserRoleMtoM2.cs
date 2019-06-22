using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab1.Migrations
{
    public partial class UserRoleMtoM2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Users_UserId",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Movies",
                newName: "OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_UserId",
                table: "Movies",
                newName: "IX_Movies_OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Users_OwnerId",
                table: "Movies",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Users_OwnerId",
                table: "Movies");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Movies",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Movies_OwnerId",
                table: "Movies",
                newName: "IX_Movies_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Users_UserId",
                table: "Movies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
