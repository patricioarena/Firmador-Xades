﻿using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.X509;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;
using X509Extension = Org.BouncyCastle.Asn1.X509.X509Extension;

namespace Helper.Services;

public enum CertificateStatus { Good = 0, Revoked = 1, Unknown = 2 };

public class OcspClient
{
    private static readonly int BufferSize = 4096 * 8;

    private static readonly int MaxClockSkew = 36000000;

    public CertificateStatus Validate_Certificate_Using_OCSP_Protocol(X509Certificate2 x509Certificate2)
    {
        X509Certificate eeCert = new X509CertificateParser().ReadCertificate(x509Certificate2.GetRawCertData());
        X509Certificate2 issuerSource = GetIssuer(x509Certificate2);
        X509Certificate issuerCert = new X509CertificateParser().ReadCertificate(issuerSource.GetRawCertData());

        // Query the first Ocsp Url found in certificate
        List<string> urls = GetAuthorityInformationAccessOcspUrl(eeCert);

        if (urls.Count == 0)
        {
            //throw new Exception("No OCSP url found in ee certificate.");
            return CertificateStatus.Unknown;
        }

        string url = urls[0];

        Console.WriteLine("Querying '" + url + "'...");

        OcspReq req = GenerateOcspRequest(issuerCert, eeCert.SerialNumber);

        byte[] binaryResp = PostData(url, req.GetEncoded(), "application/ocsp-request", "application/ocsp-response");

        return ProcessOcspResponse(eeCert, issuerCert, binaryResp);
    }

    public byte[] PostData(string url, byte[] data, string contentType, string accept)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "POST";
        request.ContentType = contentType;
        request.ContentLength = data.Length;
        request.Accept = accept;
        Stream stream = request.GetRequestStream();
        stream.Write(data, 0, data.Length);
        stream.Close();
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        Stream respStream = response.GetResponseStream();
        byte[] resp = ToByteArray(respStream);
        respStream.Close();

        return resp;
    }

    public byte[] ToByteArray(Stream stream)
    {
        byte[] buffer = new byte[BufferSize];
        MemoryStream ms = new MemoryStream();

        int read = 0;

        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            ms.Write(buffer, 0, read);
        }

        return ms.ToArray();
    }

    public static List<string> GetAuthorityInformationAccessOcspUrl(X509Certificate cert)
    {
        List<string> ocspUrls = new List<string>();

        try
        {
            Asn1Object obj = GetExtensionValue(cert, X509Extensions.AuthorityInfoAccess.Id);

            if (obj == null)
            {
                return null;
            }

            // For a strange reason I cannot acess the aia.AccessDescription[].
            // Hope it will be fixed in the next version (1.5).
            // AuthorityInformationAccess aia = AuthorityInformationAccess.GetInstance(obj);

            // Switched to manual parse
            Asn1Sequence s = (Asn1Sequence)obj;
            IEnumerator elements = s.GetEnumerator();

            while (elements.MoveNext())
            {
                Asn1Sequence element = (Asn1Sequence)elements.Current;
                DerObjectIdentifier oid = (DerObjectIdentifier)element[0];

                if (oid.Id.Equals("1.3.6.1.5.5.7.48.1")) // Is Ocsp?
                {
                    Asn1TaggedObject taggedObject = (Asn1TaggedObject)element[1];
                    GeneralName gn = (GeneralName)GeneralName.GetInstance(taggedObject);
                    ocspUrls.Add(((DerIA5String)DerIA5String.GetInstance(gn.Name)).GetString());
                }
            }
        }
        catch (Exception e)
        {
            //throw new Exception("Error parsing AIA.", e);
            return ocspUrls;
        }

        return ocspUrls;
    }

    protected static Asn1Object GetExtensionValue(X509Certificate cert,
            string oid)
    {
        if (cert == null)
        {
            return null;
        }

        byte[] bytes = cert.GetExtensionValue(new DerObjectIdentifier(oid)).GetOctets();

        if (bytes == null)
        {
            return null;
        }

        Asn1InputStream aIn = new Asn1InputStream(bytes);

        return aIn.ReadObject();
    }

    private static X509Certificate2 GetIssuer(X509Certificate2 leafCert)
    {
        if (leafCert.Subject == leafCert.Issuer) { return leafCert; }
        X509Chain chain = new X509Chain();
        chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
        chain.Build(leafCert);
        X509Certificate2 issuer = null;
        if (chain.ChainElements.Count > 1)
        {
            issuer = chain.ChainElements[1].Certificate;
        }
        chain.Reset();
        return issuer;
    }

    private CertificateStatus ProcessOcspResponse(X509Certificate eeCert, X509Certificate issuerCert, byte[] binaryResp)
    {
        OcspResp r = new OcspResp(binaryResp);
        CertificateStatus cStatus = CertificateStatus.Unknown;

        switch (r.Status)
        {
            case OcspRespStatus.Successful:
                BasicOcspResp or = (BasicOcspResp)r.GetResponseObject();

                if (or.Responses.Length == 1)
                {
                    SingleResp resp = or.Responses[0];

                    ValidateCertificateId(issuerCert, eeCert, resp.GetCertID());

                    Object certificateStatus = resp.GetCertStatus();

                    if (certificateStatus == Org.BouncyCastle.Ocsp.CertificateStatus.Good)
                    {
                        cStatus = CertificateStatus.Good;
                    }
                    else if (certificateStatus is RevokedStatus)
                    {
                        cStatus = CertificateStatus.Revoked;
                    }
                    else if (certificateStatus is UnknownStatus)
                    {
                        cStatus = CertificateStatus.Unknown;
                    }
                }
                break;
            default:
                //throw new Exception("Unknow status '" + r.Status + "'.");
                return cStatus;
        }

        return cStatus;
    }

    private void ValidateResponse(BasicOcspResp or, X509Certificate issuerCert)
    {
        ValidateResponseSignature(or, issuerCert.GetPublicKey());
        ValidateSignerAuthorization(issuerCert, or.GetCerts()[0]);
    }

    //3. The identity of the signer matches the intended recipient of the
    //request.
    //4. The signer is currently authorized to sign the response.
    private void ValidateSignerAuthorization(X509Certificate issuerCert, X509Certificate signerCert)
    {
        // This code just check if the signer certificate is the same that issued the ee certificate
        // See RFC 2560 for more information
        if (!(issuerCert.IssuerDN.Equivalent(signerCert.IssuerDN) && issuerCert.SerialNumber.Equals(signerCert.SerialNumber)))
        {
            throw new Exception("Invalid OCSP signer");
        }
    }

    //2. The signature on the response is valid;
    private void ValidateResponseSignature(BasicOcspResp or, AsymmetricKeyParameter asymmetricKeyParameter)
    {
        if (!or.Verify(asymmetricKeyParameter))
        {
            throw new Exception("Invalid OCSP signature");
        }
    }

    //6. When available, the time at or before which newer information will
    //be available about the status of the certificate (nextUpdate) is
    //greater than the current time.
    private void ValidateNextUpdate(SingleResp resp)
    {
        if (resp.NextUpdate != null && resp.NextUpdate.Value != null && resp.NextUpdate.Value.Ticks <= DateTime.Now.Ticks)
        {
            throw new Exception("Invalid next update.");
        }
    }

    //5. The time at which the status being indicated is known to be
    //correct (thisUpdate) is sufficiently recent.
    private void ValidateThisUpdate(SingleResp resp)
    {
        if (Math.Abs(resp.ThisUpdate.Ticks - DateTime.Now.Ticks) > MaxClockSkew)
        {
            throw new Exception("Max clock skew reached.");
        }
    }

    //1. The certificate identified in a received response corresponds to
    //that which was identified in the corresponding request;
    private void ValidateCertificateId(X509Certificate issuerCert, X509Certificate eeCert, CertificateID certificateId)
    {
        CertificateID expectedId = new CertificateID(CertificateID.HashSha1, issuerCert, eeCert.SerialNumber);

        if (!expectedId.SerialNumber.Equals(certificateId.SerialNumber))
        {
            throw new Exception("Invalid certificate ID in response");
        }

        if (!Arrays.AreEqual(expectedId.GetIssuerNameHash(), certificateId.GetIssuerNameHash()))
        {
            throw new Exception("Invalid certificate Issuer in response");
        }
    }

    private OcspReq GenerateOcspRequest(X509Certificate issuerCert, BigInteger serialNumber)
    {
        CertificateID id = new CertificateID(CertificateID.HashSha1, issuerCert, serialNumber);
        return GenerateOcspRequest(id);
    }

    private OcspReq GenerateOcspRequest(CertificateID id)
    {
        OcspReqGenerator ocspRequestGenerator = new OcspReqGenerator();

        ocspRequestGenerator.AddRequest(id);

        BigInteger nonce = BigInteger.ValueOf(new DateTime().Ticks);

        ArrayList oids = new ArrayList();
        Hashtable values = new Hashtable();

        oids.Add(OcspObjectIdentifiers.PkixOcsp);

        Asn1OctetString asn1 = new DerOctetString(new DerOctetString(new byte[] { 1, 3, 6, 1, 5, 5, 7, 48, 1, 1 }));

        values.Add(OcspObjectIdentifiers.PkixOcsp, new X509Extension(false, asn1));
        ocspRequestGenerator.SetRequestExtensions(new X509Extensions(oids, values));

        return ocspRequestGenerator.Generate();
    }

    public JObject x509ChainVerify(X509Certificate2 certificate)
    {
        JObject Error = new JObject();
        X509Chain x509Chain = new X509Chain();
        x509Chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
        x509Chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
        x509Chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
        x509Chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
        x509Chain.Build(certificate);

        int certNumber = 0;
        foreach (X509ChainElement element in x509Chain.ChainElements)
        {
            certNumber = certNumber + 1;
            JObject CertError = new JObject();
            bool isValid = element.Certificate.Verify();
            if (element.Certificate.Verify().Equals(false))
            {
                CertError.Add("Issuer", element.Certificate.Issuer.ToString());
                CertError.Add("ValidUntil", element.Certificate.NotAfter.ToString());
                CertError.Add("SerialNumber", element.Certificate.SerialNumber.ToString());
                CertError.Add("Thumbprint", element.Certificate.Thumbprint.ToString());
                CertError.Add("IsValid", isValid.ToString());
                Error.Add($"CertNumber({certNumber})", CertError);
            }

            if (x509Chain.ChainStatus.Length > 0)
            {
                for (int index = 0; index < element.ChainElementStatus.Length; index++)
                {
                    JObject ChainElementStatus = new JObject();
                    ChainElementStatus.Add("Status", element.ChainElementStatus[index].Status.ToString());
                    ChainElementStatus.Add("StatusInformation", element.ChainElementStatus[index].StatusInformation.ToString());
                    CertError.Add($"ChainElementStatus({index})", ChainElementStatus);
                }
            }
        }
        return Error;
    }
}