using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMG_s_Game_Repo.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGameTableSimplifyFavorites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Games_GameId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGames_Games_GameId",
                table: "UserGames");

            migrationBuilder.DropTable(
                name: "GameGenre");

            migrationBuilder.DropTable(
                name: "GamePlatform");

            migrationBuilder.DropTable(
                name: "Screenshot");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropIndex(
                name: "IX_UserGames_GameId",
                table: "UserGames");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_GameId",
                table: "Favorites");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "UserGames",
                newName: "RawgId");

            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "Favorites",
                newName: "RawgId");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1a2b3c4d-5e6f-7g8h-9i0j-1k2l3m4n5o6p",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fc9779ec-3f4c-4c33-b812-bfe87370950e", "AQAAAAIAAYagAAAAENqLd+Ze4meBlWkYECwatJhQL0fO++jRRoWRPIPpR/JPi9qZVNuR0XhxOArqKbzJ2w==", "d053fa59-9f2c-4a43-a58c-6c9dd38d415d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2b3c4d5e-6f7g-8h9i-0j1k-2l3m4n5o6p7q",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fb1ca48e-e0ba-4cdd-8857-53e2e8b55f57", "AQAAAAIAAYagAAAAEKHbYYvm72GuIS7UrpNCHdUzPzgcm6tEgxLc/OKMbC51msyDbikzYhVfNtGXHKEw9A==", "99f7ad4d-7caf-42f5-8dac-cdbbf40760b4" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c4d5e6f-7g8h-9i0j-1k2l-3m4n5o6p7q8r",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "84588b80-704c-45da-860e-f548cc5b1802", "AQAAAAIAAYagAAAAEIF0u+WUDRqjhsi4S0OngJCLq6FH/erUt1LmJHQyM9ttF5fE+lHl/b5CsD22LYmj+Q==", "9588104b-1ad4-4562-ac9e-8f957119ba13" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RawgId",
                table: "UserGames",
                newName: "GameId");

            migrationBuilder.RenameColumn(
                name: "RawgId",
                table: "Favorites",
                newName: "GameId");

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BackgroundImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    RawgId = table.Column<int>(type: "int", nullable: false),
                    Released = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameGenre",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    GenresId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenre", x => new { x.GamesId, x.GenresId });
                    table.ForeignKey(
                        name: "FK_GameGenre_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGenre_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GamePlatform",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    PlatformsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlatform", x => new { x.GamesId, x.PlatformsId });
                    table.ForeignKey(
                        name: "FK_GamePlatform_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlatform_Platforms_PlatformsId",
                        column: x => x.PlatformsId,
                        principalTable: "Platforms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Screenshot",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenshot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Screenshot_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1a2b3c4d-5e6f-7g8h-9i0j-1k2l3m4n5o6p",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "92245ecb-f76a-47c1-b26f-3eb54c98c7a5", "AQAAAAIAAYagAAAAENkuHLUiJe0W0dbl3KlTU6BUcYbw8KCxlyuL/sDA2xT+hW5EaOSE+XEhJRZz+9EI0g==", "82e581c9-6294-4a1d-a6e7-68224a453fe3" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2b3c4d5e-6f7g-8h9i-0j1k-2l3m4n5o6p7q",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d078e92d-dee9-4262-99ae-7ca17fc20907", "AQAAAAIAAYagAAAAEJe/Yl/a7N8qx/1ofmgjOZEX0Li/TQ6ebCpGUmIJ3YX31G9ddKfw8eSy/sxrcR1kUg==", "8b61f5d1-e00f-47f0-b325-19a2dbc15972" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c4d5e6f-7g8h-9i0j-1k2l-3m4n5o6p7q8r",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "8001c899-9508-4ab7-a6a8-6fecf51aa13b", "AQAAAAIAAYagAAAAEPWEebG6/eN/1j9sQpdy69zBKVuY1rNff65bDiKLCAx5BD/bJg2C+RLxz9zAa1LCqg==", "13aa88e9-95ea-40eb-863e-91c90a8aac60" });

            migrationBuilder.CreateIndex(
                name: "IX_UserGames_GameId",
                table: "UserGames",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_GameId",
                table: "Favorites",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameGenre_GenresId",
                table: "GameGenre",
                column: "GenresId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlatform_PlatformsId",
                table: "GamePlatform",
                column: "PlatformsId");

            migrationBuilder.CreateIndex(
                name: "IX_Screenshot_GameId",
                table: "Screenshot",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Games_GameId",
                table: "Favorites",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGames_Games_GameId",
                table: "UserGames",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
