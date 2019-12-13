using FirmaXadesNet.Signature.Parameters;
using FirmaXadesNet.Utils;
using Microsoft.Xades;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Demo.Services
{
    public class Class1
    {
        public Class1() 
        {
        }
        public string GetSerialNumberAsDecimalString(X509Certificate2 certificate)
        {
            List<int> dec = new List<int> { 0 };

            foreach (char c in certificate.SerialNumber)
            {
                int carry = Convert.ToInt32(c.ToString(), 16);

                for (int i = 0; i < dec.Count; ++i)
                {
                    int val = dec[i] * 16 + carry;
                    dec[i] = val % 10;
                    carry = val / 10;
                }

                while (carry > 0)
                {
                    dec.Add(carry % 10);
                    carry /= 10;
                }
            }

            var chars = dec.Select(d => (char)('0' + d));
            var cArr = chars.Reverse().ToArray();
            return new string(cArr);
        }
        public bool VerifyDigest(XmlDocument xAssertionDoc, string strDigestValue)
        {

            Cert cert = new Cert();

            SignatureParameters parameters = new SignatureParameters();
            String stringCert = "MIIHzzCCBbegAwIBAgITfwAGU6DLt+sbEXWkyAAAAAZToDANBgkqhkiG9w0BAQsFADCCAUwxCzAJBgNVBAYTAkFSMSkwJwYDVQQIDCBDaXVkYWQgQXV0w7Nub21hIGRlIEJ1ZW5vcyBBaXJlczEzMDEGA1UECwwqU3Vic2VjcmV0YXLDrWEgZGUgVGVjbm9sb2fDrWFzIGRlIEdlc3Rpw7NuMSkwJwYDVQQLDCBTZWNyZXRhcsOtYSBkZSBHZXN0acOzbiBQw7pibGljYTE5MDcGA1UECwwwT2ZpY2luYSBOYWNpb25hbCBkZSBUZWNub2xvZ8OtYXMgZGUgSW5mb3JtYWNpw7NuMSowKAYDVQQKDCFKZWZhdHVyYSBkZSBHYWJpbmV0ZSBkZSBNaW5pc3Ryb3MxGTAXBgNVBAUTEENVSVQgMzA2ODA2MDQ1NzIxMDAuBgNVBAMMJ0F1dG9yaWRhZCBDZXJ0aWZpY2FudGUgZGUgRmlybWEgRGlnaXRhbDAeFw0xOTEwMzAxMzUzMzhaFw0yMDEwMjIxNTEwNTZaMFExGTAXBgNVBAUTEENVSUwgMjAzMzY4ODY0NTgxCzAJBgNVBAYTAkFSMScwJQYDVQQDEx5BUkVOQSBQYXRyaWNpbyBFcm5lc3RvIEFudG9uaW8wggEiMA0GCSqGSIb3DQEBAQUAA4IBDwAwggEKAoIBAQDBiHGfDYJ17bGPLd1/Wr/klLrlES4LTm0VCl3Jmw3M5jFqU/+iO5BD0WkabjTNiF71Cb1DDytQhVhARb4tyxtkoimfuDgxBbwRi6jsfw6Z3y/HDQgCKjYQz+9LvE7i7NAFaIyEb3xh2NmpyGd33TEYzVf96U2eW9zi2jcEMYOwX3dchhGcvfyEoNRjtQ2ZtXOenkCUXeYt2/opeb/LFNxDRANulCub57SDhrZHp2yAryD5JbqfeyFUXHP1mFLrYppPHa05e3TReohU+EfDF5KxiQHQ2siZGo6Rmri2Q/8tU91TZfGEdL3b/JbzNnKPdTX4ssbKMMI1erC2K1eB1/SjAgMBAAGjggKhMIICnTAkBggrBgEFBQcBAwQYMBYwFAYIKwYBBQUHCwIwCAYGYCABCgICMA4GA1UdDwEB/wQEAwIE8DAdBgNVHQ4EFgQUXwlhHjRiQTd3YcEXBVnUlPB/PuQwHgYDVR0RBBcwFYETcGFyZW5hQGZlcGJhLmdvdi5hcjAfBgNVHSMEGDAWgBSMG/0ubHBsM5IabSTMm8w/mcXrqDBXBgNVHR8EUDBOMEygSqBIhiBodHRwOi8vcGtpLmpnbS5nb3YuYXIvY3JsL0ZELmNybIYkaHR0cDovL3BraWNvbnQuamdtLmdvdi5hci9jcmwvRkQuY3JsMIHQBggrBgEFBQcBAQSBwzCBwDAyBggrBgEFBQcwAoYmaHR0cDovL3BraS5qZ20uZ292LmFyL2FpYS9jYWZkT05USS5jcnQwNgYIKwYBBQUHMAKGKmh0dHA6Ly9wa2ljb250LmpnbS5nb3YuYXIvYWlhL2NhZmRPTlRJLmNydDAmBggrBgEFBQcwAYYaaHR0cDovL3BraS5qZ20uZ292LmFyL29jc3AwKgYIKwYBBQUHMAGGHmh0dHA6Ly9wa2ljb250LmpnbS5nb3YuYXIvb2NzcDAMBgNVHRMBAf8EAjAAMD0GCSsGAQQBgjcVBwQwMC4GJisGAQQBgjcVCIb66iGBpoxmh6GHK4a7ljmGx+VaKIb/sHiF8rJTAgFkAgEOMB0GA1UdJQQWMBQGCCsGAQUFBwMEBggrBgEFBQcDAjAnBgkrBgEEAYI3FQoEGjAYMAoGCCsGAQUFBwMEMAoGCCsGAQUFBwMCMEQGA1UdIAQ9MDswOQYFYCABAQMwMDAuBggrBgEFBQcCARYiaHR0cDovL3BraS5qZ20uZ292LmFyL2Nwcy9jcHMucGRmADANBgkqhkiG9w0BAQsFAAOCAgEAorwcRkzIj9DEy35dA43q7J3EwDX5LoeLdwjoZZpPKLuwXdKbs5gyzXAhCP1D7x1kuBcr5tkuu13GC5KMFOj/iwDi8bD8Uk1bv7pMEFURKN3tQY5om/+RgqSUyDST+wJ14ee7pwTuUSNKowd3eAWKZt0DoGqHrhfDuV483qD2sJBTYDiJARVwoSY95XcKS2TiSCGNdpQCXRB/MGhEESV81Wxs+zZOqvlZYIaWpzLRKwX5YhLE0ht8FuBJXGzKKR7xMbEe6akmhDF6flgKIpQNUt/5Qzu+maYmGUPQgWStHeEfY9a9/EAkq9oO1XbmmLdHNWsk/AC+sBza6ps+wbo8UvjBMtKdA2qfthCZz2nuk0KeixawzwAjSf5NkiAo/yAtVrgzgoOgSmNdF6G5UqC7qxHNFGmYavQRegfc1fd/zkq9R7eWJbhw8nNAmVFHuqzeQzQWdl3pleGk0foBgcmaeRKb6GclQ+YJjiEVa5a+rSHUASqKCbdBS6bd/mikc1xN2Y6GeGpN4dc+L/KgjLavQVTMIm942c/fsRkRHfeZgg96vvFrKTp/PjpOrA7i9L7ncDBpfuFd7gTZXxdM1m98LZ6Z+Yt6i7PD54xLbJsFTjHBbmQIvs+ywv7bWS6rvAojLecL+we0oL1pH91ADUei2jr21XYMaiqFP/s4zci0a7s=";
            Byte[] rawCertData = System.Convert.FromBase64String(stringCert);
            X509Certificate2 x509Certificate2 = new X509Certificate2(rawCertData);

            cert.IssuerSerial.X509IssuerName = x509Certificate2.IssuerName.Name;
            cert.IssuerSerial.X509SerialNumber = GetSerialNumberAsDecimalString(parameters.Signer.Certificate);

            var temp = Convert.ToBase64String(cert.CertDigest.DigestValue);

            // if xmlsignature is available in the XML then remove the signature node
            //xAssertionDoc.DocumentElement.RemoveChild(xAssertionDoc.GetElementsByTagName("ds:Signature")[0]);

            //create c14n instance and load in xml file
            XmlDsigC14NTransform c14n = new XmlDsigC14NTransform(false);

            // Loading the Assetion Node into the canonicalization
            c14n.LoadInput(xAssertionDoc);

            //get canonalised stream
            Stream canonalisedStream = (Stream)c14n.GetOutput(typeof(Stream));

            //Creating SHA1 object to get Hash
            var sha1 = new SHA512CryptoServiceProvider();

            Byte[] output = sha1.ComputeHash(canonalisedStream);

            //Getting the Base64 version of digest Value computed
            string xmlDigestValue = Convert.ToBase64String(output);

            // If Computed and original digest value matches then return true else false.
            if (xmlDigestValue == strDigestValue)
            {
                Console.WriteLine("The computed hash matches with the Digest contained in the XML.");
                return true;
            }

            return false;
        }
    }

 


}
