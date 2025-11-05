using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMG_s_Game_Repo.Migrations
{
    /// <inheritdoc />
    public partial class AddFavoritesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1a2b3c4d-5e6f-7g8h-9i0j-1k2l3m4n5o6p",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "974faf66-d51f-45f9-b7b6-d43e5895d186", "AQAAAAIAAYagAAAAEBhX12tYa8ntiwLgfX2yqYi+kCMw1TvnaorgJX4Wo5A/3UDySUwiZdxvQUDgZDpkCQ==", "feb406bd-f1db-4698-972b-7a2b2c47b373" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2b3c4d5e-6f7g-8h9i-0j1k-2l3m4n5o6p7q",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "568b5d55-edf1-4362-bd3b-4d010ee76fd3", "AQAAAAIAAYagAAAAED1eiTxt6wp4j7Tw2awQqsV/ZHWQiTD5v3TXmhV3E+0QOSgmPgtNq6lrarmTvf8xyg==", "ed47f5a5-849d-4bc3-b328-fd4bc2e7c7a9" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "3c4d5e6f-7g8h-9i0j-1k2l-3m4n5o6p7q8r",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "dfb452af-3850-4994-98c8-c3dfc7c86a09", "AQAAAAIAAYagAAAAECAY1JdtYT7gL48iEJi6YwsnNrXE36lCCnLKxlv0unpVUG/OoTpMnudnPXYWWxE8JA==", "368cab2c-619d-412e-ade5-127b6b8a3057" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
