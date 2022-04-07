

namespace ShopOnline.Core.Entities
{
    public class FavoriteProductEntity : BaseEntity
    {
        public int IdCustomer { get; set; }
        public CustomerEntity Customer { get; set; }

        public int IdProductDetail { get; set; }
        public ProductDetailEntity ProductDetail { get; set; }
    }
}
