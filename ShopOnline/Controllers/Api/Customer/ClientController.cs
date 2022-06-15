using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business.Customer;
using ShopOnline.Business.Staff;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Filters;
using ShopOnline.Core.Helpers;
using ShopOnline.Core.Models.Client;
using ShopOnline.Core.Models.HistoryOrder;
using ShopOnline.Core.Models.Mobile;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Controllers.Customer
{
    public class ClientController : ApiController
    {
        private readonly IClientBusiness _clientBusiness;
        private readonly IOrderBusiness _orderBusiness;

        public ClientController(IClientBusiness clientBusiness,
            IOrderBusiness orderBusiness)
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

        [HttpGet("detail-product")]
        public async Task<IActionResult> ProductDetailAsync(int id)
        {
            var productDetail = await _clientBusiness.GetDetailProductAsync(id);
            return Ok(productDetail);
        }

        [AuthorizeFilter(TypeAcc.Customer)]
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

        [HttpGet("products-of-brand")]
        [AllowAnonymous]
        public async Task<IActionResult> GetListProductByBrandAsync([FromQuery] ProductParamsModel model)
        {
            var products = await _clientBusiness.GetListProductOfBrandAsync(model);

            var productsBrandPageViewModel = new ProductsOfBrandViewModel
            {
                Skip = model.Skip,
                Take = model.Take,
                AmountProduct = products.AmountProduct,
                TypeOfBrand = await _clientBusiness.GetTypesOfBrandAsync(model.BrandId),
                ProductsInfor = products.ProductsInfor,
            };
            return Ok(productsBrandPageViewModel);
        }

        [HttpGet("order-history")]
        [AuthorizeFilter(TypeAcc.Customer)]
        public async Task<IEnumerable<HistoryOrderInfor>> ListHistoryOrderCustomerAsync(string sortOrder, string currentFilter, int? page)
        => await _orderBusiness.GetHistoryOrderCustomerAsync();

        [HttpGet("brands")]
        [AllowAnonymous]
        public async Task<IEnumerable<BrandInforModel>> GetBrands()
        {
            var brands = await _clientBusiness.GetBrandAsync();

            return brands;
        }


        [HttpGet("popular-products")]
        [AllowAnonymous]
        public async Task<IEnumerable<ProductDetailModel>> GetPopularProducts()
        {
            var popularProducts = await _clientBusiness.GetPopularProductsAsync();

            return popularProducts;
        }

        [HttpGet("product-by-id-detail")]
        [AllowAnonymous]
        public async Task<ProductModel> GetProductByIdDetail(int idProductDetail)
        {
            var product = await _clientBusiness.GetProductByIdDetailAsync(idProductDetail);

            return product;
        }

        [HttpGet("favorite-products")]
        [AuthorizeFilter(TypeAcc.Customer)]
        public async Task<IEnumerable<ProductDetailModel>> GetFavoriteProducts()
        {
            var favoriteProducts = await _clientBusiness.GetFavoriteProductsAsync();

            return favoriteProducts;
        }

        [HttpGet("products-by-id-brand")]
        [AllowAnonymous]
        public async Task<IEnumerable<ProductDetailModel>> GetProductsByIdBrand(int idBrand)
        {
            var products = await _clientBusiness.GetProductsByIdBrandAsync(idBrand);

            return products;
        }

        [HttpGet("products-by-filter")]
        [AllowAnonymous]
        public async Task<IEnumerable<ProductDetailModel>> GetProductsByFilter(string searchTerm)
        {
            var products = await _clientBusiness.GetProductsByFilterAsync(searchTerm);

            return products;
        }

        [HttpGet("products-brand")]
        [AllowAnonymous]
        public async Task<IEnumerable<ProductDetailModel>> FilterProducts(int idBrand)
        {
            var products = await _clientBusiness.GetProductsByIdBrandAsync(idBrand);

            return products;
        }

    }
}
