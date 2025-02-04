using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutbornE_commerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddSizeToBag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SizeId",
                table: "BagItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BagItems_SizeId",
                table: "BagItems",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BagItems_Sizes_SizeId",
                table: "BagItems",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BagItems_Sizes_SizeId",
                table: "BagItems");

            migrationBuilder.DropIndex(
                name: "IX_BagItems_SizeId",
                table: "BagItems");

            migrationBuilder.DropColumn(
                name: "SizeId",
                table: "BagItems");
        }
    }
}
