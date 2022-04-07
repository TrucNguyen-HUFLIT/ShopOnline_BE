using System;

namespace ShopOnline.Core.Models.Client
{
    public class ReviewDetailModel
    {
        public int IdCustomer { get; set; }
        public int IdProductDetail { get; set; }
        public string Content { get; set; }
    }

    public class ReviewDetailViewModel : ReviewDetailModel
    {
        public int Id { get; set; }
        public DateTime ReviewTime { get; set; }
        public string CustomerFullName { get; set; }
    }
}
