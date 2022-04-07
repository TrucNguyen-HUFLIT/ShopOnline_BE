using System.Net;

namespace ShopOnline.Core.Models.Base
{
    public class ApiResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public object ModelValidationErrors { get; set; }

        public object ErrorDetails { get; set; }

        public object Data { get; set; }
    }
}
