using iTextSharp.text.pdf;
using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace FirmarPDFLibrary
{
    public class ValidatorForDesktop : ValidatorBase
    {
        public static new bool IsValidX509Certificate2(X509Certificate2 aCert)
        {
            return ValidatorBase.IsValidX509Certificate2(aCert);
        }
        /// <summary>
        /// Valida si un archivo PDF cumple con el estándar PDF/A.
        /// Extrae los metadatos del archivo y verifica si contiene las etiquetas
        /// necesarias para ser un PDF/A mediante la validación del formato XML.
        /// </summary>
        /// <param name="source">Ruta del archivo PDF a validar.</param>
        /// <returns>
        /// True si el archivo es válido como PDF/A, de lo contrario False.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Lanza una excepción si la ruta del archivo es nula o vacía.
        /// </exception>
        /// <exception cref="Exception">
        /// Lanza una excepción si ocurre un error durante la validación del archivo PDF.
        /// </exception>
        public static bool IsValidPDFA(string source)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(source))
                    throw new ArgumentException("La ruta del archivo PDF no puede estar vacía o ser nula.", nameof(source));

                PdfReader reader = new PdfReader(source);
                byte[] metadata = reader.Metadata;

                if (metadata == null)
                    return false;

                string pdfInStringFormat = Encoding.Default.GetString(metadata);
                return ValidatePdfAStringFormat(pdfInStringFormat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Valida si una cadena de texto en formato XML contiene las etiquetas
        /// necesarias para cumplir con el estándar PDF/A.
        /// Verifica la presencia de las etiquetas "pdfaid:part" y "pdfaid:conformance",
        /// y que sus valores no estén vacíos.
        /// </summary>
        /// <param name="pdfInStringFormat">Cadena en formato XML que representa el PDF.</param>
        /// <returns>
        /// True si la cadena contiene las etiquetas "pdfaid:part" o "pdfaid:conformance"
        /// y sus valores no están vacíos, de lo contrario False.
        /// </returns>
        private static bool ValidatePdfAStringFormat(string pdfInStringFormat)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(pdfInStringFormat);

                XmlNodeList nodesPart = xmlDoc.GetElementsByTagName("pdfaid:part");
                XmlNodeList nodesConformance = xmlDoc.GetElementsByTagName("pdfaid:conformance");

                bool hasValidPartNode = nodesPart.Cast<XmlNode>().Any(node => !string.IsNullOrWhiteSpace(node.InnerText));
                bool hasValidConformanceNode = nodesConformance.Cast<XmlNode>().Any(node => !string.IsNullOrWhiteSpace(node.InnerText));

                return hasValidPartNode || hasValidConformanceNode;
            }
            catch (XmlException ex)
            {
                Console.WriteLine($"Error al cargar el XML: {ex.Message}");
                return false;
            }
        }
    }
}
