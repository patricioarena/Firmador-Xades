using Demo.Enums;
using Demo.Handlers;
using Demo.Models;
using FirmarPDFLibrary;
using FirmaXadesNet.Crypto;
using FirmaXadesNet.Utils;
using Helper.Model;
using Helper.Results;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Web.Http;

namespace Demo.Controllers
{
    [RoutePrefix("api/Signature")]
    public class SignatureController : BaseController
    {
        private static string reason = Properties.Settings.Default.Reason;

        private static string country = Properties.Settings.Default.Country;

        private static bool addVisibleSign = Properties.Settings.Default.AddVisibleSign;

        private IDecisionHandler Decision { get; set; }

        private IVerificationHandler Verification { get; set; }

        public SignatureController(IDecisionHandler decision, IVerificationHandler verification)
        {
            this.Decision = decision;
            this.Verification = verification;
        }

        /// <summary>
        /// Obtiene un certificado digital desde el almacén de certificados del usuario. 
        /// Se utiliza para verificar el acceso a los recursos locales del usuario.
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
            catch (Exception ex)
            {
                return CustomErrorStatusCode(ex);
            }
        }

        /// <summary>
        /// Verifica que la aplicación esté corriendo correctamente. Utilizado para pruebas de conectividad desde otras aplicaciones.
        /// </summary>
        [HttpGet]
        [Route("ping")]
        public IHttpActionResult Ping()
        {
            return Content(HttpStatusCode.OK, true, Configuration.Formatters.JsonFormatter);
        }

        /// <summary>
        /// Permite firmar un archivo PDF/A en la máquina del usuario abriendo el explorador de archivos.
        /// El PDF firmado se guarda localmente, simulando el proceso manual de firma mediante la UI.
        /// </summary>
        [HttpGet]
        [Route("PDF/Signature")]
        public void PDFSignature()
        {
            Thread _thread = new Thread((ThreadStart)(() =>
            {
                Signature.GetInstance().PDFSignatureHandler();
            }));

            // Ejecute el código desde un hilo con estado STA para el manejo adecuado de UI
            _thread.SetApartmentState(ApartmentState.STA);
            _thread.Start();
            _thread.Join();
        }

        /// <summary>
        /// Recibe un archivo PDF/A codificado en Base64, lo firma digitalmente, y retorna el PDF firmado en Base64.
        /// Valida que el archivo siga el estándar PDF/A.
        /// </summary>
        /// <param name="model">El modelo que contiene el PDF a firmar y los detalles de la firma.</param>
        /// <returns>El PDF/A firmado en formato Base64.</returns>
        [HttpPost]
        [Route("PDF/Signature/Sign/Base64")]
        public IHttpActionResult PDFSignatureSign([FromBody] PdfToSign model)
        {
            try
            {
                if (model == null)
                    throw new CustomException(CustomException.ErrorsEnum.ModelNull);

                if (string.IsNullOrWhiteSpace(model.PdfBase64))
                    throw new CustomException(CustomException.ErrorsEnum.PdfNull);

                if (string.IsNullOrEmpty(model.Reason))
                    model.Reason = reason;

                byte[] data = Convert.FromBase64String(model.PdfBase64);
                if (!ValidatorForRestful.IsValidPDFA(new MemoryStream(data)))
                    throw new CustomException(CustomException.ErrorsEnum.PdfInvalido);

                X509Certificate2 certificate = new Signer(CertUtil.SelectCertificate()).Certificate;
                if (!ValidatorForRestful.IsValidX509Certificate2(certificate))
                    throw new CustomException(CustomException.ErrorsEnum.InvalidCert);

                var signedPdfBase64 = PDFSigner.Sign(new MemoryStream(data), certificate, reason, country, addVisibleSign);

                ProcessResult result = new ProcessResult
                {
                    Data = new List<string> { signedPdfBase64 },
                    Code = StatusSignProcess.Good.ToString()
                };

                return Ok(new ResponseApi<ProcessResult>(HttpStatusCode.OK, "Firmas Electronicas", result));
            }
            catch (Exception ex)
            {
                return CustomErrorStatusCode(ex);
            }
        }

        /// <summary>
        /// Realiza una firma electrónica única sobre un archivo XML, con opción de utilizar la verificación mediante OCSP.
        /// </summary>
        /// <param name="typeSignature">Tipo de firma electrónica. Original(1) o CIFE(2)</param>
        /// <param name="model">El modelo que contiene los datos del XML a firmar.</param>
        /// <param name="usarComprobaciónPorOCSP">Indica si se debe utilizar verificación por OCSP.</param>
        /// <returns>Resultado de la firma electrónica.</returns>
        [HttpPost]
        [Route("Single/TypeSignature/{typeSignature}/oscp/{usarComprobaciónPorOCSP}")]
        public IHttpActionResult SingleElectronic(string typeSignature, [FromBody] XmlToSign model, bool usarComprobaciónPorOCSP)
        {
            try
            {
                ProcessResult result = this.Decision.CoreDecision(typeSignature, model, usarComprobaciónPorOCSP);
                return Ok(new ResponseApi<ProcessResult>(HttpStatusCode.OK, usarComprobaciónPorOCSP ? "Firmas Digitales" : "Firmas Electronicas", result));
            }
            catch (Exception ex)
            {
                return CustomErrorStatusCode(ex);
            }
        }

        /// <summary>
        /// Realiza una firma electrónica en masa sobre múltiples archivos XML, con opción de utilizar la verificación mediante OCSP.
        /// </summary>
        /// <param name="typeSignature">Tipo de firma electrónica. Original(1) o CIFE(2)</param>
        /// <param name="model">Lista de modelos XML a firmar.</param>
        /// <param name="usarComprobaciónPorOCSP">Indica si se debe utilizar verificación por OCSP.</param>
        /// <returns>Resultado de la firma electrónica en masa.</returns>
        [HttpPost]
        [Route("Bulk/TypeSignature/{typeSignature}/oscp/{usarComprobaciónPorOCSP}")]
        public IHttpActionResult BulkElectronic(string typeSignature, [FromBody] List<XmlToSign> model, bool usarComprobaciónPorOCSP)
        {
            try
            {
                ProcessResult result = this.Decision.BulkCoreDecision(typeSignature, model, usarComprobaciónPorOCSP);
                return Ok(new ResponseApi<ProcessResult>(HttpStatusCode.OK, usarComprobaciónPorOCSP ? "Firmas Digitales" : "Firmas Electronicas", result));
            }
            catch (Exception ex)
            {
                return CustomErrorStatusCode(ex);
            }
        }

        /// <summary>
        /// Verifica la existencia de una o más firmas en un archivo XML.
        /// </summary>
        /// <param name="model">El modelo XML en el que se verifican las firmas.</param>
        /// <returns>Un objeto JSON que indica si hay una o más firmas presentes.</returns>
        [HttpPost]
        [Route("Verify/Exist/One/Or/More/Signatures")]
        public IHttpActionResult ExistSignatures([FromBody] XmlToSign model)
        {
            try
            {
                JObject SignatureList = this.Verification.ExistOneOrMoreSignatures(model);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, "Firmas", SignatureList));
            }
            catch (Exception ex)
            {
                return CustomErrorStatusCode(ex);
            }
        }

        /// <summary>
        /// Verifica la validez de múltiples firmas en un archivo XML.
        /// </summary>
        /// <param name="model">El modelo XML en el que se verifican las firmas.</param>
        /// <returns>Una lista de objetos JSON con los resultados de la verificación de las firmas.</returns>
        [HttpPost]
        [Route("Verify/Multi/Signatures")]
        public IHttpActionResult CheckMultiSignatures([FromBody] XmlToSign model)
        {
            try
            {
                List<JObject> SignatureList = this.Verification.CheckSignatures(model);
                return Ok(new ResponseApi<List<JObject>>(HttpStatusCode.OK, "Firmas", SignatureList));
            }
            catch (Exception ex)
            {
                return CustomErrorStatusCode(ex);
            }
        }
    }
}