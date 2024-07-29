using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OutbornE_commerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddLabelInProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "Label",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7657e7ee-dd47-46c0-a39a-181f9eb97cca", null, "Admin", "ADMIN" },
                    { "81664205-2585-4ccf-8bff-75716f3bd177", null, "User", "USER" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1f43cc51-347a-47dc-89bb-5fb7334db02d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7657e7ee-dd47-46c0-a39a-181f9eb97cca");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81664205-2585-4ccf-8bff-75716f3bd177");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "900349f6-9b92-42b9-a3db-230442af45c8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98af5544-4a8c-4f62-aa56-a60f1aecb522");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc55e2ee-7ac6-445a-befa-89f078391e03");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "Products");

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
                    { "90682956-d9c4-4627-87d9-42778a6498fb", null, "Admin", "ADMIN" },
                    { "9c8781b6-27ff-4cca-aabb-5a1e1302f217", null, "User", "USER" },
                    { "ab6d6d92-e799-40c8-b9eb-36487852687c", null, "User", "USER" },
                    { "fed78c70-2ea6-42aa-95e8-f272e3d669d9", null, "Admin", "ADMIN" }
                });
        }
    }
}
