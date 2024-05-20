using Helper.Model;
using Demo.Results;
using FirmaXadesNet.Crypto;
using FirmaXadesNet.Utils;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Threading;
using Demo.Handlers;
using Demo.Models;

namespace Demo.Controllers
{
    [RoutePrefix("api/Signature")]
    public class SignatureController : BaseController
    {
        IDecisionHandler Decision { get; set; }
        IVerificationHandler Verification { get; set; }

        public SignatureController(IDecisionHandler decision, IVerificationHandler verification)
        {
            this.Decision = decision;
            this.Verification = verification;
        }

        /// <summary>
        /// Para probar el acceso a los certificados del almacen de certificados y recursos locales de la pc del usuario
        /// </summary>
        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            try
            {
                X509Certificate2 certificate = new Signer(CertUtil.SelectCertificate()).Certificate;
                return Ok(certificate);
            }
            catch (System.Exception ex)
            {
                return CustomErrorStatusCode(ex);
            }
        }

        /// <summary>
        /// Para probar que la aplicacion esta corriendo desde otras aplicaciones que consuman la api de esta misma
        /// </summary>
        [HttpGet]
        [Route("ping")]
        public IHttpActionResult Ping()
        {
            return Content(HttpStatusCode.OK, true, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// Firmar PDF/A 
        /// </summary>
        [HttpGet]
        [Route("PDF/Signature")]
        public void PDFSignatureHandler()
        {
            Thread _thread = new Thread((ThreadStart)(() => {
                Signature.GetInstance().PDFSignatureHandler();
            }));

            // Ejecute su código desde un hilo que se une al hilo de STA
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
            _thread.Join();
        }

        [HttpPost]
        [Route("Single/{typeSignature}/{usarComprobaciónPorOCSP}")]
        public IHttpActionResult SingleElectronic(string typeSignature, [FromBody] ObjetoModel model, bool usarComprobaciónPorOCSP)
        {
            try
            {
                // Verificar la decisión de usar o no la comprobación por OCSP
                if (usarComprobaciónPorOCSP)
                {
                    ProcessResult result = this.Decision.CoreDecision(typeSignature, model, true); // Se utiliza la comprobación por OCSP
                    return Ok(new ResponseApi<ProcessResult>(HttpStatusCode.OK, "Firmas Digitales", result));
                }
                else
                {
                    ProcessResult result = this.Decision.CoreDecision(typeSignature, model, false); // No se utiliza la comprobación por OCSP
                    return Ok(new ResponseApi<ProcessResult>(HttpStatusCode.OK, "Firmas Electronicas", result));
                }
            }
            catch (System.Exception ex)
            {
                return CustomErrorStatusCode(ex); // Manejo de excepciones internas del servidor
            }
        }

        /// <summary>
        /// Realiza una firma electrónica a granel basada en la decisión de usar o no la comprobación por OCSP.
        /// </summary>
        /// <param name="typeSignature">Tipo de firma electrónica.</param>
        /// <param name="model">Lista de objetos de modelo.</param>
        /// <param name="usarComprobaciónPorOCSP">Indica si se debe utilizar la comprobación por OCSP (0 para no, 1 para sí).</param>
        /// <returns>Una acción HTTP que representa el resultado de la firma electrónica.</returns>
        [HttpPost]
        [Route("Bulk/{typeSignature}/{usarComprobaciónPorOCSP}")]
        public IHttpActionResult BulkElectronic(string typeSignature, [FromBody] List<ObjetoModel> model, bool usarComprobaciónPorOCSP)
        {
            try
            {
                // Verificar la decisión de usar o no la comprobación por OCSP
                if (usarComprobaciónPorOCSP)
                {
                    ProcessResult result = this.Decision.BulkCoreDecision(typeSignature, model, true); // Se utiliza la comprobación por OCSP
                    return Ok(new ResponseApi<ProcessResult>(HttpStatusCode.OK, "Firmas Digitales", result));
                }
                else
                {
                    ProcessResult result = this.Decision.BulkCoreDecision(typeSignature, model, false); // No se utiliza la comprobación por OCSP
                    return Ok(new ResponseApi<ProcessResult>(HttpStatusCode.OK, "Firmas Electronicas", result));
                }
            }
            catch (System.Exception ex)
            {
                return CustomErrorStatusCode(ex); // Manejo de excepciones internas del servidor
            }
        }

        /// <summary>
        /// Verifica si existe una o mas firmas
        /// </summary>
        /// Xades Original :: 1
        [HttpPost]
        [Route("Verify/1")]
        public IHttpActionResult ExistSignatures([FromBody] ObjetoModel model)
        {
            try
            {
                JObject SignatureList = this.Verification.ExistOneOrMoreSignatures(model);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, "Firmas", SignatureList));
            }
            catch (System.Exception ex)
            {
                return CustomErrorStatusCode(ex);
            }
        }

        /// <summary>
        /// Verifica valides de multiples firmas
        /// </summary>
        /// Xades CIFE :: 2
        [HttpPost]
        [Route("Verify/2")]
        public IHttpActionResult CheckMultiSignatures([FromBody] ObjetoModel model)
        {
            try
            {
                List<JObject> SignatureList = this.Verification.CheckSignatures(model);
                return Ok(new ResponseApi<List<JObject>>(HttpStatusCode.OK, "Firmas", SignatureList));
            }
            catch (System.Exception ex)
            {
                return CustomErrorStatusCode(ex);
            }
        }
    }
}
