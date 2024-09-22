using Helper.Results;
using System;
using System.Net;
using System.Web.Http;

namespace Demo.Controllers
{

    public class BaseController : ApiController
    {
        public BaseController()
        {
        }

        public IHttpActionResult CustomErrorStatusCode(Exception e)
        {
            if (e is CustomException)
            {
                var errorCode = ((CustomException)e).errorCode;
                var message = ((CustomException)e).Message;
                if (errorCode == 403)
                    return Content(HttpStatusCode.Forbidden, new ResponseApi<object>(HttpStatusCode.PreconditionFailed, "ha ocurrido un error", null, e.InnerException != null ? e.InnerException.Message : message, errorCode, ex: e.ToString()));
                else
                    return Content(HttpStatusCode.PreconditionFailed, new ResponseApi<object>(HttpStatusCode.PreconditionFailed, "ha ocurrido un error", null, e.InnerException != null ? e.InnerException.Message : message, errorCode, ex: e.ToString()));
            }
            else
            {
                return Content(HttpStatusCode.InternalServerError, new ResponseApi<object>(HttpStatusCode.InternalServerError, "ha ocurrido un error", null, e.InnerException != null ? e.InnerException.Message : e.Message, ex: e.ToString()));
            }
        }
    }
}
