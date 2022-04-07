using System.Collections.Generic;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Models.Client
{
    public static class CART
    {
        public const string CART_KEY = "cart";

    }

    public class ProductCartModel
    {
        public int Id { get; set; }
        public int IdProductDetail { get; set; }
        public string Name { get; set; }
        public int PriceVND { get; set; }
        public int PriceUSD { get; set; }
        public int BasePrice { get; set; }
        public int TotalVND { get; set; }
        public int TotalUSD { get; set; }
        public int TotalBasePrice { get; set; }
        public string Pic { get; set; }
        public int Quantity { get; set; }
        public int SelectedQuantity { get; set; }
        public ProductSize Size { get; set; }
    }

    public class ProductCartViewModel
    {
        public IEnumerable<ProductCartModel> ProductCarts { get; set; }
        public UserInfor UserInfor { get; set; }
    }

    public class OrderInfor
    {
        public int Id { get; set; }
        public string Addess { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int PriceVND { get; set; }
    }
}
