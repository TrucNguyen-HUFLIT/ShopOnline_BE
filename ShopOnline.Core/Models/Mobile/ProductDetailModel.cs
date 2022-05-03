using ShopOnline.Core.Models.Client;
using System.Collections.Generic;

namespace ShopOnline.Core.Models.Mobile
{
    public class ProductDetailModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public int PriceUSD { get; set; }
        public int PriceVND { get; set; }
        public string Pic { get; set; }
    }

    public class ProductModel : ProductDetailModel
    {
        public IEnumerable<BaseProductInfor> ProductSizes { get; set; }
        public string Description { get; set; }
        public string Pic2 { get; set; }
        public string Pic3 { get; set; }
    }
}
