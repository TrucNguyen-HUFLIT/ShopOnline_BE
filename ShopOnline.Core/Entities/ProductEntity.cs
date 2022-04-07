
using System.Collections.Generic;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public ProductSize Size { get; set; }

        public int IdProductDetail { get; set; }
        public ProductDetailEntity ProductDetail { get; set; }
        public virtual IList<OrderDetailEntity> OrderDetails { get; set; }
    }
}
