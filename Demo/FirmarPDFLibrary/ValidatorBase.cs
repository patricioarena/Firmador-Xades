using System;
using System.Security.Cryptography.X509Certificates;

namespace FirmarPDFLibrary
{
    public class ValidatorBase
    {
        public static bool IsValidX509Certificate2(X509Certificate2 aCert)
        {
            try
            {
                if (!aCert.HasPrivateKey)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}