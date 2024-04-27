using System;
using System.Collections.Generic;

namespace Demo.Results
{
    public class CustomException : Exception
    {
        static Dictionary<ErrorsEnum, string> _errors;
        public static Dictionary<ErrorsEnum, string> Errors{ get => _errors; }
        public enum ErrorsEnum : int
        {
            NoCert = -1,
            InvalidCert = -2,

            ServiceNull = 1,
            ModelNull = 2,
            TypeSignatureNull = 3,
            ListOfModelNullorEmpty = 4
        };
        
        public int errorCode = 0;
        public new string Message;

        public int ErrorList { get => errorCode; }

        public CustomException(ErrorsEnum errorsEnum)
        {
            //declaracion de tipos de error
            _errors = new Dictionary<ErrorsEnum, string>
            {
                { ErrorsEnum.ServiceNull, "Service is null" },
                { ErrorsEnum.ModelNull, "Model is null" },
                { ErrorsEnum.ListOfModelNullorEmpty, "List is null or empty" },
                { ErrorsEnum.TypeSignatureNull, "TypeSignature is null" },

                { ErrorsEnum.NoCert, "No se selecciono certificado" },
                { ErrorsEnum.InvalidCert, "Certificado no valido " }
            };

            //se asigna el mensaje
            Message = _errors[errorsEnum];
            errorCode = (int)errorsEnum;

            siNosOlvidamosAlgunaDescripcion();
        }

        private void siNosOlvidamosAlgunaDescripcion()
        {
            if (Enum.GetNames(typeof(ErrorsEnum)).Length != _errors.Count)
            {
                foreach (ErrorsEnum errorName in Enum.GetValues(typeof(ErrorsEnum)))
                {
                    if (!_errors.ContainsKey(errorName))
                    {
                        _errors.Add(errorName, errorName.ToString());
                    }
                }
            }
        }
    }
}
