using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PMG_s_Game_Repo.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUsersViaContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "IsAdmin", "IsBanned", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileDescription", "ProfilePictureUrl", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1a2b3c4d-5e6f-7g8h-9i0j-1k2l3m4n5o6p", 0, "92245ecb-f76a-47c1-b26f-3eb54c98c7a5", "georgeadmin@pmggamerepo.com", true, true, false, false, null, "GEORGEADMIN@PMGGAMEREPO.COM", "GEORGEADMIN", "AQAAAAIAAYagAAAAENkuHLUiJe0W0dbl3KlTU6BUcYbw8KCxlyuL/sDA2xT+hW5EaOSE+XEhJRZz+9EI0g==", null, false, null, "https://i.ibb.co/2Wj9WzN/default-avatar.png", "82e581c9-6294-4a1d-a6e7-68224a453fe3", false, "georgeadmin" },
                    { "2b3c4d5e-6f7g-8h9i-0j1k-2l3m4n5o6p7q", 0, "d078e92d-dee9-4262-99ae-7ca17fc20907", "miroslavadmin@pmggamerepo.com", true, true, false, false, null, "MIROSLAVADMIN@PMGGAMEREPO.COM", "MIROSLAVADMIN", "AQAAAAIAAYagAAAAEJe/Yl/a7N8qx/1ofmgjOZEX0Li/TQ6ebCpGUmIJ3YX31G9ddKfw8eSy/sxrcR1kUg==", null, false, null, "https://i.ibb.co/2Wj9WzN/default-avatar.png", "8b61f5d1-e00f-47f0-b325-19a2dbc15972", false, "miroslavadmin" },
                    { "3c4d5e6f-7g8h-9i0j-1k2l-3m4n5o6p7q8r", 0, "8001c899-9508-4ab7-a6a8-6fecf51aa13b", "petaradmin@pmggamerepo.com", true, true, false, false, null, "PETARADMIN@PMGGAMEREPO.COM", "PETARADMIN", "AQAAAAIAAYagAAAAEPWEebG6/eN/1j9sQpdy69zBKVuY1rNff65bDiKLCAx5BD/bJg2C+RLxz9zAa1LCqg==", null, false, null, "https://i.ibb.co/2Wj9WzN/default-avatar.png", "13aa88e9-95ea-40eb-863e-91c90a8aac60", false, "petaradmin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1a2b3c4d-5e6f-7g8h-9i0j-1k2l3m4n5o6p");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2b3c4d5e-6f7g-8h9i-0j1k-2l3m4n5o6p7q");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c4d5e6f-7g8h-9i0j-1k2l-3m4n5o6p7q8r");
        }
    }
}
