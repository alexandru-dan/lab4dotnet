using Microsoft.EntityFrameworkCore.Migrations;

namespace Lab1.Migrations
{
    public partial class AddedDurationInMinutesRemovedUnnecesaryFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StoryLine",
                table: "Movies");

            migrationBuilder.AddColumn<int>(
                name: "DurationInMinutes",
                table: "Movies",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationInMinutes",
                table: "Movies");

            migrationBuilder.AddColumn<string>(
                name: "StoryLine",
                table: "Movies",
                nullable: true);
        }
    }
}
