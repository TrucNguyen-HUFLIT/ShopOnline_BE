using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business.Staff;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Models;
using ShopOnline.Core.Models.Review;
using System;
using System.Threading.Tasks;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Controllers.Staff
{
    public class ReviewController : ApiController
    {
        private readonly IReviewBusiness _reviewBusiness;

        public ReviewController(IReviewBusiness reviewBusiness)
        {
            _reviewBusiness = reviewBusiness;
        }

        [Authorize(Roles = ROLE.STAFF)]
        [HttpGet("ListReview")]
        public async Task<IActionResult> ListReviewAsync(string sortOrder, string currentFilter, int reviewStatus, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.ReviewTimeSortParm = String.IsNullOrEmpty(sortOrder) ? "review-time" : "";

            if (page == 0 || page == null) page = 1;
            reviewStatus = reviewStatus == 0 ? 1 : reviewStatus;
            //ViewBag.CurrentFilter = reviewStatus;

            var enumReviewStatus = (ReviewStatus)reviewStatus;

            var model = new ReviewModel
            {
                ListProductDetail = await _reviewBusiness.GetListProductDetail(),
                ListCustomer = await _reviewBusiness.GetListCustomer(),
                ListReview = await _reviewBusiness.GetListReviewAsync(sortOrder, currentFilter, enumReviewStatus, page)
            };
            return Ok(model);
        }

        [HttpPost("ApproveReview")]
        public async Task<IActionResult> ApproveReview(int id)
        {
            await _reviewBusiness.ApproveReview(id);
            return Ok();
        }

        [HttpPost("RejectReview")]
        public async Task<IActionResult> RejectReview(int id)
        {
            await _reviewBusiness.RejectReview(id);
            return Ok();
        }

    }
}
