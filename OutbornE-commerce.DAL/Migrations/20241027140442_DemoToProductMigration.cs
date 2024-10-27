using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OutbornE_commerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DemoToProductMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72851fc2-825c-434a-9c91-5b1e4404e81a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b109f4e9-127d-4f7c-9072-684ee9dfb293");

            migrationBuilder.AddColumn<string>(
                name: "DemoUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d5e6ed19-8190-4c71-8f37-5170d107487c", null, "User", "USER" },
                    { "dcef19f6-fc80-4d42-861f-c7d61144c029", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d5e6ed19-8190-4c71-8f37-5170d107487c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcef19f6-fc80-4d42-861f-c7d61144c029");

            migrationBuilder.DropColumn(
                name: "DemoUrl",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "72851fc2-825c-434a-9c91-5b1e4404e81a", null, "User", "USER" },
                    { "b109f4e9-127d-4f7c-9072-684ee9dfb293", null, "Admin", "ADMIN" }
                });
        }
    }
}
