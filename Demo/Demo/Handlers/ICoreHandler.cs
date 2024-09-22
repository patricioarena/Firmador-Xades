using FirmaXadesNet;
using FirmaXadesNet.Signature.Parameters;
using Helper.Model;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Demo.Handlers
{
    public interface ICoreHandler
    {
        int BulkSignatureHandler(List<XmlToSign> list, IService service, bool usarComprobaciónPorOCSP, out List<string> listString);
        int comprobaciónPorOCSP(X509Certificate2 aCert);
        SignatureParameters ObtenerParametrosFirma();
        SignaturePolicyInfo ObtenerPolitica();
        int SignatureHandler(XmlToSign model, IService service, bool usarComprobaciónPorOCSP, out List<string> listString);
        bool VerifyX509Certificate(X509Certificate2 aCert);
    }
}