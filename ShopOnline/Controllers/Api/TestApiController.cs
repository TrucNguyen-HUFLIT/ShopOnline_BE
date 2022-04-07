using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ShopOnline.Controllers.Api
{
    public class TestApiController : ApiController
    {
        private const string GetEndpoint = "test-get-api";

        /// <summary>
        ///     Test Get API
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(GetEndpoint)]
        [AllowAnonymous]
        public string Get()
        {
            return "Test get api";
        }
    }
}
