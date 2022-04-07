using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopOnline.Data.Migrations
{
    public partial class UpdateFieldsOrderSectionAndUpdateMyDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "OrderDetail",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "Payment",
                table: "OrderDetail",
                newName: "TotalBasePrice");

            migrationBuilder.AddColumn<int>(
                name: "QuantityPurchased",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Payment",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityPurchased",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "Payment",
                table: "Order");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "OrderDetail",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "TotalBasePrice",
                table: "OrderDetail",
                newName: "Payment");
        }
    }
}
