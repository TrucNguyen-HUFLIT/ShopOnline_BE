using System.Collections.Generic;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Models.Product
{
    public class ProductModel
    {
        public ProductInfor productInfor { get; set; }
        public List<ProductDetailInfor> ListProductDetail { get; set; }
        public IPagedList<ProductInfor> ListProduct { get; set; }
    }

    public class ProductBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public ProductSize Size { get; set; }
        public int IdProductDetail { get; set; }
    }

    public class ProductInfor : ProductBase
    {
        public string Pic1 { get; set; }
        public string Pic2 { get; set; }
        public string Pic3 { get; set; }
    }

    public class ProductCreate : ProductBase
    {
    }
    public class ProductUpdate : ProductBase
    {

    }

    public class ProductCreateViewModel
    {
        public List<ProductSize> ListProductSize { get; set; }
        public List<ProductDetailInfor> ListProductDetail { get; set; }
    }

    public class ProductUpdateViewModel
    {
        public ProductUpdate ProductUpdate { get; set; }
        public List<ProductDetailInfor> ListProductDetail { get; set; }
    }
}
