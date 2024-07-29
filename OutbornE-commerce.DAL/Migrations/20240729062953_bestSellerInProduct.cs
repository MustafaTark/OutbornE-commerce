using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OutbornE_commerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class bestSellerInProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0682cc2e-8b21-4561-90a5-f4c9d4dbd83a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1865e48d-435f-450e-a789-25cf015dc6bf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7331654a-0977-45b3-8c5c-3d918b6d4a92");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "78f6a6e6-4f37-4b6c-8cc8-1b470e938cd6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7d91cb43-63d5-48b8-8887-445111967ac2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a05bad23-96b3-4ae6-b924-4b6d0e679bbf");

            migrationBuilder.AddColumn<bool>(
                name: "IsBestSeller",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3ca5ca62-d19e-427b-be27-08594908c4ba", null, "Admin", "ADMIN" },
                    { "5a6b0533-5613-4d9b-9a86-4584baca8caa", null, "User", "USER" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ca5ca62-d19e-427b-be27-08594908c4ba");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5a6b0533-5613-4d9b-9a86-4584baca8caa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "90682956-d9c4-4627-87d9-42778a6498fb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9c8781b6-27ff-4cca-aabb-5a1e1302f217");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ab6d6d92-e799-40c8-b9eb-36487852687c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fed78c70-2ea6-42aa-95e8-f272e3d669d9");

            migrationBuilder.DropColumn(
                name: "IsBestSeller",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0682cc2e-8b21-4561-90a5-f4c9d4dbd83a", null, "User", "USER" },
                    { "1865e48d-435f-450e-a789-25cf015dc6bf", null, "Admin", "ADMIN" },
                    { "7331654a-0977-45b3-8c5c-3d918b6d4a92", null, "Admin", "ADMIN" },
                    { "78f6a6e6-4f37-4b6c-8cc8-1b470e938cd6", null, "User", "USER" },
                    { "7d91cb43-63d5-48b8-8887-445111967ac2", null, "Admin", "ADMIN" },
                    { "a05bad23-96b3-4ae6-b924-4b6d0e679bbf", null, "User", "USER" }
                });
        }
    }
}
