using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpotifyCache.Migrations
{
    public partial class initialmigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artists",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SearchedRelatedArtists = table.Column<bool>(type: "bit", nullable: false),
                    LastSearchedSongs = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PercentileBuckets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Statistic = table.Column<int>(type: "int", nullable: false),
                    Min = table.Column<float>(type: "real", nullable: false),
                    Max = table.Column<float>(type: "real", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PercentileBuckets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Event = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Artists = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArtistIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Danceability = table.Column<float>(type: "real", nullable: false),
                    Energy = table.Column<float>(type: "real", nullable: false),
                    Acousticness = table.Column<float>(type: "real", nullable: false),
                    Instrumentalness = table.Column<float>(type: "real", nullable: false),
                    Liveness = table.Column<float>(type: "real", nullable: false),
                    Valence = table.Column<float>(type: "real", nullable: false),
                    Key = table.Column<int>(type: "int", nullable: false),
                    Loudness = table.Column<float>(type: "real", nullable: false),
                    Mode = table.Column<int>(type: "int", nullable: false),
                    Speechiness = table.Column<float>(type: "real", nullable: false),
                    Tempo = table.Column<float>(type: "real", nullable: false),
                    DurationMs = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreationHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TrackId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreationHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreationHistories_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreationHistories_TrackId",
                table: "CreationHistories",
                column: "TrackId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Artists");

            migrationBuilder.DropTable(
                name: "CreationHistories");

            migrationBuilder.DropTable(
                name: "PercentileBuckets");

            migrationBuilder.DropTable(
                name: "PlayerEvents");

            migrationBuilder.DropTable(
                name: "Tracks");
        }
    }
}
