using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Demo.Results
{
    public class ResponseApi<T> where T : class
    {
        public ResponseApi(HttpStatusCode ok, String message = null, T data = null)
        {
            this.ok = ok;
            this.data = data;
            this.message = message;

        }
        public ResponseApi(HttpStatusCode ok, String message = null, T data = null, String developerMessage = null, int errorCode = 0)
        {
            this.ok = ok;
            this.data = data;
            this.message = message;
            this.developerMessage = developerMessage;
            this.errorCode = errorCode;
        }
        public ResponseApi(Exception e)
        {
            this.data = null;
            if (e is CustomException)
            {
                this.ok = HttpStatusCode.PreconditionFailed;
                this.message = "ha ocurrido un error de aplicacion";
                this.data = null;
                this.errorCode = ((CustomException)e).errorCode;
                this.developerMessage = ((CustomException)e).Message;
            }
            else
            {
                this.ok = HttpStatusCode.InternalServerError;
                this.message = "ha ocurrido un error no controlado";
                if ((e.InnerException != null) && (e.InnerException.Message != null))
                    this.developerMessage = e.InnerException.Message;
                else
                    this.developerMessage = e.Message;
            }
        }

        public HttpStatusCode ok { get; set; }
        public String message { get; set; }
        public T data { get; set; }
        public String developerMessage { get; set; }
        public int errorCode { get; set; }
    }
}
