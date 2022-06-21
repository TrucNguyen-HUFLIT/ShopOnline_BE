using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business.Customer;
using ShopOnline.Business.Staff;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Filters;
using ShopOnline.Core.Models.Product;
using ShopOnline.Core.Validators.Paging;
using System;
using System.Linq;
using System.Threading.Tasks;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Controllers.Staff
{
    [Route("api/product-detail")]
    public class ProductDetailController : ApiController
    {
        private readonly IProductBusiness _productBusiness;
        private readonly IClientBusiness _clientBusiness;

        public ProductDetailController(IProductBusiness productBusiness,
                    IClientBusiness clientBusiness)
        {
            _productBusiness = productBusiness;
            _clientBusiness = clientBusiness;
        }

        [HttpGet("")]
        public async Task<PagedCollectionResultModel<ProductDetailInfor>> GetListProductDetailsAsync([FromQuery] ProductDetailParamsModel model)
        {
            var productDetails = await _productBusiness.GetListProductDetailsAsync(model);
            return productDetails;
        }


        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpPost("create")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> CreateProductDetailAsync([FromBody] ProductDetailCreate model)
        {
            var staff = await _productBusiness.CreateProductDetailAsync(model);
            return Ok(staff);
        }

        [AuthorizeFilter(TypeAcc.Manager)]
        [AllowAnonymous]
        [HttpGet("detail")]
        public async Task<IActionResult> GetProductDetailByIdAsync([FromQuery] int id)
        {
            var model = await _productBusiness.GetProductDetailByIdAsync(id);
            return Ok(model);
        }

        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpPut("update")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        public async Task<IActionResult> UpdateStaff([FromBody] ProductDetailUpdate model)
        {
            await _productBusiness.UpdateProductDetailAsync(model);
            return Ok(model.Id);
        }
    }
}
