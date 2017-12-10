using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Allocation.Controllers
{
    public class BaseApiController : ApiController
    {
        protected HttpResponseMessage BuildSuccessResult(HttpStatusCode statusCode)
        {
            return Request.CreateResponse(statusCode);
        }

        protected HttpResponseMessage BuildSuccessResult(HttpStatusCode statusCode, object data)
        {
            return data != null ? Request.CreateResponse(statusCode, data) : Request.CreateResponse(statusCode);
        }

        protected HttpResponseMessage BuildErrorResult(HttpStatusCode statusCode, object data)
        {
            return data != null ? Request.CreateResponse(statusCode, data) : Request.CreateResponse(statusCode);
        }

        protected HttpResponseMessage BuildErrorResult(HttpStatusCode statusCode, string errorCode = null, string message = null)
        {
            return Request.CreateResponse(statusCode, new
            {
                ErrorCode = errorCode,
                Message = message
            });
        }
    }
}
