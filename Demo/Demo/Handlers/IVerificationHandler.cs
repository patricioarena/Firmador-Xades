using Helper.Model;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Demo.Handlers
{
    public interface IVerificationHandler
    {
        List<JObject> CheckSignatures(XmlToSign model);
        JObject ExistOneOrMoreSignatures(XmlToSign model);
    }
}