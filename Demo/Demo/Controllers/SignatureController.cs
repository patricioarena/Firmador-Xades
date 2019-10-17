using Demo.Results;
using FirmaXadesNet;
using FirmaXadesNet.Crypto;
using FirmaXadesNet.Signature;
using FirmaXadesNet.Signature.Parameters;
using FirmaXadesNet.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Services;
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
        #endregion

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
        /// Verifica si existe una o mas firmas
        /// </summary>
        /// <param name="value"></param>
        /// <returns>JSON</returns>
        [HttpPost]
        [Route("Verify")]
        public IHttpActionResult Verify([FromBody] string value)
        {
            try
            {
                string pathFile = "D:\\Desktop\\FacturaFirmada.xml";
                JObject SignatureList = new JObject();
                SignatureDocument[] firmas = null;
                using (FileStream fs = new FileStream(pathFile, FileMode.Open))
                //using (FileStream fs = new FileStream("D:\\Desktop\\Facturae.xml", FileMode.Open))
                {
                    XadesService xadesService = new XadesService();
                    firmas = xadesService.Load(fs);
                    foreach (var afirma in firmas)
                    {
                        string Subject = afirma.XadesSignature.GetSigningCertificate().Subject;
                        DateTime SigningTime = afirma.XadesSignature.XadesObject.QualifyingProperties.SignedProperties.SignedSignatureProperties.SigningTime;
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

        [HttpPost]
        [Route("Signature")]
        public XmlElement Signature([FromBody] JObject byteArray)
        //public XmlElement Signature()
        {
            try
            {
                string pathFile = "C:\\Users\\nperez\\Desktop\\xml_test.xml";
                XadesService xadesService = new XadesService();
                SignatureParameters parametros = ObtenerParametrosFirma();
                SignatureDocument _signatureDocument;

                parametros.SignaturePolicyInfo = ObtenerPolitica();
                parametros.SignaturePackaging = SignaturePackaging.INTERNALLY_DETACHED;
                parametros.DataFormat = new DataFormat();
                parametros.DataFormat.MimeType = MimeTypeInfo.GetMimeType(pathFile);

                /**
                //INTERNNALLY DETACHED: Objeto indeterminado, se selecciona automaticamente el modo de firma. 
                // Cuando la firma y los datos firmados relacionados se incluyen en un elemento primario (solo XML).
                //if (rbInternnallyDetached.Checked)
                //{
                //    parametros.SignaturePolicyInfo = ObtenerPolitica();
                //    parametros.SignaturePackaging = SignaturePackaging.INTERNALLY_DETACHED;
                //    parametros.DataFormat = new DataFormat();
                //    parametros.DataFormat.MimeType = MimeTypeInfo.GetMimeType(txtFichero.Text);
                //}
                
                //DETACHED: Se devuelve la firma sin el documento. Cuando la firma se relaciona con los recursos externos separados de ella.
                //else if (rbExternallyDetached.Checked)
                //{
                //    parametros.SignaturePackaging = SignaturePackaging.EXTERNALLY_DETACHED;
                //    parametros.ExternalContentUri = txtFichero.Text;
                //}

                //ENVELOPED: La firma está integrada dentro del documento XML. Cuando la firma se aplica a los datos que rodean el resto del documento.
                //else if (rbEnveloped.Checked)
                //{
                //    parametros.SignaturePackaging = SignaturePackaging.ENVELOPED;
                //}

                //ENVELOPING: La firma incluye el documento XML codificado en base64.
                //cuando los datos firmados forman un subelemento de la propia firma;
                // - Binarios codificados en Base64;
                // - Incrustar objetos XML;
                // - Incrustar objeto(s) de manifiesto

                //else if (rbEnveloping.Checked)
                //{
                //    parametros.SignaturePackaging = SignaturePackaging.ENVELOPING;
                //}
                */

                byte[] file = File.ReadAllBytes(pathFile);

                using (parametros.Signer = new Signer(CertUtil.SelectCertificate()))
                {
                    if (parametros.SignaturePackaging != SignaturePackaging.EXTERNALLY_DETACHED)
                    {
                        using (Stream stream = new MemoryStream(file))
                        {
                            _signatureDocument = xadesService.Sign(stream, parametros);
                        }
                    }
                    else
                    {
                        _signatureDocument = xadesService.Sign(null, parametros);
                    }
                }
                //_signatureDocument.Save("D:\\Desktop\\test.xml");
                XmlDocument xmlDocument = _signatureDocument.Document;
                return xmlDocument.DocumentElement;
            }
            catch (System.Exception ex)
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlException exception = new XmlException(ex.ToString());
                xmlDocument.LoadXml($"<root><exception>{exception}</exception></root>");
                return xmlDocument.DocumentElement;
            }
        }

        [HttpGet]
        [Route("Document")]
        public XmlElement Document()
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml("<?xml version=\"1.0\"?> \n" +
                    "<books xmlns=\"http://www.contoso.com/books\"> \n" +
                    "  <book genre=\"novel\" ISBN=\"1-861001-57-8\" publicationdate=\"1823-01-28\"> \n" +
                    "    <title>Pride And Prejudice</title> \n" +
                    "    <price>24.95</price> \n" +
                    "  </book> \n" +
                    "  <book genre=\"novel\" ISBN=\"1-861002-30-1\" publicationdate=\"1985-01-01\"> \n" +
                    "    <title>The Handmaid's Tale</title> \n" +
                    "    <price>29.95</price> \n" +
                    "  </book> \n" +
                    "</books>");
            return xmlDocument.DocumentElement;
        }

        [HttpGet]
        [Route("Contenido")]
        public XmlElement GetContenido()
        {
            SignatureDocument[] firmas = null;

            string pathFile = "D:\\Desktop\\estrellaFirmada.xml";
            XmlDocument document = new XmlDocument();
            document.Load(pathFile);


            XmlNode xnList = document.SelectSingleNode("/DOCFIRMA/CONTENT");
 
                Console.WriteLine("Name: {0} ", xnList.InnerText);
 
 

            return document.DocumentElement;
        }
    }
}
