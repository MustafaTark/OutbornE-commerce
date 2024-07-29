using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OutbornE_commerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddProductTypeToPRoduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "190b987b-6677-48e2-97bf-161efe25e16a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "49ddfab3-2b5c-4ae6-8cd1-437d861cebe2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5341a8e7-ad6d-4ad5-8afe-8786e27bb2f1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5398aac8-2fbe-48ac-9236-fd25f76d98da");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "76d7075c-ada6-492c-9707-90114c825a5b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e4bd6e5e-2977-44eb-b723-6ac8b5d7782b");

            migrationBuilder.AddColumn<int>(
                name: "ProductType",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "23183066-225f-4e5c-a049-6279d5323fce", null, "Admin", "ADMIN" },
                    { "2da402fa-3b9c-4e8a-bc22-f348a1c63a1c", null, "User", "USER" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "190b987b-6677-48e2-97bf-161efe25e16a", null, "Admin", "ADMIN" },
                    { "49ddfab3-2b5c-4ae6-8cd1-437d861cebe2", null, "User", "USER" },
                    { "5341a8e7-ad6d-4ad5-8afe-8786e27bb2f1", null, "Admin", "ADMIN" },
                    { "5398aac8-2fbe-48ac-9236-fd25f76d98da", null, "User", "USER" },
                    { "76d7075c-ada6-492c-9707-90114c825a5b", null, "Admin", "ADMIN" },
                    { "e4bd6e5e-2977-44eb-b723-6ac8b5d7782b", null, "User", "USER" }
                });
        }
    }
}
