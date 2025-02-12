using Demo.Enums;
using Demo.Properties;
using Demo.Utils;
using FirmaXadesNetCore;
using FirmaXadesNetCore.Crypto;
using FirmaXadesNetCore.Signature;
using FirmaXadesNetCore.Signature.Parameters;
using Helper.Model;
using Helper.Results;
using Helper.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Demo.Handlers
{
    public class CoreHandler : ICoreHandler
    {
        public int SingleSignature(XmlToSign model, bool usarComprobaciónPorOcsp, out List<string> listString)
        {
            try
            {
                listString = null;

                X509Certificate2 aCert = CertificateUtils.SelectCertificate();

                if (aCert == null)
                    return (int)CustomException.ErrorsEnum.NoCert;

                if (usarComprobaciónPorOcsp)
                    VerificationByOcsp(aCert);

                if (VerifyX509Certificate(aCert)) // Certificado tiene una clave privada, sirve para firmar
                {
                    SignatureParameters parameters = GetSignatureParameters();
                    parameters.SignaturePolicyInfo = GetSignaturePolicyInfo();
                    parameters.SignaturePackaging = SignaturePackaging.ENVELOPED;
                    parameters.DataFormat = new DataFormat();
                    parameters.DataFormat.MimeType = MimeTypeInfo.GetMimeType(model.Extension);
                    
                    byte[] bytes = model.XmlFile;

                    SignatureDocument signatureDocument;
                    using (parameters.Signer = new Signer(aCert))
                    {
                        
                        if (parameters.SignaturePackaging != SignaturePackaging.EXTERNALLY_DETACHED)
                        {
                            using (Stream stream = new MemoryStream(bytes))
                            {
                                var service = new XadesService();
                                signatureDocument = service.Sign(stream, parameters);
                            }
                        }
                        else
                        {
                            var service = new XadesService();
                            signatureDocument = service.Sign(null, parameters);
                        }
                    }
                    //_signatureDocument.Save("C:\\Users\\parena\\Desktop\\objecto_Firmado.xml"); // Guardar automaticamente en el escritorio
                    XmlDocument xmlDocument = signatureDocument.Document;

                    var listStringAux = new List<string>
                    {
                        xmlDocument.DocumentElement.OuterXml
                    };
                    listString = listStringAux;
                    return (int)StatusSignProcess.Good;
                }

                return (int)CustomException.ErrorsEnum.InvalidCert;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int BulkSignature(List<XmlToSign> list, bool usarComprobaciónPorOcsp, out List<string> listString)
        {
            try
            {
                listString = null;

                X509Certificate2 aCert = CertificateUtils.SelectCertificate();

                if (aCert == null)
                    return (int)CustomException.ErrorsEnum.NoCert;

                if (usarComprobaciónPorOcsp)
                    VerificationByOcsp(aCert);

                if (VerifyX509Certificate(aCert))
                {
                    var outXmlElement = new List<XmlElement>();
                    foreach (var model in list)
                    {
                        SignatureParameters parameters = GetSignatureParameters();
                        parameters.SignaturePolicyInfo = GetSignaturePolicyInfo();
                        parameters.SignaturePackaging = SignaturePackaging.ENVELOPED;
                        parameters.DataFormat = new DataFormat();
                        parameters.DataFormat.MimeType = MimeTypeInfo.GetMimeType(model.Extension);
                        
                        byte[] bytes = model.XmlFile;

                        SignXmlDocumentAndReturnSignatureDocument(outXmlElement, parameters, bytes, aCert);
                    }

                    var listStringAux = new List<string>();
                    foreach (var xmlElement in outXmlElement)
                    {
                        listStringAux.Add(xmlElement.OuterXml);
                    }
                    listString = listStringAux;
                    return (int)StatusSignProcess.Good;
                }
                return (int)CustomException.ErrorsEnum.InvalidCert;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        private SignaturePolicyInfo GetSignaturePolicyInfo() =>
            new()
            {
                PolicyIdentifier = Settings.Default.SignaturePolicyInfoPolicyIdentifier,
                PolicyHash = Settings.Default.SignaturePolicyInfoPolicyHash,
                PolicyUri = Settings.Default.SignaturePolicyInfoPolicyUri
            };

        private SignatureParameters GetSignatureParameters()
        {
            SignatureParameters parameters = new SignatureParameters();
            parameters.SignatureMethod = SignatureMethod.RSAwithSHA256;
            parameters.SigningDate = DateTime.Now;

            var sc = new SignatureCommitment(SignatureCommitmentType.ProofOfOrigin);
            parameters.SignatureCommitments.Add(sc);

            return parameters;
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

        private static void SignXmlDocumentAndReturnSignatureDocument(List<XmlElement> outXmlElement,
            SignatureParameters parameters, byte[] bytes, X509Certificate2 aCert)
        {
            SignatureDocument signatureDocument;
            using (parameters.Signer = new Signer(aCert))
            {
                if (parameters.SignaturePackaging != SignaturePackaging.EXTERNALLY_DETACHED)
                {
                    using (Stream stream = new MemoryStream(bytes))
                    {
                        var service = new XadesService();
                        signatureDocument = service.Sign(stream, parameters);
                    }
                }
                else
                {
                    var service = new XadesService();
                    signatureDocument = service.Sign(null, parameters);
                }
            }
            //_signatureDocument.Save("C:\\Users\\parena\\Desktop\\objecto_Firmado.xml"); // Guardar automaticamente en el escritorio
            XmlDocument xmlDocument = signatureDocument.Document;
            outXmlElement.Add(xmlDocument.DocumentElement);
        }

        private int VerificationByOcsp(X509Certificate2 aCert)
        {
            OcspClient client = new OcspClient();
            CertificateStatus resp = client.Validate_Certificate_Using_OCSP_Protocol(aCert);
            JObject T = client.x509ChainVerify(aCert);

            if (T.Count > 0 || resp != CertificateStatus.Good)
                return (int)CustomException.ErrorsEnum.InvalidCert;
            return 0;
        }
    }
}
