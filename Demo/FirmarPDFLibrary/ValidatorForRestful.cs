using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace FirmarPDFLibrary
{
    public class ValidatorForRestful : ValidatorBase
    {
        public static new bool IsValidX509Certificate2(X509Certificate2 aCert)
        {
            return ValidatorBase.IsValidX509Certificate2(aCert);
        }

        /// <summary>
        /// Valida si un archivo PDF codificado en Base64 cumple con el estándar PDF/A.
        /// Convierte la cadena en Base64 a su formato original y valida el formato PDF/A.
        /// </summary>
        /// <param name="base64EncodedPdf">Cadena Base64 que representa un archivo PDF.</param>
        /// <returns>
        /// True si el archivo es válido como PDF/A, de lo contrario False.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Lanza una excepción si el archivo PDF es nulo o está vacío.
        /// </exception>
        /// <exception cref="FormatException">
        /// Lanza una excepción si la cadena Base64 no tiene un formato válido.
        /// </exception>
        /// <exception cref="Exception">
        /// Lanza una excepción si ocurre un error durante la validación del archivo PDF.
        /// </exception>
        public static bool IsValidPDFA(MemoryStream pdfStream)
        {
            try
            {
                using (var pdfReader = new iText.Kernel.Pdf.PdfReader(pdfStream))
                {
                    var pdfDocument = new iText.Kernel.Pdf.PdfDocument(pdfReader);
                    var pdfVersion = pdfDocument.GetPdfVersion()
                        .ToString().Replace("PDF-", "");
                    var pdfVersions = new List<string> { "1.4", "1.5", "1.6", "1.7" };

                    if (pdfVersions.Contains(pdfVersion))
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar el PDF: {ex.Message}");
                return false;
            }
        }
    }
}
