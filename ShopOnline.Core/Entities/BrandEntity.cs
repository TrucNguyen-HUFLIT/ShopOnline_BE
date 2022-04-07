using System.Collections.Generic;

namespace ShopOnline.Core.Entities
{
    public class BrandEntity : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<ProductTypeEntity> ProductTypes { get; set; }
    }
}
