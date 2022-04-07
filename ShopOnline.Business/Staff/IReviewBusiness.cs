using ShopOnline.Core.Models.Customer;
using ShopOnline.Core.Models.Product;
using ShopOnline.Core.Models.Review;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Business.Staff
{
    public interface IReviewBusiness
    {
        Task<IPagedList<ReviewInfor>> GetListReviewAsync(string sortOrder, string currentFilter, ReviewStatus reviewStatus, int? page);
        Task<List<CustomerInfor>> GetListCustomer();
        Task<List<ProductDetailInfor>> GetListProductDetail();
        Task<bool> ApproveReview(int id);
        Task<bool> RejectReview(int id);
    }
}
