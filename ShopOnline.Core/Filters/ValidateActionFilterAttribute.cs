using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopOnline.Core.Models.Base;

namespace ShopOnline.Core.Filters
{
    public sealed class ValidateActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
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
