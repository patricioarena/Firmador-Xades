using Demo.Results;
using FirmaXadesNet.Signature;
using Helper.Model;
using Helper.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Demo.Handlers
{
    public class VerificationHandler : IVerificationHandler
    {
        public JObject ExistOneOrMoreSignatures(ObjetoModel model)
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

        public List<JObject> CheckSignatures(ObjetoModel model)
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
    }
}
