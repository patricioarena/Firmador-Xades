using Demo.Enums;
using Demo.Models;
using FirmaXadesNet;
using Helper.Model;
using Helper.Results;
using System;
using System.Collections.Generic;

namespace Demo.Handlers
{
    public class DecisionHandler : IDecisionHandler
    {
        ICoreHandler Core { get; set; }

        public DecisionHandler(ICoreHandler coreHandler)
        {
            this.Core = coreHandler;
        }

        public ProcessResult CoreDecision(string typeSignature, XmlToSign model, bool usarComprobaciónPorOCSP)
        {
            if (String.IsNullOrEmpty(typeSignature))
                throw new CustomException(CustomException.ErrorsEnum.TypeSignatureNull);

            if (model == null)
                throw new CustomException(CustomException.ErrorsEnum.ModelNull);

            IService service = null;
            TypeService key = (TypeService)Int32.Parse(typeSignature);

            switch (key)
            {
                case TypeService.Original:
                    service = new FirmaXadesNet.XadesService();
                    break;
                case TypeService.CIFE:
                    service = new Custom.FirmaXadesNet.XadesService_CIFE();
                    break;
                default:
                    break;
            }

            int code = this.Core.SignatureHandler(model, service, usarComprobaciónPorOCSP, out List<string> listString);

            return new ProcessResult()
            {
                Data = listString,
                Code = code.ToString()
            };
        }

        public ProcessResult BulkCoreDecision(string typeSignature, List<XmlToSign> list, bool usarComprobaciónPorOCSP)
        {

            if (String.IsNullOrEmpty(typeSignature))
                throw new CustomException(CustomException.ErrorsEnum.TypeSignatureNull);

            if (list == null)
                throw new CustomException(CustomException.ErrorsEnum.ModelNull);

            IService service = null;
            TypeService key = (TypeService)Int32.Parse(typeSignature);

            switch (key)
            {
                case TypeService.Original:
                    service = new FirmaXadesNet.XadesService();
                    break;
                case TypeService.CIFE:
                    service = new Custom.FirmaXadesNet.XadesService_CIFE();
                    break;
                default:
                    break;
            }

            int code = this.Core.BulkSignatureHandler(list, service, usarComprobaciónPorOCSP, out List<string> listString);

            return new ProcessResult()
            {
                Data = listString,
                Code = code.ToString()
            };
        }
    }
}
