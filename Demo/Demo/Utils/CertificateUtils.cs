using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Demo.Extensions;

namespace Demo.Utils
{
    public static class CertificateUtils
    {
        private static readonly string CertificateKey = Properties.Settings.Default.CertificateKey;

        private static readonly string CertificateName = Properties.Settings.Default.CertificateName;

        private static readonly string AuthenticationServer = Properties.Settings.Default.AuthenticationServer;

        private static readonly string DnsName = Properties.Settings.Default.DnsName;

        private static readonly string Issuer = Properties.Settings.Default.OntiIssuer;

        /// <summary>
        /// Crear un Certificado autofirmado para SSL y almacenarlo en el escritorio.
        /// El certificado es de extencion pfx.
        /// </summary>
        public static void BuildSelfSignedServerCertificate()
        {
            SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName(DnsName);
            sanBuilder.AddDnsName(Environment.MachineName);

            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN={CertificateName}");

            using (RSA rsa = RSA.Create(4096))
            {
                var request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

                request.CertificateExtensions.Add(
                    new X509KeyUsageExtension(X509KeyUsageFlags.DataEncipherment | X509KeyUsageFlags.KeyEncipherment | X509KeyUsageFlags.DigitalSignature, false));

                request.CertificateExtensions.Add(
                    new X509EnhancedKeyUsageExtension(
                        new OidCollection { new Oid(AuthenticationServer) }, false));

                request.CertificateExtensions.Add(sanBuilder.Build());

                var certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));
                certificate.FriendlyName = CertificateName;

                // Exportar el certificado como PFX
                byte[] pfxBytes = certificate.Export(X509ContentType.Pfx, CertificateKey);

                // Guardar el archivo PFX en el escritorio
                string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{CertificateName}.pfx");
                File.WriteAllBytes(desktopPath, pfxBytes);
            }
        }

        public static X509Certificate2 GetDefaultCertificate()
        {
            byte[] certificateBytes = Properties.Resources.Authentica;
            X509Certificate2 certificate = ConvertToX509Certificate2(certificateBytes, CertificateKey);
            return certificate;
        }

        public static X509Certificate2 GetCertificateWithoutPrivateKey()
        {
            byte[] certificateBytes = Encoding.UTF8.GetBytes(Properties.Resources.Authentica_cer);
            X509Certificate2 certificate = new X509Certificate2(certificateBytes, CertificateKey);
            return certificate;
        }

        private static X509Certificate2 ConvertToX509Certificate2(byte[] certificateBytes, string password)
        {
            return new X509Certificate2(certificateBytes, password);
        }

        public static List<X509Store> GetDefaultStores() =>
        [
            new(StoreName.Root, StoreLocation.CurrentUser),
            new(StoreName.My, StoreLocation.CurrentUser)
        ];

        private static X509Certificate2 GetCertificateOfStore(X509Store store)
        {
            X509Certificate2 defaultCert = GetDefaultCertificate();
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            return collection
                .OfType<X509Certificate2>().FirstOrDefault(cert => Equals(cert.Thumbprint, defaultCert.Thumbprint));
        }

        public static async Task<bool> ExistInStoreAsync(X509Store store) =>
            await Task.Run(() => !GetCertificateOfStore(store).IsNull());

        /// <summary>
        /// Selecciona un certificado del almacén de certificados
        /// </summary>
        /// <returns></returns>
        public static X509Certificate2 SelectCertificate(string message = null, string title = null, bool onlyFirmaDigital = true)
        {
            X509Certificate2 cert = null;

            try
            {
                // Open the store of personal certificates.
                X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);

                if (Program._ontiChecked)
                    fcollection = filterBySerialNumber(fcollection);

                if (string.IsNullOrEmpty(message))
                {
                    message = "Seleccione un certificado.";
                }

                if (string.IsNullOrEmpty(title))
                {
                    title = "Firmar archivo";
                }
                //A X509Certificate2UI window always on top
                IntPtr windowHandle = Process.GetCurrentProcess().MainWindowHandle;
                X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(fcollection, title, message, X509SelectionFlag.SingleSelection, windowHandle);

                if (scollection != null && scollection.Count == 1)
                {
                    cert = scollection[0];

                    if (cert.HasPrivateKey == false)
                    {
                        throw new Exception("El certificado no tiene asociada una clave privada.");
                    }
                }

                store.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("No se ha podido obtener la clave privada.", ex);
            }

            return cert;
        }

        private static X509Certificate2Collection filterBySerialNumber(X509Certificate2Collection installedCertificatesCollection)
        {
            //string issuer = "30680604572";
            string issuer = Issuer;
            X509Certificate2Collection ontiCertificates = new X509Certificate2Collection();

            foreach (X509Certificate2 currentCert in installedCertificatesCollection)
            {
                if (currentCert.Issuer.Contains(issuer, StringComparison.OrdinalIgnoreCase))
                {
                    ontiCertificates.Add(currentCert);
                }
            }
            return ontiCertificates;
        }
    }
}
