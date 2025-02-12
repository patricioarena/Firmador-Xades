using System.Security.Cryptography;
using iText.Signatures;

namespace FirmarPDFLibrary;

// Implementación de ExternalSignature compatible con RSA
public class ExternalSignatureImplementation : IExternalSignature
{
    private readonly RSA _rsa;
    private readonly string _hashAlgorithm;

    public ExternalSignatureImplementation(RSA rsa, string hashAlgorithm)
    {
        _rsa = rsa;
        _hashAlgorithm = hashAlgorithm;
    }

    public string GetHashAlgorithm() => _hashAlgorithm;

    public string GetEncryptionAlgorithm() => "RSA";

    public byte[] Sign(byte[] message)
    {
        return _rsa.SignData(message, new HashAlgorithmName(_hashAlgorithm), RSASignaturePadding.Pkcs1);
    }
}