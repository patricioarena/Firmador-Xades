using FirmaXadesNetCore.Signature.Parameters;
using Helper.Model;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Demo.Handlers
{
    public interface ICoreHandler
    {
        int BulkSignature(List<XmlToSign> list, bool usarComprobaciónPorOcsp, out List<string> listString);
        int SingleSignature(XmlToSign model, bool usarComprobaciónPorOcsp, out List<string> listString);
    }
}