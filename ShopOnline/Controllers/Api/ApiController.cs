using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ShopOnline.Core.Filters;

namespace ShopOnline.Controllers.Api
{
    [Route("api/[controller]")]
    [Authorize]
    [ServiceFilter(typeof(ValidateActionFilterAttribute))]
    [ServiceFilter(typeof(ValidationModelFilterAttribute))]
    [ServiceFilter(typeof(WrapperResultActionFilterAttribute))]
    [EnableCors("AllowAllCorsPolicy")]
    public class ApiController : ControllerBase
    {
    }
}
