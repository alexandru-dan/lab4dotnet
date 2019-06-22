using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab1.Migrations
{
    public partial class UserRoleMtoM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserRole",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataRegistered",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Movies",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserToRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    UserRoleId = table.Column<int>(nullable: false),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserToRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserToRoles_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_UserId",
                table: "Movies",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToRoles_UserId",
                table: "UserToRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserToRoles_UserRoleId",
                table: "UserToRoles",
                column: "UserRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Users_UserId",
                table: "Movies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Users_UserId",
                table: "Movies");

            migrationBuilder.DropTable(
                name: "UserToRoles");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_Movies_UserId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "DataRegistered",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Movies");

            migrationBuilder.AddColumn<int>(
                name: "UserRole",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }
    }
}
