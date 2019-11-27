﻿using Demo.Model;
using Demo.Results;
using FirmaXadesNet;
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
                return Ok(new Signer(CertUtil.SelectCertificate()).Certificate);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Firma el documento recibido
        /// </summary>
        [HttpPost]
        [Route("Signature/{typeSignature}")]
        public IHttpActionResult Signature(string typeSignature, [FromBody] ObjetoModel model)
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
                {
                    return Content(HttpStatusCode.OK, -1); // No se selecciono certificado
                }

                else if (this.Verify(aCert).Equals(true))
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
        /// Verifica si existe una firma
        /// </summary>
        /// Xades Original :: 2
        [HttpPost]
        [Route("Verify/2")]
        public IHttpActionResult Verify2([FromBody] ObjetoModel model)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(model.Archivo);
                byte[] bytes = Encoding.ASCII.GetBytes(model.Archivo);
                List<JObject> SignatureList = new List<JObject>();
                Custom.FirmaXadesNet.XadesService2 service = new Custom.FirmaXadesNet.XadesService2();

                //Para hacer prubas usando un documento local en el escritorio 
                //string pathFile = "C:\\Users\\parena\\Desktop\\Document.xml";
                //using (FileStream stream = new FileStream(pathFile, FileMode.Open))
                using (Stream stream = new MemoryStream(bytes))
                {
                    SignatureDocument[] signatureDocument = service.Load(stream);
                    foreach (var aFirma in signatureDocument)
                    {
                        FirmaXadesNet.Validation.ValidationResult validation = service.Validate(aFirma);
                        string Subject = aFirma.XadesSignature.GetSigningCertificate().Subject;

                        JObject jObject = new JObject();
                        jObject.Add("Subject", Subject);
                        jObject.Add("IsValid", validation.IsValid);
                        jObject.Add("Message", validation.Message);


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

        #region Extra
        /// <summary>
        /// Verifica si existe una firma metodo extraido del proyecto TeViewer
        /// </summary>
        /**
        public void TeViewerVerify(XmlDocument doc)
        {
            System.Security.Cryptography.Xml.SignedXml signedXml = new System.Security.Cryptography.Xml.SignedXml(doc);
            XmlNodeList nodeList = doc.GetElementsByTagName("ds:Signature");
            if (nodeList.Count > 0)
            {
                try
                {
                    var temp = (XmlElement)nodeList[0];
                    signedXml.LoadXml(temp);
                    try
                    {

                        if (signedXml.CheckSignature())
                        {
                            //agregamos un nodo con el resultado del chequeo para el display
                            XmlNode n = doc.SelectSingleNode("//titulo");
                            XmlElement e = doc.CreateElement("firma_valida");
                            e.InnerText = "1";
                            n.AppendChild(e);
                        }
                        else
                        {

                            XmlNode n = doc.SelectSingleNode("//titulo");
                            XmlElement e = doc.CreateElement("firma_valida");
                            e.InnerText = "0";
                            e.InnerText = "FIRMA_INVALIDA";
                            n.AppendChild(e);

                        }

                        string xml = doc.InnerXml;
                    }
                    catch (CryptographicException ex)
                    {
                        throw ex;
                    }
                }
                catch (CryptographicException ex)
                {
                    throw ex;
                }
            }
            else
            {
                throw new CryptographicException();
            }
        }
        */
        #endregion Extra
    }
}
