using System;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Entities
{
    public class ReviewDetailEntity : BaseEntity
    {
        public string Content { get; set; }
        public DateTime ReviewTime { get; set; }
        public ReviewStatus ReviewStatus { get; set; }

        public int IdCustomer { get; set; }
        public CustomerEntity Customer { get; set; }

        public int IdProductDetail { get; set; }
        public ProductDetailEntity ProductDetail { get; set; }
    }
}
