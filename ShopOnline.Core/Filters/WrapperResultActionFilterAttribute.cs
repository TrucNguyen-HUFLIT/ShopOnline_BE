using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopOnline.Core.Models.Base;
using System.Net;

namespace ShopOnline.Core.Filters
{
    public class WrapperResultActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            var result = context.Result as ObjectResult;

            // if exception is happening
            if (context.Exception != null)
            {
                return;
            }

            HttpStatusCode responseStatusCode;

            if (result != null && result.StatusCode.HasValue)
            {
                responseStatusCode = (HttpStatusCode)result.StatusCode.Value;
            }
            else
            {
                responseStatusCode = HttpStatusCode.OK;
            }

            // setting response model
            var baseApiResponse = new ApiResponseModel
            {
                Data = result?.Value,
                StatusCode = responseStatusCode,
                Message = "success",
                ErrorDetails = null,
                ModelValidationErrors = null,
            };

            context.Result = new ObjectResult(baseApiResponse)
            {
                StatusCode = result?.StatusCode ?? (int)HttpStatusCode.OK
            };
        }
    }
}
