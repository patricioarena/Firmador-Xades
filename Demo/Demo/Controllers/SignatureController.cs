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

namespace Demo.Controllers
{
    [RoutePrefix("api/Signature")]
    public class SignatureController : ApiController
    {
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
                return Ok(ex);
            }
        }

        /// <summary>
        /// Para probar que la aplicacion esta corriendo desde otras aplicaciones que consuman la api de esta misma
        /// </summary>
        [HttpGet]
        [Route("isAlive")]
        public IHttpActionResult IsAlive()
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

        /// <summary>
        /// Firma el documento recibido (Firma Electronica)
        /// </summary>
        [HttpPost]
        [Route("Electronic/Signature/{typeSignature}")]
        public IHttpActionResult Electronic(string typeSignature, [FromBody] ObjetoModel model)
        {
            try
            {
                return CoreDecision(typeSignature, model, false);
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        /// <summary>
        /// Firma el documento recibido (Firma Digital)
        /// </summary>
        [HttpPost]
        [Route("Digital/Signature/{typeSignature}")]
        public IHttpActionResult Digital(string typeSignature, [FromBody] ObjetoModel model)
        {
            try
            {
                return CoreDecision(typeSignature, model, true);
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
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
                JObject SignatureList = ExistOneOrMoreSignatures(model);
                return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, "Firmas", SignatureList));
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
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
                List<JObject> SignatureList = CheckSignatures(model);
                return Ok(new ResponseApi<List<JObject>>(HttpStatusCode.OK, "Firmas", SignatureList));
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private IHttpActionResult CoreDecision(string typeSignature, ObjetoModel model, bool usarComprobaciónPorOCSP)
        {
            if (String.IsNullOrEmpty(typeSignature))
                throw new CustomException(CustomException.ErrorsEnum.TypeSignatureNull);

            if (model == null)
                throw new CustomException(CustomException.ErrorsEnum.ModelNull);

            IService service = null;
            TypeService key = (TypeService)Int32.Parse(typeSignature);

            switch (key)
            {
                case TypeService.Original:
                    service = new FirmaXadesNet.XadesService();
                    break;
                case TypeService.CIFE:
                    service = new Custom.FirmaXadesNet.XadesService_CIFE();
                    break;
                default:
                    break;
            }

            int code = SignatureHandler(model, service, usarComprobaciónPorOCSP, out var xmlElement);
            if (code == ((int)StatusSignProcess.Good))
                return Content(HttpStatusCode.OK, xmlElement, Configuration.Formatters.XmlFormatter);
            return Content(HttpStatusCode.OK, code);

        }

        private List<JObject> CheckSignatures(ObjetoModel model)
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(model.Archivo);
            byte[] bytes = Encoding.ASCII.GetBytes(model.Archivo);
            List<JObject> SignatureList = new List<JObject>();

            //Servicio correspondiente a la libreria que uso y modifique un poco
            Custom.FirmaXadesNet.XadesService_CIFE service = new Custom.FirmaXadesNet.XadesService_CIFE();

            using (Stream stream = new MemoryStream(bytes))
            {
                // Esto es propio de la libreia que uso crea un arreglo de documentos firmados [[documento,firma 1]
                // [documento,firma 2]] 
                SignatureDocument[] signatureDocument = service.Load(stream);

                //Recorro las firmas de atras para adelante
                for (int index = signatureDocument.Length - 1; index >= 0; index--)
                {

                    VerifyMultiSignature verifyMultiSignature = new VerifyMultiSignature();

                    var aFirma = signatureDocument[index];
                    var x509Certificate2 = aFirma.XadesSignature.GetSigningCertificate(); //Obtener certificado del documento firmado
                    var validationResult = verifyMultiSignature.MatchesSignature(doc, x509Certificate2, index);

                    JObject jObject = new JObject
                    {
                        { "Subject", x509Certificate2.Subject },
                        { "IsValid", validationResult.IsValid },
                        { "Message", validationResult.Message }
                    };

                    SignatureList.Add(jObject);

                }
                return SignatureList;
            }
        }

        private SignaturePolicyInfo ObtenerPolitica()
        {
            return new SignaturePolicyInfo()
            {
                PolicyIdentifier = Properties.Settings.Default.SignaturePolicyInfoPolicyIdentifier,
                PolicyHash = Properties.Settings.Default.SignaturePolicyInfoPolicyHash,
                PolicyUri = Properties.Settings.Default.SignaturePolicyInfoPolicyUri
            };
        }

        private SignatureParameters ObtenerParametrosFirma()
        {
            SignatureParameters parametros = new SignatureParameters();
            parametros.SignatureMethod = SignatureMethod.RSAwithSHA256;
            parametros.SigningDate = DateTime.Now;

            var sc = new SignatureCommitment(SignatureCommitmentType.ProofOfOrigin);
            parametros.SignatureCommitments.Add(sc);

            return parametros;
        }

        private bool VerifyX509Certificate(X509Certificate2 aCert)
        {
            try
            {
                if (!aCert.HasPrivateKey)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private JObject ExistOneOrMoreSignatures(ObjetoModel model)
        {
            try
            {
                if (model == null)
                    throw new CustomException(CustomException.ErrorsEnum.ModelNull);

                byte[] bytes = Encoding.ASCII.GetBytes(model.Archivo);
                FirmaXadesNet.XadesService service = new FirmaXadesNet.XadesService();
                JObject SignatureList = new JObject();

                //Para hacer prubas usando un documento local en el escritorio 
                //string pathFile = "C:\\Users\\parena\\Desktop\\Document.xml";
                //using (FileStream stream = new FileStream(pathFile, FileMode.Open))
                using (Stream stream = new MemoryStream(bytes))
                {
                    SignatureDocument[] signatureDocument = service.Load(stream);
                    foreach (var aFirma in signatureDocument)
                    {
                        string Subject = aFirma.XadesSignature.GetSigningCertificate().Subject;
                        DateTime SigningTime = aFirma.XadesSignature.XadesObject.QualifyingProperties.SignedProperties.SignedSignatureProperties.SigningTime;
                        JProperty Signature = new JProperty(Subject, SigningTime.ToString());
                        SignatureList.Add(Signature);
                    }
                }
                return SignatureList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int SignatureHandler(ObjetoModel model, IService service, bool usarComprobaciónPorOCSP, out XmlElement OutXmlElement)
        {
            try
            {
                OutXmlElement = null;

                if (service == null)
                    throw new CustomException(CustomException.ErrorsEnum.ServiceNull);

                if (model == null)
                    throw new CustomException(CustomException.ErrorsEnum.ModelNull);

                SignatureParameters parametros = ObtenerParametrosFirma();
                SignatureDocument _signatureDocument;

                parametros.SignaturePolicyInfo = ObtenerPolitica();
                parametros.SignaturePackaging = SignaturePackaging.ENVELOPED;
                parametros.DataFormat = new DataFormat();
                parametros.DataFormat.MimeType = MimeTypeInfo.GetMimeType(model.Extension);

                byte[] bytes = Encoding.ASCII.GetBytes(model.Archivo);

                X509Certificate2 aCert = CertUtil.SelectCertificate();

                if (aCert == null)
                    return (int)CustomException.ErrorsEnum.NoCert;

                if (usarComprobaciónPorOCSP)
                    comprobaciónPorOCSP(aCert);

                if (VerifyX509Certificate(aCert)) // Certificado tiene una clave privada, sirve para firmar
                {
                    using (parametros.Signer = new Signer(aCert))
                    {
                        if (parametros.SignaturePackaging != SignaturePackaging.EXTERNALLY_DETACHED)
                        {
                            using (Stream stream = new MemoryStream(bytes))
                            {
                                _signatureDocument = service.Sign(stream, parametros);
                            }
                        }
                        else
                        {
                            _signatureDocument = service.Sign(null, parametros);
                        }
                    }
                    //_signatureDocument.Save("C:\\Users\\parena\\Desktop\\objecto_Firmado.xml"); // Guardar automaticamente en el escritorio
                    XmlDocument xmlDocument = _signatureDocument.Document;
                    OutXmlElement = xmlDocument.DocumentElement;
                    return (int)StatusSignProcess.Good;
                }

                return (int)CustomException.ErrorsEnum.InvalidCert;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int comprobaciónPorOCSP(X509Certificate2 aCert)
        {
            Helper.Services.OcspClient client = new Helper.Services.OcspClient();
            Helper.Services.CertificateStatus resp = client.Validate_Certificate_Using_OCSP_Protocol(aCert);
            JObject T = client.x509ChainVerify(aCert);

            if (T.Count > 0 || resp != Helper.Services.CertificateStatus.Good)
                return (int)CustomException.ErrorsEnum.InvalidCert;
            return 0;
        }
    }
}
