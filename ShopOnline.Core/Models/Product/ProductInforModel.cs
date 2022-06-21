using ShopOnline.Core.Validators.Paging;
using System.Collections.Generic;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Models.Product
{
    public class ProductInforModel
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
        public int Size { get; set; }
        public int IdProductDetail { get; set; }
    }

    public class ProductInfor : ProductBase
    {
    }

    public class ProductParamsModel : PagedCollectionParametersModel
    {
        public bool IsDescending { get; set; }
        public ProductSortByEnum SortBy { get; set; }
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
