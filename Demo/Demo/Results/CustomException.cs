using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Results
{
    public class CustomException : Exception
    {
        static Dictionary<ErrorsEnum, string> _errors;
        public static Dictionary<ErrorsEnum, string> Errors{ get => _errors; }
        public enum ErrorsEnum : int
        {
            operacionNoConcretada = 1,

            Escritos_idEscrito_inexistente = 1001,
            Escritos_Error_validacion_datos = 1002,
            Escritos_Estado_inválido = 1003,
            Escritos_Id_Escrito_No_Retornado = 1004,
            Escritos_Id_Estado_No_Retornado = 1005,
            Escritos_Id_Texto_No_Retornado = 1006,
            Escritos_Estado_No_Retornado = 1007,

            Moderaciones_idModeracion_inexistente = 2001,
            Moderaciones_Estado_inválido = 2002,
            Moderaciones_No_se_puede_cargar_moderador = 2003,
            Moderaciones_Fecha_inválida = 2004,

            Usuarios_Usuario_inválido = 3001,
            Usuario_Sin_Usuario = 3002,

            EscritosTexto_Texto_inválido = 4001,
            EscritosTexto_Titulo_inválido = 4002,

            SQL_Sin_Inicio_de_Sesion = 4060,
            SQL_Sin_Sin_Usuario_En_El_Sistema = 4061,


            Observaciones_Texto_inválido = 5001

        };
        
        public int errorCode = 0;
        public new string Message;

        public int ErrorList { get => errorCode; }

        public CustomException(ErrorsEnum errorsEnum)
        {
            //declaracion de tipos de error
            _errors = new Dictionary<ErrorsEnum, string>();
            _errors.Add(ErrorsEnum.operacionNoConcretada, "Operación no concretada por consistencia en la base");

            _errors.Add(ErrorsEnum.Escritos_idEscrito_inexistente, "Escrito Inexistente");
            _errors.Add(ErrorsEnum.Escritos_Error_validacion_datos, "Error en  validación de datos");
            _errors.Add(ErrorsEnum.Escritos_Estado_inválido, "Estado inválido del escrito");
            _errors.Add(ErrorsEnum.Escritos_Id_Escrito_No_Retornado, "No se pudo Obtener el Id de Escrito, Escrito no incertado");
            _errors.Add(ErrorsEnum.Escritos_Id_Estado_No_Retornado, "No se pudo Obtener el Id del Estado, Escrito no incertado");
            _errors.Add(ErrorsEnum.Escritos_Id_Texto_No_Retornado, "No se pudo Obtener el Id del Texto, Escrito no incertado");
            _errors.Add(ErrorsEnum.Escritos_Estado_No_Retornado, "No se pudo Obtener el Estado de Este Escrito");




            _errors.Add(ErrorsEnum.Moderaciones_idModeracion_inexistente, "Moderacion inexistente");
            _errors.Add(ErrorsEnum.Moderaciones_Estado_inválido, "Estado inválido de Moderacion");
            _errors.Add(ErrorsEnum.Moderaciones_No_se_puede_cargar_moderador, "No se permite agregar un nuevo moderador");
            _errors.Add(ErrorsEnum.Moderaciones_Fecha_inválida, "Fecha inválida de Moderación");

            _errors.Add(ErrorsEnum.Usuarios_Usuario_inválido, "Usuario inválido");
            _errors.Add(ErrorsEnum.Usuario_Sin_Usuario, "No tiene un usuario en este sistema, comunicarse con soporte");


            _errors.Add(ErrorsEnum.EscritosTexto_Texto_inválido, "Texto inválido");
            _errors.Add(ErrorsEnum.EscritosTexto_Titulo_inválido, "Titulo inválido");

            _errors.Add(ErrorsEnum.SQL_Sin_Inicio_de_Sesion, "El usuario no tiene inicio de secion en la base de datos");
            _errors.Add(ErrorsEnum.SQL_Sin_Sin_Usuario_En_El_Sistema, "El usuario no un usuario creado en la base");


            _errors.Add(ErrorsEnum.Observaciones_Texto_inválido, "Texto de observaciones inválido");


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
