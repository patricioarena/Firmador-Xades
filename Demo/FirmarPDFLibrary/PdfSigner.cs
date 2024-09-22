using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.X509;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using BX509 = Org.BouncyCastle.X509;

namespace FirmarPDFLibrary
{
    public class PDFSigner
    {
        public static string Sign(MemoryStream pdfStream, X509Certificate2 certificate, string reason, string country, bool addVisibleSign)
        {
            try
            {
                PdfReader reader = new PdfReader(pdfStream);
                AcroFields fields = reader.AcroFields;
                X509CertificateParser objCP = new X509CertificateParser();
                BX509.X509Certificate[] objChain = new BX509.X509Certificate[] { objCP.ReadCertificate(certificate.RawData) };
                string fieldName = Guid.NewGuid().ToString();
                float width = 100.0F;
                float height = 45.0F;
                float left = 45.0F;
                float right = 120.0F;

                SetSignatureConfiguration(ref addVisibleSign, reader, fields, ref left, ref right);

                return SignPdfDocumentForRestful(certificate, reason, country, addVisibleSign, reader, objChain, fieldName, width, height, left, right);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Sign(string source, string target, X509Certificate2 certificate, string reason, string country, bool addVisibleSign)
        {
            try
            {
                PdfReader reader = new PdfReader(source);
                AcroFields fields = reader.AcroFields;
                X509CertificateParser objCP = new X509CertificateParser();
                BX509.X509Certificate[] objChain = new BX509.X509Certificate[] { objCP.ReadCertificate(certificate.RawData) };
                string fieldName = Guid.NewGuid().ToString();
                float width = 100.0F;
                float height = 45.0F;
                float left = 45.0F;
                float right = 120.0F;

                SetSignatureConfiguration(ref addVisibleSign, reader, fields, ref left, ref right);

                return SignPdfDocumentForDesktop(target, certificate, reason, country, addVisibleSign, reader, objChain, fieldName, width, height, left, right);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Configura los parámetros para la firma visible en un documento PDF, ajustando la posición de la firma en función de las firmas existentes.
        /// Si no hay espacio suficiente para agregar una firma visible en la página actual, se desactiva la firma visible.
        /// </summary>
        /// <param name="addVisibleSign">Referencia a un valor booleano que indica si se debe agregar una firma visible en el PDF. Este valor se ajustará en función del espacio disponible.</param>
        /// <param name="reader">Objeto <see cref="PdfReader"/> que contiene el documento PDF a firmar.</param>
        /// <param name="fields">Objeto <see cref="AcroFields"/> que representa los campos del formulario PDF, incluidas las firmas existentes.</param>
        /// <param name="left">Referencia a la posición izquierda de la firma visible, que se ajustará en función de la última firma existente en el PDF.</param>
        /// <param name="right">Referencia a la posición derecha de la firma visible, que se ajustará en función de la última firma existente en el PDF.</param>
        private static void SetSignatureConfiguration(ref bool addVisibleSign, PdfReader reader, AcroFields fields, ref float left, ref float right)
        {
            float x = 80.0f;
            if (fields.GetSignatureNames().Count > 0)
            {
                var signatureName = fields.GetSignatureNames().Last();
                var signaturePositions = fields.GetFieldPositions(signatureName);
                var signaturePosition = signaturePositions[0];

                left = signaturePosition.position.Left + x;
                right = signaturePosition.position.Right + x;

                var pageSizeRight = reader.GetPageSize(reader.NumberOfPages).Right;

                if (pageSizeRight < right)
                {
                    addVisibleSign = false; // Si no hay espacio para poner la firma visible, se oculta la firma
                }
            }
        }

        /// <summary>
        /// Firma un documento PDF utilizando un certificado X509 y lo guarda en un archivo.
        /// </summary>
        private static bool SignPdfDocumentForDesktop(string target, X509Certificate2 certificate, string reason, string country, bool addVisibleSign, PdfReader reader, BX509.X509Certificate[] objChain, string fieldName, float width, float height, float left, float right)
        {
            try
            {
                using (FileStream outFile = new FileStream(target, FileMode.Create))
                {
                    SignPdfDocument(outFile, certificate, reason, country, addVisibleSign, reader, objChain, fieldName, width, height, left, right);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Firma un documento PDF utilizando un certificado X509 y retorna el documento firmado como una cadena Base64.
        /// </summary>
        private static string SignPdfDocumentForRestful(X509Certificate2 certificate, string reason, string country, bool addVisibleSign, PdfReader reader, BX509.X509Certificate[] objChain, string fieldName, float width, float height, float left, float right)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    SignPdfDocument(memoryStream, certificate, reason, country, addVisibleSign, reader, objChain, fieldName, width, height, left, right);
                    byte[] pdfBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(pdfBytes);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Firma un documento PDF utilizando un certificado X509, opcionalmente agregando una firma visible con un gráfico.
        /// </summary>
        /// <param name="targetStream">Stream de destino donde se guardará el documento firmado (puede ser un FileStream o MemoryStream).</param>
        /// <param name="certificate">Certificado X509 usado para firmar el documento.</param>
        /// <param name="reason">Razón para la firma, que se mostrará en los metadatos del PDF.</param>
        /// <param name="country">Ubicación que se mostrará en los metadatos del PDF como lugar de la firma.</param>
        /// <param name="addVisibleSign">Indica si se debe agregar una firma visible en el PDF.</param>
        /// <param name="reader">Objeto PdfReader que contiene el documento PDF a firmar.</param>
        /// <param name="objChain">Cadena de certificados utilizada para la validación de la firma.</param>
        /// <param name="fieldName">Nombre del campo de firma dentro del PDF.</param>
        /// <param name="width">Ancho de la firma visible en el PDF.</param>
        /// <param name="height">Altura de la firma visible en el PDF.</param>
        /// <param name="left">Posición izquierda de la firma visible en el PDF.</param>
        /// <param name="right">Posición derecha de la firma visible en el PDF.</param>
        private static void SignPdfDocument(Stream targetStream, X509Certificate2 certificate, string reason, string country, bool addVisibleSign, PdfReader reader, BX509.X509Certificate[] objChain, string fieldName, float width, float height, float left, float right)
        {
            using (PdfStamper stamper = PdfStamper.CreateSignature(reader, targetStream, '\0', null, true))
            {
                PdfSignatureAppearance appearance = stamper.SignatureAppearance;

                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "logo.png");
                appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION;
                appearance.SignatureGraphic = Image.GetInstance(imagePath);

                if (addVisibleSign)
                    appearance.SetVisibleSignature(new Rectangle(left, height, right, width), reader.NumberOfPages, fieldName);

                appearance.Reason = reason;
                appearance.Location = country;
                appearance.Acro6Layers = true;

                IExternalSignature signature = new X509Certificate2Signature(certificate, "SHA-1");
                MakeSignature.SignDetached(appearance, signature, objChain, null, null, null, 0, CryptoStandard.CMS);
            }
        }
    }
}
