using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business.Customer;
using ShopOnline.Business.Staff;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Filters;
using ShopOnline.Core.Models;
using ShopOnline.Core.Models.Product;
using System;
using System.Linq;
using System.Threading.Tasks;
using static ShopOnline.Core.Models.Enum.AppEnum;

namespace ShopOnline.Controllers.Staff
{
    public class ProductController : ApiController
    {
        private readonly IProductBusiness _productBusiness;
        private readonly IClientBusiness _clientBusiness;

        public ProductController(IProductBusiness productBusiness,
                    IClientBusiness clientBusiness)
        {
            _productBusiness = productBusiness;
            _clientBusiness = clientBusiness;
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("ListBrand")]
        public async Task<IActionResult> ListBrand(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

            //ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";

            //StaticAcc.Name = User.Claims.Where(x => x.Type == "name").FirstOrDefault().Value;

            if (searchString != null) page = 1;
            else searchString = currentFilter;
            //ViewBag.CurrentFilter = searchString;

            var model = new BrandModel
            {
                ListBrand = await _productBusiness.GetListBrandAsync(sortOrder, currentFilter, searchString, page)
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("CreateBrand")]
        public IActionResult CreateBrand()
        {
            var model = new BrandCreateViewModel
            {
                BrandCreate = new BrandCreate(),
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpPost("CreateBrand")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> CreateBrand([FromForm] BrandCreate brandCreate)
        {
            await _clientBusiness.InitBrands();
            await _productBusiness.CreateBrandAsync(brandCreate);
            return Ok();
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("UpdateBrand")]
        public async Task<IActionResult> UpdateBrand(int id)
        {
            var model = new BrandInforViewModel
            {
                BrandInfor = await _productBusiness.GetBrandByIdAsync(id),
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpPost("UpdateBrand")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        public async Task<IActionResult> UpdateBrand(BrandInfor brandInfor)
        {
            await _clientBusiness.InitBrands();
            await _productBusiness.EditBrandAsync(brandInfor);
            return Ok(brandInfor.Id);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("DeleteBrandAsync")]
        public async Task<IActionResult> DeleteBrandAsync(int id)
        {
            await _productBusiness.DeleteBrandAsync(id);
            return Ok();
        }


        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("ListProductType")]
        public async Task<IActionResult> ListProductType(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

            //ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";

            //StaticAcc.Name = User.Claims.Where(x => x.Type == "name").FirstOrDefault().Value;

            if (searchString != null) page = 1;
            else searchString = currentFilter;
            //ViewBag.CurrentFilter = searchString;

            var model = new ProductTypeModel
            {
                ListBrand = await _productBusiness.GetListBrand(),
                ListProductType = await _productBusiness.GetListProductTypeAsync(sortOrder, currentFilter, searchString, page)
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("CreateProductType")]
        public async Task<IActionResult> CreateProductType()
        {
            var model = new ProductTypeViewModel
            {
                productType = new ProductTypeInfor(),
                ListBrand = await _productBusiness.GetListBrand(),
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpPost("CreateProductType")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> CreateProductType([FromForm] ProductTypeInfor productType)
        {
            await _productBusiness.CreateProductTypeAsync(productType);
            return Ok();
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("UpdateProductType")]
        public async Task<IActionResult> UpdateProductTypeAsync(int id)
        {
            var model = new ProductTypeViewModel
            {
                productType = _productBusiness.GetProductTypeByIdAsync(id),
                ListBrand = await _productBusiness.GetListBrand(),
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpPost("UpdateProductType")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> UpdateProductType(ProductTypeInfor productType)
        {
            await _productBusiness.EditProductTypeAsync(productType);
            return Ok(productType.Id);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("DeleteProductType")]
        public async Task<IActionResult> DeleteProductTypeAsync(int id)
        {
            await _productBusiness.DeleteProductTypeAsync(id);
            return Ok();
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("CreateProductDetail")]
        public async Task<IActionResult> CreateProductDetail()
        {
            var model = new ProductDetailCreateViewModel
            {
                ProductDetailCreate = new ProductDetailCreate(),
                ListProductType = await _productBusiness.GetListProductType(),
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpPost("CreateProductDetail")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> CreateProductDetail([FromForm] ProductDetailCreate productDetailCreate)
        {
            await _productBusiness.CreateProductDetailAsync(productDetailCreate);
            return Ok();
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("UpdateProductDetail")]
        public async Task<IActionResult> UpdateProductDetail(int id)
        {
            var model = new ProductDetailUpdateViewModel
            {
                ProductDetailUpdate = _productBusiness.GetProductDetailByIdAsync(id),
                ListProductType = await _productBusiness.GetListProductType(),
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpPost("UpdateProductDetail")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        public async Task<IActionResult> UpdateProductDetail(ProductDetailUpdate productDetailUpdate)
        {
            await _productBusiness.UpdateProductDetailAsync(productDetailUpdate);
            return Ok(productDetailUpdate.Id);
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("DeleteProductDetail")]
        public async Task<IActionResult> DeleteProductDetailAsync(int id)
        {
            await _productBusiness.DeleteProductDetailAsync(id);
            return Ok();
        }

        [Authorize(Roles = ROLE.MANAGER)]
        [HttpGet("ListProductDetail")]
        public async Task<IActionResult> ListProductDetail(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

            //ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";

            //StaticAcc.Name = User.Claims.Where(x => x.Type == "name").FirstOrDefault().Value;

            if (searchString != null) page = 1;
            else searchString = currentFilter;
            //ViewBag.CurrentFilter = searchString;

            var model = new ProductDetailModel
            {
                ListProductType = await _productBusiness.GetListProductType(),
                ListProductDetail = await _productBusiness.GetListProductDetailAsync(sortOrder, currentFilter, searchString, page)
            };

            return Ok(model);
        }

        [Authorize(Roles = ROLE.STAFF)]
        [HttpGet("GetListProductDetail")]
        public async Task<IActionResult> GetListProductDetail()
        {
            var listProductDetail = await _productBusiness.GetListProductDetail();
            return Ok(listProductDetail);
        }

        [Authorize(Roles = ROLE.STAFF)]
        [HttpGet("CreateProduct")]
        public async Task<IActionResult> CreateProduct()
        {
            var model = new ProductCreateViewModel
            {
                ListProductSize = Enum.GetValues(typeof(ProductSize)).Cast<ProductSize>().ToList(),
                ListProductDetail = await _productBusiness.GetListProductDetail(),
            };

            return Ok(model);
        }

        [Authorize(Roles = ROLE.STAFF)]
        [HttpPost("CreateProduct")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        [TypeFilter(typeof(ExceptionFilter))]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreate productCreate)
        {
            await _productBusiness.CreateProductAsync(productCreate);
            return Ok();
        }

        [Authorize(Roles = ROLE.STAFF)]
        [HttpGet("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var model = new ProductUpdateViewModel
            {
                ProductUpdate = await _productBusiness.GetProductByIdAsync(id),
                ListProductDetail = await _productBusiness.GetListProductDetail(),
            };
            return Ok(model);
        }

        [Authorize(Roles = ROLE.STAFF)]
        [HttpPost("UpdateProduct")]
        [TypeFilter(typeof(ModelStateAjaxFilter))]
        public async Task<IActionResult> UpdateProduct(ProductUpdate productUpdate)
        {
            await _productBusiness.UpdateProductAsync(productUpdate);
            return Ok(productUpdate.Id);
        }

        [Authorize(Roles = ROLE.STAFF)]
        [HttpGet("DeleteProduct")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            await _productBusiness.DeleteProductAsync(id);
            return Ok();
        }

        [Authorize(Roles = ROLE.STAFF)]
        [HttpGet("ListProduct")]
        public async Task<IActionResult> ListProduct(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

            //ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";

            //StaticAcc.Name = User.Claims.Where(x => x.Type == "name").FirstOrDefault().Value;

            if (searchString != null) page = 1;
            else searchString = currentFilter;
            //ViewBag.CurrentFilter = searchString;

            var model = new ProductModel
            {
                ListProductDetail = await _productBusiness.GetListProductDetail(),
                ListProduct = await _productBusiness.GetListProductAsync(sortOrder, currentFilter, searchString, page)
            };

            return Ok(model);
        }
    }
}
