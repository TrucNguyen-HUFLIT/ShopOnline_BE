using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopOnline.Core.Models.Base;

namespace ShopOnline.Core.Filters
{
    public class ValidationModelFilterAttribute : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.ModelState.IsValid == false)
            {
                var badRequest = new BadRequestObjectResult(context.ModelState);
                var baseApiResponse = new ApiResponseModel
                {
                    Message = "Model validation Error(s)",
                    Data = null,
                    ErrorDetails = null,
                    ModelValidationErrors = badRequest.Value,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                context.Result = new BadRequestObjectResult(baseApiResponse);
            }
        }
    }
}
