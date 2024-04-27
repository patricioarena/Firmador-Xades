using Demo.Enums;
using Demo.Results;
using FirmaXadesNet;
using FirmaXadesNet.Crypto;
using FirmaXadesNet.Signature;
using FirmaXadesNet.Signature.Parameters;
using FirmaXadesNet.Utils;
using Helper.Model;
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
        public SignaturePolicyInfo ObtenerPolitica()
        {
            return new SignaturePolicyInfo()
            {
                PolicyIdentifier = Properties.Settings.Default.SignaturePolicyInfoPolicyIdentifier,
                PolicyHash = Properties.Settings.Default.SignaturePolicyInfoPolicyHash,
                PolicyUri = Properties.Settings.Default.SignaturePolicyInfoPolicyUri
            };
        }

        public SignatureParameters ObtenerParametrosFirma()
        {
            SignatureParameters parametros = new SignatureParameters();
            parametros.SignatureMethod = SignatureMethod.RSAwithSHA256;
            parametros.SigningDate = DateTime.Now;

            var sc = new SignatureCommitment(SignatureCommitmentType.ProofOfOrigin);
            parametros.SignatureCommitments.Add(sc);

            return parametros;
        }

        public bool VerifyX509Certificate(X509Certificate2 aCert)
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

        public int SignatureHandler(ObjetoModel model, IService service, bool usarComprobaciónPorOCSP, out List<string> listString)
        {
            try
            {
                listString = null;

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

        public int BulkSignatureHandler(List<ObjetoModel> list, IService service, bool usarComprobaciónPorOCSP, out List<string> listString)
        {
            try
            {
                listString = null;

                if (service == null)
                    throw new CustomException(CustomException.ErrorsEnum.ServiceNull);

                if (list.Count == 0)
                    throw new CustomException(CustomException.ErrorsEnum.ListOfModelNullorEmpty);

                X509Certificate2 aCert = CertUtil.SelectCertificate();

                if (aCert == null)
                    return (int)CustomException.ErrorsEnum.NoCert;

                if (usarComprobaciónPorOCSP)
                    comprobaciónPorOCSP(aCert);

                if (VerifyX509Certificate(aCert))
                {
                    var outXmlElement = new List<XmlElement>();
                    foreach (var model in list)
                    {
                        SignatureParameters parametros = ObtenerParametrosFirma();
                        SignatureDocument _signatureDocument;

                        parametros.SignaturePolicyInfo = ObtenerPolitica();
                        parametros.SignaturePackaging = SignaturePackaging.ENVELOPED;
                        parametros.DataFormat = new DataFormat();
                        parametros.DataFormat.MimeType = MimeTypeInfo.GetMimeType(model.Extension);

                        byte[] bytes = Encoding.ASCII.GetBytes(model.Archivo);

                        _signatureDocument = SignXmlDocumentAndReturnSignatureDocument(service, outXmlElement, parametros, bytes, aCert);
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

        public static SignatureDocument SignXmlDocumentAndReturnSignatureDocument(IService service, List<XmlElement> OutXmlElement, SignatureParameters parametros, byte[] bytes, X509Certificate2 aCert)
        {
            SignatureDocument _signatureDocument;
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
            OutXmlElement.Add(xmlDocument.DocumentElement);
            return _signatureDocument;
        }

        public int comprobaciónPorOCSP(X509Certificate2 aCert)
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
