using ShopOnline.Core.Models.Customer;
using ShopOnline.Core.Models.Product;
using System;
using System.Collections.Generic;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Core.Models.Review
{
    public class ReviewModel
    {
        public ReviewInfor ReviewInfor { get; set; }
        public List<ProductDetailInfor> ListProductDetail { get; set; }
        public List<CustomerInfor> ListCustomer { get; set; }
        public IPagedList<ReviewInfor> ListReview { get; set; }
    }
    public class ReviewInfor
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime ReviewTime { get; set; }
        public ReviewStatus ReviewStatus { get; set; }
        public int IdCustomer { get; set; }
        public int IdProductDetail { get; set; }
    }
}
