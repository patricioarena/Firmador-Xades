using Demo.Models;
using Helper.Model;
using System.Collections.Generic;
using System.Web.Http;
using System.Xml;

namespace Demo.Handlers
{
    public interface IDecisionHandler
    {
        ProcessResult CoreDecision(string typeSignature, XmlToSign model, bool usarComprobaciónPorOCSP);
        ProcessResult BulkCoreDecision(string typeSignature, List<XmlToSign> list, bool usarComprobaciónPorOCSP);
    }
}