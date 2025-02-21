using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OutbornE_commerce.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddShippingCostToCity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ShippingCost",
                table: "Cities",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingCost",
                table: "Cities");
        }
    }
}
