using Helper.Model;
using Demo.Results;
using Helper.Services;
using FirmaXadesNet;
using FirmaXadesNet.Clients;
using FirmaXadesNet.Crypto;
using FirmaXadesNet.Signature;
using FirmaXadesNet.Signature.Parameters;
using FirmaXadesNet.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Http;
using System.Xml;
using Org.BouncyCastle.X509;
using System.Configuration;
using Demo.Enums;
using System.Web.Services.Description;
using System.Threading;
using System.Windows.Controls;
using Demo.Handlers;
using Demo.Models;
using System.Web.Http.Results;

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
