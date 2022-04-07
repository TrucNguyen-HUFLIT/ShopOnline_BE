using System.Collections.Generic;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Models.Client
{
    public class ProductDetailPageViewModel
    {
        public ProductDetailViewModel ProductDetail { get; set; }
        public List<ProductInforModel> ProductInfors { get; set; }
    }

    public class ProductDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Pic1 { get; set; }
        public string Pic2 { get; set; }
        public string Pic3 { get; set; }
        public string Description { get; set; }
        public int PriceVND { get; set; }
        public int PriceUSD { get; set; }
        public BrandInforModel BrandInfor { get; set; }
        public ProductStatus Status { get; set; }
        public List<BaseProductInfor> BaseProductInfors { get; set; }
        public List<ReviewDetailViewModel> ReviewsDetail { get; set; }

    }

    public class BaseProductInfor
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public ProductSize Size { get; set; }
        public bool IsAvailable { get; set; }
    }
}
