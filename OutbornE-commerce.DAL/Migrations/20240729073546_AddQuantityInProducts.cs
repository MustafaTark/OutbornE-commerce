using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OutbornE_commerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddQuantityInProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "QuantityInStock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "190b987b-6677-48e2-97bf-161efe25e16a", null, "Admin", "ADMIN" },
                    { "49ddfab3-2b5c-4ae6-8cd1-437d861cebe2", null, "User", "USER" },
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "QuantityInStock",
                table: "Products");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1f43cc51-347a-47dc-89bb-5fb7334db02d", null, "Admin", "ADMIN" },
                    { "7657e7ee-dd47-46c0-a39a-181f9eb97cca", null, "Admin", "ADMIN" },
                    { "81664205-2585-4ccf-8bff-75716f3bd177", null, "User", "USER" },
                    { "900349f6-9b92-42b9-a3db-230442af45c8", null, "User", "USER" },
                    { "98af5544-4a8c-4f62-aa56-a60f1aecb522", null, "User", "USER" },
                    { "fc55e2ee-7ac6-445a-befa-89f078391e03", null, "Admin", "ADMIN" }
                });
        }
    }
}
