using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Helper.Services
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }

        public ValidationResult() { }
    }
    public class VerifyMultiSignature
    {
        #region Private variables
        private static long? maxCharactersFromEntities = null;
        private static bool s_readMaxCharactersInDocument = false;
        private static long s_maxCharactersInDocument = 0;
        #endregion

        #region Constructor
        public VerifyMultiSignature() { }
        #endregion

        #region Internal methods
        internal static long GetNetFxSecurityRegistryValue(string regValueName, long defaultValue)
        {
            try
            {
                using (RegistryKey securityRegKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\.NETFramework\Security", false))
                {
                    if (securityRegKey != null)
                    {
                        object regValue = securityRegKey.GetValue(regValueName);
                        if (regValue != null)
                        {
                            RegistryValueKind valueKind = securityRegKey.GetValueKind(regValueName);
                            if (valueKind == RegistryValueKind.DWord || valueKind == RegistryValueKind.QWord)
                            {
                                return Convert.ToInt64(regValue, CultureInfo.InvariantCulture);
                            }
                        }
                    }
                }
            }
            catch (SecurityException) { /* we could not open the key - that's fine, we can proceed with the default value */ }

            return defaultValue;
        }
        internal static long GetMaxCharactersFromEntities()
        {
            if (maxCharactersFromEntities.HasValue)
            {
                return maxCharactersFromEntities.Value;
            }

            long maxCharacters = GetNetFxSecurityRegistryValue("SignedXmlMaxCharactersFromEntities", (long)1e7);

            maxCharactersFromEntities = maxCharacters;
            return maxCharactersFromEntities.Value;
        }
        internal static long GetMaxCharactersInDocument()
        {
            // Allow machine administrators to specify a maximum document load size for SignedXml.
            if (s_readMaxCharactersInDocument)
            {
                return s_maxCharactersInDocument;
            }

            // The default value, 0, is "no limit"
            long maxCharacters = GetNetFxSecurityRegistryValue("SignedXmlMaxCharactersInDocument", 0);

            s_maxCharactersInDocument = maxCharacters;
            s_readMaxCharactersInDocument = true;

            return s_maxCharactersInDocument;
        }
        internal class MyXmlDocument : XmlDocument
        {
            protected override XmlAttribute CreateDefaultAttribute(string prefix, string localName, string namespaceURI)
            {
                return this.CreateAttribute(prefix, localName, namespaceURI);
            }
        }
        internal static XmlDocument PreProcessElementInput(XmlElement elem, XmlResolver xmlResolver, string baseUri)
        {
            if (elem == null)
                throw new ArgumentNullException("elem");

            MyXmlDocument doc = new MyXmlDocument();
            doc.PreserveWhitespace = true;
            // Normalize the document
            using (TextReader stringReader = new StringReader(elem.OuterXml))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.XmlResolver = xmlResolver;
                settings.DtdProcessing = DtdProcessing.Parse;
                settings.MaxCharactersFromEntities = GetMaxCharactersFromEntities();
                settings.MaxCharactersInDocument = GetMaxCharactersInDocument();
                XmlReader reader = XmlReader.Create(stringReader, settings, baseUri);
                doc.Load(reader);
            }
            return doc;
        }
        internal static string ComputeHash(string DigestMethod, XmlDsigC14NTransform c14n)
        {
            Stream canonalisedStream = (Stream)c14n.GetOutput(typeof(Stream));

            if (DigestMethod == "http://www.w3.org/2000/09/xmldsig#sha1")
            {
                return Convert.ToBase64String(new SHA1CryptoServiceProvider().ComputeHash(canonalisedStream));
            }
            else if (DigestMethod == "http://www.w3.org/2001/04/xmlenc#sha256")
            {
                return Convert.ToBase64String(new SHA256CryptoServiceProvider().ComputeHash(canonalisedStream));
            }
            else if (DigestMethod == "http://www.w3.org/2001/04/xmlenc#sha512")
            {
                return Convert.ToBase64String(new SHA512CryptoServiceProvider().ComputeHash(canonalisedStream));
            }
            else
            {
                throw new Exception("Unsupported digest method");
            }
        }
        internal bool VerifyDigest(XmlDocument document, SignedXml aVerifier, int indexOfSignature)
        {
            XmlDocument doc = document;
            doc.PreserveWhitespace = true;

            string digestMethod = aVerifier.SignedInfo.GetXml().ParentNode.FirstChild.ChildNodes[2].ChildNodes[1].Attributes[0].Value;
            string digestValue = aVerifier.SignedInfo.GetXml().InnerText;

            XmlNode xmlNode = doc.GetElementsByTagName("ds:Signature").Item(indexOfSignature);
            doc.DocumentElement.RemoveChild(xmlNode);

            //create c14n instance and load in xml file
            XmlDsigC14NTransform c14n = new XmlDsigC14NTransform(true);
            c14n.LoadInput(doc);

            //Getting the Base64 version of digest
            string xmlDigestValue = ComputeHash(digestMethod, c14n);

            // If Computed and original digest value matches then return true else false.
            if (xmlDigestValue == digestValue)
                return true;
            return false;
        }
        #endregion

        #region Public methods
        public ValidationResult MatchesSignature(XmlDocument document, X509Certificate2 x509cert, int indexOfSignature)
        {
            ValidationResult result = new ValidationResult();
            XmlDocument doc = document;
            doc.PreserveWhitespace = true;
            SignedXml verifier = new SignedXml();

            //Next, the SignedXml class must be given the value of the signature it is to validate.This can be done by looking for elements with the tag name of Signature.See code below:
            XmlElement xmlElement = (System.Xml.XmlElement)document.GetElementsByTagName("ds:Signature").Item(indexOfSignature);
            verifier.LoadXml(xmlElement);

            // Get the public key
            AsymmetricAlgorithm key = x509cert.PublicKey.Key;
            // =================================================================================
            if (key == null)
                throw new ArgumentNullException("key");

            SignatureDescription signatureDescription = CryptoConfig.CreateFromName(verifier.SignatureMethod) as SignatureDescription;
            if (signatureDescription == null)
                throw new CryptographicException("123456789"); // "123456789" is an arbitrary value.

            // Let's see if the key corresponds with the SignatureMethod
            Type ta = Type.GetType(signatureDescription.KeyAlgorithm);
            Type tb = key.GetType();
            if ((ta != tb) && !ta.IsSubclassOf(tb) && !tb.IsSubclassOf(ta))
            {
                result.IsValid = false;
                result.Message = "La verificación de la firma no ha sido satisfactoria";

                return result;
            }

            HashAlgorithm hashAlgorithm = signatureDescription.CreateDigest();
            if (hashAlgorithm == null)
                throw new CryptographicException("123456789");

            //====================================================
            byte[] digestedSignedInfo = null;
            string baseUri = (doc == null ? null : doc.BaseURI);
            XmlResolver resolver = new XmlSecureResolver(new XmlUrlResolver(), baseUri);
            XmlDocument docProcessed = PreProcessElementInput(verifier.SignedInfo.GetXml(), resolver, baseUri);
            System.Security.Cryptography.Xml.Transform c14nMethodTransform = verifier.SignedInfo.CanonicalizationMethodObject;
            c14nMethodTransform.Resolver = resolver;
            c14nMethodTransform.LoadInput(docProcessed);
            digestedSignedInfo = c14nMethodTransform.GetDigestedOutput(hashAlgorithm);
            //====================================================
            AsymmetricSignatureDeformatter asymmetricSignatureDeformatter = signatureDescription.CreateDeformatter(key);

            // Verify the signature
            bool isSignatureOK = asymmetricSignatureDeformatter.VerifySignature(digestedSignedInfo, verifier.SignatureValue);
            bool isDigestOK = this.VerifyDigest(doc, verifier, indexOfSignature);
            if (isSignatureOK && isDigestOK)
            {
                result.IsValid = true;
                result.Message = "Verificación de la firma satisfactoria";

                return result;
            }
            result.IsValid = false;
            result.Message = "La verificación de la firma no ha sido satisfactoria";

            return result;
        }
        #endregion
    }
}
