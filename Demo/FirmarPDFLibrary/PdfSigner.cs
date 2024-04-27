using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.X509;
using iTextSharp.text.pdf.security;
using BX509 = Org.BouncyCastle.X509;
using System.Text;

namespace FirmarPDFLibrary
{
    public class PDF
    {
        public static bool IsValidPDFA(string source)
        {
            try
            {
                PdfReader reader = new PdfReader(source);
                byte[] metadata = reader.Metadata;

                if (metadata == null)
                    return false;

                string sXmlMetadata = Encoding.Default.GetString(metadata);
                var xmlDoc = new System.Xml.XmlDocument();
                xmlDoc.LoadXml(sXmlMetadata);
                var nodesPart = xmlDoc.GetElementsByTagName("pdfaid:part");
                var nodesConformance = xmlDoc.GetElementsByTagName("pdfaid:conformance");

                if (nodesPart != null || nodesConformance != null)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool SignHashed(string source, string target, X509Certificate2 certificate, string reason, string location, bool addVisibleSign)
        {
            try
            {
                PdfSignatureAppearance appearance = null;
                PdfReader reader = new PdfReader(source);

                AcroFields fields = reader.AcroFields;
                X509CertificateParser objCP = new X509CertificateParser();
                BX509.X509Certificate[] objChain = new BX509.X509Certificate[] { objCP.ReadCertificate(certificate.RawData) };

                string fieldName = Guid.NewGuid().ToString();

                float width = 100.0F;
                float height = 45.0F;
                float left = 45.0F;
                float right = 120.0F;

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

                using (FileStream outFile = new FileStream(target, FileMode.Create))
                {
                    using (PdfStamper stamper = PdfStamper.CreateSignature(reader, outFile, '\0', null, true))
                    {
                        appearance = stamper.SignatureAppearance;

                        string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "logo.png");
                        appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION;
                        appearance.SignatureGraphic = Image.GetInstance(imagePath);

                        if (addVisibleSign)
                            appearance.SetVisibleSignature(new Rectangle(left, height, right, width), reader.NumberOfPages, fieldName);

                        appearance.Reason = reason;
                        appearance.Location = location;
                        appearance.Acro6Layers = true;


                        IExternalSignature signature = new X509Certificate2Signature(certificate, "SHA-1");
                        MakeSignature.SignDetached(appearance, signature, objChain, null, null, null, 0, CryptoStandard.CMS);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }

}
