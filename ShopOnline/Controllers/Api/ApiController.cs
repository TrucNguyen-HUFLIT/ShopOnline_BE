using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Core.Filters;

namespace ShopOnline.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    //[AuthorizeFilter(TypeAcc.Staff, TypeAcc.Admin, TypeAcc.Manager, TypeAcc.Customer, TypeAcc.Shipper)]
    [ServiceFilter(typeof(ValidateActionFilterAttribute))]
    [ServiceFilter(typeof(ValidationModelFilterAttribute))]
    [ServiceFilter(typeof(WrapperResultActionFilterAttribute))]
    [EnableCors("AllowAllCorsPolicy")]
    public class ApiController : ControllerBase
    {
    }
}
