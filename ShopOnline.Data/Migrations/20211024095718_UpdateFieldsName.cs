using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopOnline.Data.Migrations
{
    public partial class UpdateFieldsName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Brand");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "ReviewDetail",
                newName: "ReviewTime");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "ProductDetail",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ProductSize",
                table: "Product",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Product",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "BrandName",
                table: "Brand",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewTime",
                table: "ReviewDetail",
                newName: "DateTime");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ProductDetail",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "Product",
                newName: "ProductSize");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Product",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Brand",
                newName: "BrandName");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Brand",
                type: "bit",
                nullable: true);
        }
    }
}
