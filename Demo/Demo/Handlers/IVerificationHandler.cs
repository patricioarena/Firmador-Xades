using Helper.Model;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Demo.Handlers
{
    public interface IVerificationHandler
    {
        List<JObject> CheckSignatures(ObjetoModel model);
        JObject ExistOneOrMoreSignatures(ObjetoModel model);
    }
}