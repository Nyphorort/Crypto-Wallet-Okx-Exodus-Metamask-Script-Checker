using Microsoft.EntityFrameworkCore.Migrations;

namespace SpotifyCache.Migrations
{
    public partial class addedyear : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Year",
                table: "Tracks",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Tracks");
        }
    }
}
