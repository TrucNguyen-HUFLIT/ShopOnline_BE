using Microsoft.EntityFrameworkCore.Migrations;

namespace ShopOnline.Data.Migrations
{
    public partial class AddFieldAvatarAndChangeTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_IdOrder",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Products_IdProduct",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_IdCustomer",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetails_ProductTypes_IProductType",
                table: "ProductDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductDetails_IdProductDetail",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductTypes_Brands_IdBrand",
                table: "ProductTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDetails_Customers_IdCustomer",
                table: "ReviewDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDetails_ProductDetails_IdProductDetail",
                table: "ReviewDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Staffs",
                table: "Staffs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewDetails",
                table: "ReviewDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductTypes",
                table: "ProductTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDetails",
                table: "ProductDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brands",
                table: "Brands");

            migrationBuilder.RenameTable(
                name: "Staffs",
                newName: "Staff");

            migrationBuilder.RenameTable(
                name: "ReviewDetails",
                newName: "ReviewDetail");

            migrationBuilder.RenameTable(
                name: "ProductTypes",
                newName: "ProductType");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "ProductDetails",
                newName: "ProductDetail");

            migrationBuilder.RenameTable(
                name: "Orders",
                newName: "Order");

            migrationBuilder.RenameTable(
                name: "OrderDetails",
                newName: "OrderDetail");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameTable(
                name: "Brands",
                newName: "Brand");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewDetails_IdProductDetail",
                table: "ReviewDetail",
                newName: "IX_ReviewDetail_IdProductDetail");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewDetails_IdCustomer",
                table: "ReviewDetail",
                newName: "IX_ReviewDetail_IdCustomer");

            migrationBuilder.RenameIndex(
                name: "IX_ProductTypes_IdBrand",
                table: "ProductType",
                newName: "IX_ProductType_IdBrand");

            migrationBuilder.RenameIndex(
                name: "IX_Products_IdProductDetail",
                table: "Product",
                newName: "IX_Product_IdProductDetail");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDetails_IProductType",
                table: "ProductDetail",
                newName: "IX_ProductDetail_IProductType");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_IdCustomer",
                table: "Order",
                newName: "IX_Order_IdCustomer");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetails_IdProduct",
                table: "OrderDetail",
                newName: "IX_OrderDetail_IdProduct");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Staff",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Customer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Staff",
                table: "Staff",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewDetail",
                table: "ReviewDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductType",
                table: "ProductType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDetail",
                table: "ProductDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail",
                columns: new[] { "IdOrder", "IdProduct" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brand",
                table: "Brand",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Customer_IdCustomer",
                table: "Order",
                column: "IdCustomer",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Order_IdOrder",
                table: "OrderDetail",
                column: "IdOrder",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Product_IdProduct",
                table: "OrderDetail",
                column: "IdProduct",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductDetail_IdProductDetail",
                table: "Product",
                column: "IdProductDetail",
                principalTable: "ProductDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetail_ProductType_IProductType",
                table: "ProductDetail",
                column: "IProductType",
                principalTable: "ProductType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductType_Brand_IdBrand",
                table: "ProductType",
                column: "IdBrand",
                principalTable: "Brand",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDetail_Customer_IdCustomer",
                table: "ReviewDetail",
                column: "IdCustomer",
                principalTable: "Customer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDetail_ProductDetail_IdProductDetail",
                table: "ReviewDetail",
                column: "IdProductDetail",
                principalTable: "ProductDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Customer_IdCustomer",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Order_IdOrder",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Product_IdProduct",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductDetail_IdProductDetail",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductDetail_ProductType_IProductType",
                table: "ProductDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductType_Brand_IdBrand",
                table: "ProductType");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDetail_Customer_IdCustomer",
                table: "ReviewDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewDetail_ProductDetail_IdProductDetail",
                table: "ReviewDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Staff",
                table: "Staff");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewDetail",
                table: "ReviewDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductType",
                table: "ProductType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductDetail",
                table: "ProductDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderDetail",
                table: "OrderDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Brand",
                table: "Brand");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Staff");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Staff",
                newName: "Staffs");

            migrationBuilder.RenameTable(
                name: "ReviewDetail",
                newName: "ReviewDetails");

            migrationBuilder.RenameTable(
                name: "ProductType",
                newName: "ProductTypes");

            migrationBuilder.RenameTable(
                name: "ProductDetail",
                newName: "ProductDetails");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "OrderDetail",
                newName: "OrderDetails");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "Orders");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameTable(
                name: "Brand",
                newName: "Brands");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewDetail_IdProductDetail",
                table: "ReviewDetails",
                newName: "IX_ReviewDetails_IdProductDetail");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewDetail_IdCustomer",
                table: "ReviewDetails",
                newName: "IX_ReviewDetails_IdCustomer");

            migrationBuilder.RenameIndex(
                name: "IX_ProductType_IdBrand",
                table: "ProductTypes",
                newName: "IX_ProductTypes_IdBrand");

            migrationBuilder.RenameIndex(
                name: "IX_ProductDetail_IProductType",
                table: "ProductDetails",
                newName: "IX_ProductDetails_IProductType");

            migrationBuilder.RenameIndex(
                name: "IX_Product_IdProductDetail",
                table: "Products",
                newName: "IX_Products_IdProductDetail");

            migrationBuilder.RenameIndex(
                name: "IX_OrderDetail_IdProduct",
                table: "OrderDetails",
                newName: "IX_OrderDetails_IdProduct");

            migrationBuilder.RenameIndex(
                name: "IX_Order_IdCustomer",
                table: "Orders",
                newName: "IX_Orders_IdCustomer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Staffs",
                table: "Staffs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewDetails",
                table: "ReviewDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductTypes",
                table: "ProductTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductDetails",
                table: "ProductDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderDetails",
                table: "OrderDetails",
                columns: new[] { "IdOrder", "IdProduct" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Brands",
                table: "Brands",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_IdOrder",
                table: "OrderDetails",
                column: "IdOrder",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Products_IdProduct",
                table: "OrderDetails",
                column: "IdProduct",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_IdCustomer",
                table: "Orders",
                column: "IdCustomer",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductDetails_ProductTypes_IProductType",
                table: "ProductDetails",
                column: "IProductType",
                principalTable: "ProductTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductDetails_IdProductDetail",
                table: "Products",
                column: "IdProductDetail",
                principalTable: "ProductDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTypes_Brands_IdBrand",
                table: "ProductTypes",
                column: "IdBrand",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDetails_Customers_IdCustomer",
                table: "ReviewDetails",
                column: "IdCustomer",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewDetails_ProductDetails_IdProductDetail",
                table: "ReviewDetails",
                column: "IdProductDetail",
                principalTable: "ProductDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
