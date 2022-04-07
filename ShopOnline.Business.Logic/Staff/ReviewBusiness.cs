using Microsoft.EntityFrameworkCore;
using ShopOnline.Business.Staff;
using ShopOnline.Core;
using ShopOnline.Core.Models.Customer;
using ShopOnline.Core.Models.Product;
using ShopOnline.Core.Models.Review;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Business.Logic.Staff
{
    public class ReviewBusiness : IReviewBusiness
    {
        private readonly MyDbContext _context;

        public ReviewBusiness(MyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ApproveReview(int id)
        {
            var review = await _context.ReviewDetails.Where(x => x.Id == id && x.ReviewStatus == ReviewStatus.Waiting && !x.IsDeleted).FirstOrDefaultAsync();
            review.ReviewStatus = ReviewStatus.Approved;
            _context.ReviewDetails.Update(review);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CustomerInfor>> GetListCustomer()
        {
            var customers = await _context.Customers.Where(x => !x.IsDeleted).Select(x => new CustomerInfor
            {
                Id = x.Id,
                Address = x.Address,
                Avatar = x.Avatar,
                Email = x.Email,
                FullName = x.FullName,
                PhoneNumber = x.PhoneNumber,
            }).ToListAsync();
            return customers;
        }

        public async Task<List<ProductDetailInfor>> GetListProductDetail()
        {
            var productDetails = await _context.ProductDetails.Where(x => !x.IsDeleted).Select(x => new ProductDetailInfor
            {
                Id = x.Id,
                BasePrice = x.BasePrice,
                Description = x.Description,
                Name = x.Name,
                Pic1 = x.Pic1,
                Pic2 = x.Pic2,
                Pic3 = x.Pic3,
                Price = x.Price,
                Status = x.Status,
            }).ToListAsync();
            return productDetails;
        }

        public async Task<IPagedList<ReviewInfor>> GetListReviewAsync(string sortOrder, string currentFilter, ReviewStatus reviewStatus, int? page)
        {
            var queryReview = _context.ReviewDetails.Where(x => !x.IsDeleted);

            if (reviewStatus != 0)
            {
                queryReview = queryReview.Where(x => x.ReviewStatus == reviewStatus);
            }

            queryReview = sortOrder switch
            {
                "review-time" => queryReview.OrderBy(x => x.ReviewTime),
                _ => queryReview.OrderByDescending(x => x.ReviewTime),
            };

            var listReview = await queryReview.Select(review => new ReviewInfor
            {
                Id = review.Id,
                Content = review.Content,
                ReviewStatus = review.ReviewStatus,
                ReviewTime = review.ReviewTime,
                IdCustomer = review.IdCustomer,
                IdProductDetail = review.IdProductDetail,
            }).ToArrayAsync();

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return listReview.ToPagedList(pageNumber, pageSize);
        }

        public async Task<bool> RejectReview(int id)
        {
            var review = await _context.ReviewDetails.Where(x => x.Id == id && x.ReviewStatus == ReviewStatus.Waiting && !x.IsDeleted).FirstOrDefaultAsync();
            review.ReviewStatus = ReviewStatus.Rejected;
            _context.ReviewDetails.Update(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
