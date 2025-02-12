using Demo.Models;
using Helper.Model;
using System.Collections.Generic;

namespace Demo.Handlers
{
    public interface ISignatureSelectorHandler
    {
        ProcessDataResultForXml Single(XmlToSign model, bool usarComprobaciónPorOcsp);
        ProcessDataResultForXml Bulk(List<XmlToSign> list, bool usarComprobaciónPorOcsp);
    }
}