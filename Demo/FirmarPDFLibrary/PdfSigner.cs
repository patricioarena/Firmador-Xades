using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using SysX509 = System.Security.Cryptography.X509Certificates;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.X509;
using System.Security.Cryptography.Pkcs;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.Crypto.Tls;
using System.Runtime.Remoting.Messaging;
using Org.BouncyCastle.Asn1.X509;
using System.Security.Policy;

namespace FirmarPDFLibrary
{
    public class PDF
    {
        public static void SignHashed(string source, string target, X509Certificate2 certificate, string reason, string location, bool addVisibleSign)
        {
            string fieldName = "cifeSignature-" + Guid.NewGuid().ToString(); 
            
            X509CertificateParser objCP = new X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate[] objChain = new Org.BouncyCastle.X509.X509Certificate[] { objCP.ReadCertificate(certificate.RawData) };

            PdfReader reader = new PdfReader(source);

            AcroFields fields = reader.AcroFields;
            bool tieneFirma = fields.GetSignatureNames().Count > 0;

            using (FileStream outFile = new FileStream(target, FileMode.Create))
            {
                using (PdfStamper stamper = PdfStamper.CreateSignature(reader, outFile, '\0', null, true))
                {
    
                    PdfSignatureAppearance appearance = stamper.SignatureAppearance;

                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "cife_logo.png");
                    appearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.GRAPHIC_AND_DESCRIPTION;
                    appearance.SignatureGraphic = iTextSharp.text.Image.GetInstance(imagePath);
                    appearance.SignatureGraphic = Image.GetInstance(imagePath);

                    if (addVisibleSign)
                    {
                        appearance.SetVisibleSignature(new Rectangle(50, 50, 300, 100), reader.NumberOfPages,  fieldName);
                    }

                    appearance.Reason = reason;
                    appearance.Location = location;
                    appearance.Acro6Layers = true;
                    

                    IExternalSignature signature = new X509Certificate2Signature(certificate, "SHA-1");
                    MakeSignature.SignDetached(appearance, signature, objChain, null, null, null, 0, CryptoStandard.CMS);
                }
            }
        }

        /// <summary>
        /// Crea la firma CMS/PKCS #7
        /// </summary>
        private static byte[] SignMsg(byte[] Message, SysX509.X509Certificate2 SignerCertificate, bool Detached)
        {
            //Creamos el contenedor
            ContentInfo contentInfo = new ContentInfo(Message);

            //Instanciamos el objeto SignedCms con el contenedor
            SignedCms objSignedCms = new SignedCms(contentInfo, Detached);

            //Creamos el "firmante"
            CmsSigner objCmsSigner = new CmsSigner(SignerCertificate);

            // Include the following line if the top certificate in the
            // smartcard is not in the trusted list.
            objCmsSigner.IncludeOption = SysX509.X509IncludeOption.EndCertOnly;

            //  Sign the CMS/PKCS #7 message. The second argument is
            //  needed to ask for the pin.
            objSignedCms.ComputeSignature(objCmsSigner, false);

            //Encodeamos el mensaje CMS/PKCS #7
            return objSignedCms.Encode();
        }
    }
}
