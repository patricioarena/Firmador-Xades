using FirmaXadesNet.Signature.Parameters;
using FirmaXadesNet.Utils;
using Microsoft.Win32;
using Microsoft.Xades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Demo.Services
{
    public class VerifyMultiSignature
    {
        private static long? maxCharactersFromEntities = null;
        private static bool s_readMaxCharactersInDocument = false;
        private static long s_maxCharactersInDocument = 0;
        public VerifyMultiSignature(){}

        private static long GetNetFxSecurityRegistryValue(string regValueName, long defaultValue)
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

        public bool MatchesSignature(XmlDocument document, X509Certificate2 x509cert , string strDigestValue)
        {
            XmlDocument doc = document;
            doc.PreserveWhitespace = true;
            SignedXml verifier = new SignedXml();

            //Next, the SignedXml class must be given the value of the signature it is to validate.This can be done by looking for elements with the tag name of Signature.See code below:
            verifier.LoadXml(doc.GetElementsByTagName("ds:Signature")[0] as XmlElement);

            X509Certificate2 x509 = x509cert;
            // Get the public key
            AsymmetricAlgorithm key = x509.PublicKey.Key;
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
                // Signature method key mismatch
                return false;

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
            bool isDigestOK = this.VerifyDigest(doc, verifier, strDigestValue);
            if (isSignatureOK && isDigestOK)
            {
                return true;
            }
            return false;
        }

        public bool VerifyDigest(XmlDocument document, SignedXml aVerifier, string strDigestValue)
        {
            //https://youtu.be/jUzjilTxdzk //Nice music
            XmlDocument doc = document;
            doc.PreserveWhitespace = true;

            int count = doc.GetElementsByTagName("ds:Signature").Count;

            XmlNode xmlNode = doc.GetElementsByTagName("ds:Signature")[count-1];
            doc.DocumentElement.RemoveChild(xmlNode);

            var temp = doc;

            //create c14n instance and load in xml file
            XmlDsigC14NTransform c14n = new XmlDsigC14NTransform(false);

            // Loading the Assetion Node into the canonicalization
            c14n.LoadInput(doc);

            //get canonalised stream
            Stream canonalisedStream = (Stream)c14n.GetOutput(typeof(Stream));

            //Creating SHA1 object to get Hash
            SHA256 sHA256 = new SHA256CryptoServiceProvider();

            Byte[] output = sHA256.ComputeHash(canonalisedStream);

            //Getting the Base64 version of digest Value computed
            string xmlDigestValue = Convert.ToBase64String(output);

            // If Computed and original digest value matches then return true else false.
            if (xmlDigestValue == strDigestValue)
            {
                return true;
            }

            return false;

        }
    }
}
