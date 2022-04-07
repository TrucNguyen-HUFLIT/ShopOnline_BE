using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ShopOnline.Core.Exceptions;
using ShopOnline.Core.Models.Base;
using ShopOnline.Infrastructure.Helper;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PostalCode.Infrastructure.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var responseData = new ApiResponseModel
                {
                    Data = null,
                    Message = ex.Message,
                    ErrorDetails = null,
                };

                // ignore these
                if (ex is OperationCanceledException || ex is ObjectDisposedException)
                {
                    return;
                }

                if (ex is UnauthorizedException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    responseData.StatusCode = HttpStatusCode.Forbidden;
                }
                else if (ex is BadRequestException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    responseData.StatusCode = HttpStatusCode.BadRequest;
                }
                else if (ex is NotFoundException)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    responseData.StatusCode = HttpStatusCode.NotFound;
                }
                else
                {
                    // write log for exception
                    LogHelper.Log(ex);

                    responseData.ErrorDetails = ex.StackTrace;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    responseData.StatusCode = HttpStatusCode.InternalServerError;
                }

                // serialize data to json
                var responseJson = JsonConvert.SerializeObject(responseData);
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(responseJson);
            }
        }
    }
}
