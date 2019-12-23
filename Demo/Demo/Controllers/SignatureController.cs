using Demo.Model;
using Demo.Results;
using Demo.Services;
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

namespace Demo.Controllers
{
    [RoutePrefix("api/Signature")]
    public class SignatureController : ApiController
    {
        #region private methods
        private SignaturePolicyInfo ObtenerPolitica()
        {
            SignaturePolicyInfo spi = new SignaturePolicyInfo();

            spi.PolicyIdentifier = "urn:oid:2.16.724.1.3.1.1.2.1.8";
            spi.PolicyHash = "V8lVVNGDCPen6VELRD1Ja8HARFk=";
            spi.PolicyUri = "http://administracionelectronica.gob.es/es/ctt/politicafirma/politica_firma_AGE_v1_8.pdf";

            return spi;
        }

        private SignatureParameters ObtenerParametrosFirma()
        {
            SignatureParameters parametros = new SignatureParameters();
            parametros.SignatureMethod = SignatureMethod.RSAwithSHA512;
            parametros.SigningDate = DateTime.Now;

            var sc = new SignatureCommitment(SignatureCommitmentType.ProofOfOrigin);
            parametros.SignatureCommitments.Add(sc);

            return parametros;
        }

        private bool Verify(X509Certificate2 aCert)
        {
            try
            {
                aCert.Verify().Equals(true);
                var key = (RSACryptoServiceProvider)aCert.PrivateKey;
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        #endregion


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
        /// Firma el documento recibido (Firma Electronica)
        /// </summary>
        [HttpPost]
        [Route("Electronic/Signature/{typeSignature}")]
        public IHttpActionResult Electronic(string typeSignature, [FromBody] ObjetoModel model)
        {
            try
            {
                IService service = null;
                //Xades Original :: 1
                if (typeSignature.Equals("1"))
                {
                    service = new FirmaXadesNet.XadesService();
                }
                //Xades Custom :: 2
                else
                {
                    service = new Custom.FirmaXadesNet.XadesService2();
                }

                SignatureParameters parametros = ObtenerParametrosFirma();
                SignatureDocument _signatureDocument;

                parametros.SignaturePolicyInfo = ObtenerPolitica();
                parametros.SignaturePackaging = SignaturePackaging.ENVELOPED;
                parametros.DataFormat = new DataFormat();
                parametros.DataFormat.MimeType = MimeTypeInfo.GetMimeType(model.Extension);

                byte[] bytes = Encoding.ASCII.GetBytes(model.Archivo);

                X509Certificate2 aCert = CertUtil.SelectCertificate();

                if (aCert == null)
                    return Content(HttpStatusCode.OK, -1); // No se selecciono certificado

                else if (this.Verify(aCert).Equals(true)) // Certificado tiene una clave privada, sirve para firmar
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
                    return Content(HttpStatusCode.OK, xmlDocument.DocumentElement, Configuration.Formatters.XmlFormatter);
                }
                return Content(HttpStatusCode.OK, -2); // Certificado no valido
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
                IService service = null;
                //Xades Original :: 1
                if (typeSignature.Equals("1"))
                {
                    service = new FirmaXadesNet.XadesService();
                }
                //Xades Custom :: 2
                else
                {
                    service = new Custom.FirmaXadesNet.XadesService2();
                }

                SignatureParameters parametros = ObtenerParametrosFirma();
                SignatureDocument _signatureDocument;

                parametros.SignaturePolicyInfo = ObtenerPolitica();
                parametros.SignaturePackaging = SignaturePackaging.ENVELOPED;
                parametros.DataFormat = new DataFormat();
                parametros.DataFormat.MimeType = MimeTypeInfo.GetMimeType(model.Extension);

                byte[] bytes = Encoding.ASCII.GetBytes(model.Archivo);

                X509Certificate2 aCert = CertUtil.SelectCertificate();

                if (aCert == null)
                    return Content(HttpStatusCode.OK, -1); // No se selecciono certificado

                Services.OcspClient client = new Services.OcspClient();
                Services.CertificateStatus resp = client.Validate_Certificate_Using_OCSP_Protocol(aCert);
                JObject T = client.x509ChainVerify(aCert);

                if(T.Count > 0 || resp != Services.CertificateStatus.Good)
                    return Content(HttpStatusCode.OK, -2); // Certificado no valido
                    //return Ok(new ResponseApi<JObject>(HttpStatusCode.OK, "Chain Error", T));

                else if (this.Verify(aCert).Equals(true)) // Certificado tiene una clave privada, sirve para firmar
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
                    _signatureDocument.Save("C:\\Users\\parena\\Desktop\\objecto_Firmado.xml"); // Guardar automaticamente en el escritorio
                    XmlDocument xmlDocument = _signatureDocument.Document;
                    return Content(HttpStatusCode.OK, xmlDocument.DocumentElement, Configuration.Formatters.XmlFormatter);
                }
                return Content(HttpStatusCode.OK, -2); // Certificado no valido
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
        public IHttpActionResult Verify1([FromBody] ObjetoModel model)
        {
            try
            {
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
        /// Xades Custom :: 2
        [HttpPost]
        [Route("Verify/2")]
        public IHttpActionResult Verify2([FromBody] ObjetoModel model)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(model.Archivo);
                byte[] bytes = Encoding.ASCII.GetBytes(model.Archivo);
                List<JObject> SignatureList = new List<JObject>();

                //Servicio correspondiente a la libreria que uso y modifique un poco
                Custom.FirmaXadesNet.XadesService2 service = new Custom.FirmaXadesNet.XadesService2();

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

                        JObject jObject = new JObject();
                        jObject.Add("Subject", x509Certificate2.Subject);
                        jObject.Add("IsValid", validationResult.IsValid);
                        jObject.Add("Message", validationResult.Message);

                        SignatureList.Add(jObject);

                    }
                    return Ok(new ResponseApi<List<JObject>>(HttpStatusCode.OK, "Firmas", SignatureList));
                }
            }
            catch (System.Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
