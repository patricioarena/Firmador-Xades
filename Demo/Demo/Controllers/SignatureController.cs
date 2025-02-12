using Demo.Enums;
using Demo.Extensions;
using Demo.Handlers;
using Demo.Models;
using Demo.Utils;
using FirmarPDFLibrary;
using FirmaXadesNetCore.Crypto;
using Helper.Model;
using Helper.Results;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignatureController(ISignatureSelectorHandler signatureSelector, IVerificationHandler verification) : ControllerBase
    {
        private static readonly string Reason = Properties.Settings.Default.Reason;

        private static readonly string Country = Properties.Settings.Default.Country;

        private ISignatureSelectorHandler SignatureSelector { get; } = signatureSelector;

        private IVerificationHandler Verification { get; } = verification;

        [HttpGet]
        [Route("version")]
        public IActionResult GetVersion()
        {
            try
            {
                InfoApp infoApp = new InfoApp { ApplicationName = "Authentica", Version = "Not Published" };
#if DEBUG
                infoApp.ApplicationName = Properties.Settings.Default.ApplicationName;
                infoApp.Version = "Not Published";
#else
                infoApp.ApplicationName = Properties.Settings.Default.ApplicationName;
                infoApp.Version = Properties.Settings.Default.Version;
#endif
                return Ok(new ResponseApi<InfoApp>(HttpStatusCode.OK, "Desktop app version", infoApp));

            }
            catch (Exception ex)
            {
                return CustomErrorStatusCode(ex);
            }
        }

        /// <summary>
        /// Obtiene un certificado digital desde el almacén de certificados del usuario. 
        /// Se utiliza para verificar el acceso a los recursos locales del usuario.
        /// </summary>
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            try
            {
                X509Certificate2 certificate = new Signer(CertificateUtils.SelectCertificate()).Certificate;
                return Ok(new ResponseApi<string>(HttpStatusCode.OK, "Certificate Thumbprint", certificate.Thumbprint));
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
        [Route("Ping")]
        public IActionResult Ping()
        {
            return StatusCode((int)HttpStatusCode.OK, true);
        }

        /// <summary>
        /// Recibe un archivo PDF/A codificado en Base64, lo firma digitalmente, y retorna el PDF firmado en Base64.
        /// Valida que el archivo siga el estándar PDF/A.
        /// </summary>
        /// <param name="model">El modelo que contiene el PDF a firmar y los detalles de la firma.</param>
        /// <returns>El PDF/A firmado en formato Base64.</returns>
        [HttpPost]
        [Route("Pdf/Signature/Sign/Base64")]
        public IActionResult PdfSignatureSign([FromBody] PdfToSign model)
        {
            try
            {
                if (model.IsNull())
                    throw new CustomException(CustomException.ErrorsEnum.ModelNull);

                if (string.IsNullOrWhiteSpace(model.PdfBase64))
                    throw new CustomException(CustomException.ErrorsEnum.PdfNull);

                if (string.IsNullOrEmpty(model.Reason))
                    model.Reason = Reason;

                byte[] data = Convert.FromBase64String(model.PdfBase64);
                if (!ValidatorForRestful.IsValidPDFA(new MemoryStream(data)))
                    throw new CustomException(CustomException.ErrorsEnum.PdfInvalido);

                X509Certificate2 certificate = new Signer(CertificateUtils.SelectCertificate()).Certificate;
                if (!ValidatorForRestful.IsValidX509Certificate2(certificate))
                    throw new CustomException(CustomException.ErrorsEnum.InvalidCert);

                var signedPdfBase64 = PdfSigner.Sign(new MemoryStream(data), certificate, Reason, Country);

                ProcessDataResultForPdf result = new ProcessDataResultForPdf
                {
                    Data = new List<string> { signedPdfBase64 },
                    Code = ((int)StatusSignProcess.Good).ToString(),
                };

                return Ok(new ResponseApi<ProcessDataResultForPdf>(HttpStatusCode.OK, "Firmas", result));
            }
            catch (Exception ex)
            {
                return CustomErrorStatusCode(ex);
            }
        }

        /// <summary>
        /// Realiza una firma electrónica única sobre un archivo XML, con opción de utilizar la verificación mediante OCSP.
        /// </summary>
        /// <param name="model">El modelo que contiene los datos del XML a firmar.</param>
        /// <param name="usarComprobaciónPorOcsp">Indica si se debe utilizar verificación por OCSP.</param>
        /// <returns>Resultado de la firma electrónica.</returns>
        [HttpPost]
        [Route("Single/Oscp/{usarComprobaciónPorOcsp}")]
        public IActionResult Single([FromBody] XmlToSign model, bool usarComprobaciónPorOcsp)
        {
            try
            {
                ProcessDataResultForXml result = SignatureSelector.Single(model, usarComprobaciónPorOcsp);
                return Ok(new ResponseApi<ProcessDataResultForXml>(HttpStatusCode.OK, usarComprobaciónPorOcsp ? "Firmas Digitales" : "Firmas Electronicas", result));
            }
            catch (Exception ex)
            {
                return CustomErrorStatusCode(ex);
            }
        }

        /// <summary>
        /// Realiza una firma electrónica en masa sobre múltiples archivos XML, con opción de utilizar la verificación mediante OCSP.
        /// </summary>
        /// <param name="model">Lista de modelos XML a firmar.</param>
        /// <param name="usarComprobaciónPorOcsp">Indica si se debe utilizar verificación por OCSP.</param>
        /// <returns>Resultado de la firma electrónica en masa.</returns>
        [HttpPost]
        [Route("Bulk/Oscp/{usarComprobaciónPorOcsp}")]
        public IActionResult Bulk([FromBody] List<XmlToSign> model, bool usarComprobaciónPorOcsp)
        {
            try
            {
                ProcessDataResultForXml result = SignatureSelector.Bulk(model, usarComprobaciónPorOcsp);
                return Ok(new ResponseApi<ProcessDataResultForXml>(HttpStatusCode.OK, usarComprobaciónPorOcsp ? "Firmas Digitales" : "Firmas Electronicas", result));
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
        public IActionResult ExistSignatures([FromBody] XmlToSign model)
        {
            try
            {
                JObject signatureList = Verification.ExistOneOrMoreSignatures(model);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, "Firmas", signatureList));
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
        public IActionResult CheckMultiSignatures([FromBody] XmlToSign model)
        {
            try
            {
                List<JObject> signatureList = Verification.CheckSignatures(model);
                return Ok(new ResponseApi<List<JObject>>(HttpStatusCode.OK, "Firmas", signatureList));
            }
            catch (Exception ex)
            {
                return CustomErrorStatusCode(ex);
            }
        }

        private IActionResult CustomErrorStatusCode(Exception e)
        {
            if (e is CustomException customEx)
            {
                var errorCode = customEx.errorCode;
                var message = customEx.Message;

                if (errorCode == 403)
                {
                    return StatusCode((int)HttpStatusCode.Forbidden,
                        new ResponseApi<object>(
                            HttpStatusCode.Forbidden,
                            "ha ocurrido un error",
                            null,
                            customEx.InnerException != null ? customEx.InnerException.Message : message,
                            errorCode,
                            ex: e.ToString()
                        ));
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed,
                        new ResponseApi<object>(
                            HttpStatusCode.PreconditionFailed,
                            "ha ocurrido un error",
                            null,
                            customEx.InnerException != null ? customEx.InnerException.Message : message,
                            errorCode,
                            ex: e.ToString()
                        ));
                }
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    new ResponseApi<object>(
                        HttpStatusCode.InternalServerError,
                        "ha ocurrido un error",
                        null,
                        e.InnerException != null ? e.InnerException.Message : e.Message,
                        -1, // Código de error genérico
                        ex: e.ToString()
                    ));
            }
        }
    }
}