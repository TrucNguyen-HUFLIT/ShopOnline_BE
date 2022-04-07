
using System.Collections.Generic;

namespace ShopOnline.Core.Entities
{
    public class ProductTypeEntity : BaseEntity
    {
        public string Name { get; set; }
        public int IdBrand { get; set; }
        public BrandEntity Brand { get; set; }

        public virtual ICollection<ProductDetailEntity> ProductDetails { get; set; }
    }
}
