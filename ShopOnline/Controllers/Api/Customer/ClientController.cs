using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business.Customer;
using ShopOnline.Business.Staff;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Helpers;
using ShopOnline.Core.Models;
using ShopOnline.Core.Models.Client;
using ShopOnline.Core.Models.HistoryOrder;
using System;
using System.Threading.Tasks;

namespace ShopOnline.Controllers.Customer
{
    public class ClientController : ApiController
    {
        private readonly IClientBusiness _clientBusiness;
        private readonly IOrderBusiness _orderBusiness;

        public ClientController(IClientBusiness clientBusiness, IOrderBusiness orderBusiness)
        {
            _clientBusiness = clientBusiness;
            _orderBusiness = orderBusiness;
        }

        [HttpGet("Home")]
        public async Task<IActionResult> HomeAsync()
        {
            await _clientBusiness.InitBrands();
            const int TAKE_4 = 4;
            var products = await _clientBusiness.GetProductsAsync(TAKE_4);

            var productHomePageViewModel = new ProductHomePageViewModel { ProductInfors = products };
            return Ok(productHomePageViewModel);
        }

        [HttpGet("DetailProduct")]
        public async Task<IActionResult> DetailProductAsync(int id)
        {
            const int TAKE_8 = 8;
            var productDetail = await _clientBusiness.GetDetailProductAsync(id);
            var products = await _clientBusiness.GetCurrentProductsInforAsync(TAKE_8);

            var productDetailPageViewModel = new ProductDetailPageViewModel()
            {
                ProductDetail = productDetail,
                ProductInfors = products,
            };
            return Ok(productDetailPageViewModel);
        }

        [Authorize(Roles = ROLE.CUSTOMER)]
        [HttpPost("CreateReviewDetail")]
        public async Task<IActionResult> CreateReviewDetailAsync(ReviewDetailModel reviewDetail)
        {
            await _clientBusiness.CreateReviewDetailAsync(reviewDetail, User);
            return Ok();
        }

        [HttpGet("Products")]
        public async Task<IActionResult> ProductsAsync(int brandId, int? typeId, bool sortIncrease, int? page)
        {
            var products = await _clientBusiness.GetProductsByBrandAsync(brandId, typeId);

            //ViewBag.CurrentSort = sortIncrease;
            //ViewBag.CurrentType = typeId;

            var productInforsPaged = await PagingHelper.ClientProductsPagingAsync(sortIncrease, page, products.ProductsInfor);

            var productsBrandPageViewModel = new ProductsBrandPageViewModel
            {
                AmountProduct = products.AmountProduct,
                TypeOfBrand = await _clientBusiness.GetTypesOfBrandAsync(brandId),
                ProductsInfor = productInforsPaged,
            };

            return Ok(productsBrandPageViewModel);
        }

        [Authorize(Roles = ROLE.CUSTOMER)]
        [HttpGet("ListHistoryOrderCustomer")]
        public async Task<IActionResult> ListHistoryOrderCustomerAsync(string sortOrder, string currentFilter, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;

            var model = new HistoryOrderModel
            {
                ListHistoryOrder = await _orderBusiness.GetHistoryOrderCustomerAsync(sortOrder, currentFilter, page, User)
            };

            return Ok(model);
        }
    }
}
