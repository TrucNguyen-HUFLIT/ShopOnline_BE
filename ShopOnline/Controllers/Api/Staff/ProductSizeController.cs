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
    [Route("api/product-size")]
    public class ProductSizeController : ApiController
    {
        private readonly IProductBusiness _productBusiness;
        private readonly IClientBusiness _clientBusiness;

        public ProductSizeController(IProductBusiness productBusiness,
                    IClientBusiness clientBusiness)
        {
            _productBusiness = productBusiness;
            _clientBusiness = clientBusiness;
        }

        [HttpGet("")]
        public async Task<PagedCollectionResultModel<ProductInfor>> GetListProductsAsync([FromQuery] ProductParamsModel model)
        {
            var products = await _productBusiness.GetListProductAsync(model);
            return products;
        }


        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpPost("create")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> CreateProductAsync([FromBody] ProductCreate model)
        {
            var staff = await _productBusiness.CreateProductAsync(model);
            return Ok(staff);
        }

        [AuthorizeFilter(TypeAcc.Manager)]
        [AllowAnonymous]
        [HttpGet("detail")]
        public async Task<IActionResult> GetProductByIdAsync([FromQuery] int id)
        {
            var model = await _productBusiness.GetProductByIdAsync(id);
            return Ok(model);
        }

        [AuthorizeFilter(TypeAcc.Manager)]
        [HttpPut("update")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        public async Task<IActionResult> UpdateStaff([FromBody] ProductUpdate model)
        {
            await _productBusiness.UpdateProductAsync(model);
            return Ok(model.Id);
        }
    }
}
