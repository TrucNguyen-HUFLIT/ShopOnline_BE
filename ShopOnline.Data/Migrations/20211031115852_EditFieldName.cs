using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopOnline.Data.Migrations
{
    public partial class EditFieldName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetail_ProductType_IProductType",
                table: "ProductDetail");

            migrationBuilder.RenameColumn(
                name: "IProductType",
                table: "ProductDetail",
                newName: "IdProductType");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDetail_IProductType",
                table: "ProductDetail",
                newName: "IX_ProductDetail_IdProductType");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetail_ProductType_IdProductType",
                table: "ProductDetail",
                column: "IdProductType",
                principalTable: "ProductType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetail_ProductType_IdProductType",
                table: "ProductDetail");

            migrationBuilder.RenameColumn(
                name: "IdProductType",
                table: "ProductDetail",
                newName: "IProductType");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDetail_IdProductType",
                table: "ProductDetail",
                newName: "IX_ProductDetail_IProductType");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetail_ProductType_IProductType",
                table: "ProductDetail",
                column: "IProductType",
                principalTable: "ProductType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
