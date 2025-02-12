using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace FirmarPDFLibrary;

public abstract class ValidatorForRestful : ValidatorBase
{
    public new static bool IsValidX509Certificate2(X509Certificate2 aCert)
    {
        return ValidatorBase.IsValidX509Certificate2(aCert);
    }
    
    /// <summary>
    /// Verifica si el archivo PDF proporcionado cumple con una versión aceptada del estándar PDF/A.
    /// </summary>
    /// <param name="pdfStream">Un flujo de memoria que contiene el archivo PDF para verificar.</param>
    /// <returns>
    /// <c>true</c> si el PDF cumple con una versión aceptada del estándar PDF/A (1.4, 1.5, 1.6 o 1.7);
    /// de lo contrario, <c>false</c>.
    /// </returns>
    /// <remarks>
    /// Este método intenta abrir el PDF con `iText.Kernel.Pdf.PdfReader` para verificar su versión.
    /// Si el archivo PDF no cumple con las versiones aceptadas o no se puede cargar, se registra el error y se devuelve <c>false</c>.
    /// </remarks>
    /// <example>
    /// <code>
    /// using var pdfStream = new MemoryStream(File.ReadAllBytes("documento.pdf"));
    /// bool esValido = ValidatorForRestful.IsValidPDFA(pdfStream);
    /// </code>
    /// </example>
    public static bool IsValidPDFA(MemoryStream pdfStream)
    {
        try
        {
            using var pdfReader = new iText.Kernel.Pdf.PdfReader(pdfStream);
            var pdfDocument = new iText.Kernel.Pdf.PdfDocument(pdfReader);
            var pdfVersion = pdfDocument.GetPdfVersion()
                .ToString().Replace("PDF-", "");
            var pdfVersions = new List<string> { "1.4", "1.5", "1.6", "1.7" };

            if (pdfVersions.Contains(pdfVersion))
                return true;
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al cargar el PDF: {ex.Message}");
            return false;
        }
    }
}
