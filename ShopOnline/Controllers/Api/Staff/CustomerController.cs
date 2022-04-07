using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business.Staff;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Models.Customer;
using System;
using System.Threading.Tasks;

namespace ShopOnline.Controllers.Staff
{
    public class CustomerController : ApiController
    {
        private readonly ICustomerBusiness _customerBusiness;

        public CustomerController(ICustomerBusiness customerBusiness)
        {
            _customerBusiness = customerBusiness;
        }

        [HttpGet("ListCustomer")]
        public async Task<IActionResult> ListCustomer(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) || sortOrder.Equals("name") ? "name_desc" : "name";

            //ViewBag.IdSortParm = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";

            if (searchString != null) page = 1;
            else searchString = currentFilter;
            //ViewBag.CurrentFilter = searchString;

            var model = new CustomerModel
            {
                ListCustomer = await _customerBusiness.GetListCustomerAsync(sortOrder, currentFilter, searchString, page)
            };
            return Ok(model);
        }

    }
}
