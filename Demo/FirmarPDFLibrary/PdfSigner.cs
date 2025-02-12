using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using iText.Kernel.Pdf;
using Org.BouncyCastle.X509;
using static iText.Signatures.PdfSigner;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;

namespace FirmarPDFLibrary;

public static class PdfSigner
{
    public static string Sign(MemoryStream pdfStream, X509Certificate2 certificate, string reason, string country)
    {
        try
        {
            byte[] pdfBytes = pdfStream.ToArray();
            string fieldName = Guid.NewGuid().ToString();
            return SignPdfDocumentForRestful(certificate, reason, country, pdfBytes, fieldName);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Firma un documento PDF utilizando un certificado X509 y retorna el documento firmado como una cadena Base64.
    /// </summary>
    private static string SignPdfDocumentForRestful(X509Certificate2 certificate, string reason, string country,
        byte[] pdfBytes, string fieldName)
    {
        try
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                SignPdfDocument(memoryStream, certificate, reason, country, pdfBytes, fieldName);
                byte[] outPdfBytes = memoryStream.ToArray();
                return Convert.ToBase64String(outPdfBytes);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private static void SignPdfDocument(Stream targetStream, X509Certificate2 certificate, string reason,
        string country, byte[] pdfBytes, string fieldName)
    {
        if (pdfBytes == null || pdfBytes.Length == 0) throw new ArgumentNullException(nameof(pdfBytes));
        if (targetStream == null) throw new ArgumentNullException(nameof(targetStream));
        if (certificate == null) throw new ArgumentNullException(nameof(certificate));

        // Crear un lector de PDF a partir del byte array
        PdfReader reader = new PdfReader(new MemoryStream(pdfBytes));
        iText.Signatures.PdfSigner signer = new iText.Signatures.PdfSigner(reader, targetStream, new StampingProperties()
            .UseAppendMode());

        // Configurar la apariencia de la firma
        signer.SetFieldName(fieldName);
        signer.GetSignatureAppearance()
            .SetReason(reason)
            .SetLocation(country);

        // Convertir el certificado a BouncyCastle para la cadena
        X509CertificateParser objCP = new X509CertificateParser();
        X509Certificate[] objChain = { objCP.ReadCertificate(certificate.RawData) };

        // Usar el proveedor de servicios RSA de .NET directamente en la firma
        var rsaProvider = certificate.GetRSAPrivateKey();
        var externalSignature = new ExternalSignatureImplementation(rsaProvider, "SHA256");

        // Firmar el documento
        signer.SignDetached(externalSignature, objChain, null, null, null, 0, CryptoStandard.CMS);
    }
}