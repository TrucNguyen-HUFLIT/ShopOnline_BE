
using System.Collections.Generic;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Entities
{
    public class ProductDetailEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Pic1 { get; set; }
        public string Pic2 { get; set; }
        public string Pic3 { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int BasePrice { get; set; }
        public ProductStatus Status { get; set; }

        public int IdProductType { get; set; }
        public ProductTypeEntity ProductType { get; set; }
        public ICollection<ProductEntity> Products { get; set; }
        public virtual ICollection<ReviewDetailEntity> ReviewDetails { get; set; }
        public virtual ICollection<FavoriteProductEntity> FavoriteProducts { get; set; }
    }
}
