using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Business.Staff;
using ShopOnline.Controllers.Api;
using ShopOnline.Core.Models.Customer;
using ShopOnline.Core.Validators.Paging;
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

        [HttpGet("")]
        public async Task<PagedCollectionResultModel<CustomerInfor>> ListCustomer([FromQuery] CustomerParamsModel model)
        {
            var customers = await _customerBusiness.GetListCustomerAsync(model);
            return customers;
        }

    }
}
