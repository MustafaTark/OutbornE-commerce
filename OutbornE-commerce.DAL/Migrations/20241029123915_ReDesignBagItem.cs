using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OutbornE_commerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReDesignBagItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BagItems_Colors_ColorId",
                table: "BagItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BagItems_Sizes_SizeId",
                table: "BagItems");

            migrationBuilder.DropIndex(
                name: "IX_BagItems_SizeId",
                table: "BagItems");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d5e6ed19-8190-4c71-8f37-5170d107487c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dcef19f6-fc80-4d42-861f-c7d61144c029");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "BagItems");

            migrationBuilder.AlterColumn<Guid>(
                name: "ColorId",
                table: "BagItems",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5b7ccfd6-467c-49cf-b2ea-4fecc0a78fae", null, "User", "USER" },
                    { "833d18b4-5405-418c-866e-346a92611f23", null, "Admin", "ADMIN" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BagItems_Colors_ColorId",
                table: "BagItems",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BagItems_Colors_ColorId",
                table: "BagItems");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5b7ccfd6-467c-49cf-b2ea-4fecc0a78fae");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "833d18b4-5405-418c-866e-346a92611f23");

            migrationBuilder.AlterColumn<Guid>(
                name: "ColorId",
                table: "BagItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SizeId",
                table: "BagItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d5e6ed19-8190-4c71-8f37-5170d107487c", null, "User", "USER" },
                    { "dcef19f6-fc80-4d42-861f-c7d61144c029", null, "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BagItems_SizeId",
                table: "BagItems",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BagItems_Colors_ColorId",
                table: "BagItems",
                column: "ColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BagItems_Sizes_SizeId",
                table: "BagItems",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
