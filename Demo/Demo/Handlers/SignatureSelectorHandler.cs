using Demo.Extensions;
using Demo.Models;
using Helper.Model;
using Helper.Results;
using System.Collections.Generic;

namespace Demo.Handlers
{
    public class SignatureSelectorHandler(ICoreHandler coreHandler) : ISignatureSelectorHandler
    {
        private ICoreHandler Core { get; } = coreHandler;

        public ProcessDataResultForXml Single(XmlToSign model, bool usarComprobaciónPorOcsp)
        {
            if (model.IsNull())
                throw new CustomException(CustomException.ErrorsEnum.ModelNull);

            int code = Core.SingleSignature(model, usarComprobaciónPorOcsp, out List<string> listString);

            return new ProcessDataResultForXml()
            {
                Data = listString,
                Code = code.ToString()
            };
        }

        public ProcessDataResultForXml Bulk(List<XmlToSign> listXmlToSign, bool usarComprobaciónPorOcsp)
        {
            if (listXmlToSign.IsNotEmpty())
                throw new CustomException(CustomException.ErrorsEnum.ModelNull);

            int code = Core.BulkSignature(listXmlToSign, usarComprobaciónPorOcsp, out List<string> listString);

            return new ProcessDataResultForXml()
            {
                Data = listString,
                Code = code.ToString()
            };
        }
    }
}
