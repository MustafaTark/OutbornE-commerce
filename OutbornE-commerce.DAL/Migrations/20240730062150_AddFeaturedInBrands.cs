using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OutbornE_commerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddFeaturedInBrands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "01efb1eb-8b7d-4216-a59c-5606f51a833a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "23183066-225f-4e5c-a049-6279d5323fce");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2da402fa-3b9c-4e8a-bc22-f348a1c63a1c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "40caf870-cf18-497d-9633-a17513cd06cd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ad445454-ee8f-4c3e-8c8d-f2b78a613126");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b177f44e-c194-4fb4-8679-4a50e2253464");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "Brands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "066cef6b-8e9f-40e1-9b51-685fecb7eb4d", null, "User", "USER" },
                    { "0cb9f307-1c24-4896-af39-1f8fb2050294", null, "Admin", "ADMIN" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "066cef6b-8e9f-40e1-9b51-685fecb7eb4d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0cb9f307-1c24-4896-af39-1f8fb2050294");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1fdf2578-22e5-440f-9200-56667d4880a3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "209fadce-db80-4c7d-8e4c-b1608845694d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57c340a3-9b20-424e-9d93-55475d699186");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7c59cc4-20c7-4eea-ab7a-41bfffa5ff38");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "Brands");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "01efb1eb-8b7d-4216-a59c-5606f51a833a", null, "Admin", "ADMIN" },
                    { "23183066-225f-4e5c-a049-6279d5323fce", null, "Admin", "ADMIN" },
                    { "2da402fa-3b9c-4e8a-bc22-f348a1c63a1c", null, "User", "USER" },
                    { "40caf870-cf18-497d-9633-a17513cd06cd", null, "User", "USER" },
                    { "ad445454-ee8f-4c3e-8c8d-f2b78a613126", null, "User", "USER" },
                    { "b177f44e-c194-4fb4-8679-4a50e2253464", null, "Admin", "ADMIN" }
                });
        }
    }
}
