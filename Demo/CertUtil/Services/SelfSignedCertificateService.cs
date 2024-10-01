using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Collections.Generic;
using CertUtilCustom.Model;
using System.Configuration;

namespace CertUtilCustom.Services
{
    /// <summary>
    /// Creacion, instalacion de certificado autofirmado y seteo de certificado 
    /// a puerto especifico para utilizacion de https.
    /// </summary>
    public static class SelfSignedCertificateService
    {
        private static StoreName StoreRoot = StoreName.Root;
        private static StoreName StoreMy = StoreName.My;
        private static StoreLocation StoreLocation = StoreLocation.CurrentUser;
        private static string CertificateName = Properties.Settings.Default.CertificateName;
        private static string Password = RandomString();
        private static string AuthenticationServer = Properties.Settings.Default.AuthenticationServer;
        private static string DataFolder = Properties.Settings.Default.DataFolder;
        private static int KeyLength = 4096;
        private static X509Certificate2 x509Certificate = null;
        private static string Settings = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"{DataFolder}\\Settings.json");
        public static string assemblyGuid = null;
        public static int? Port = null;

        /// <summary>
        /// Crear archivo en AppData\Roaming\Web Service Digital Signature
        /// El archivo sirve para hacer un control sobre el certificado.
        /// </summary>
        public static void SetConfig()
        {
            if (!File.Exists(Settings))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Settings));
                using (StreamWriter writer = File.CreateText(Settings))
                {
                    writer.WriteLine($"Thumbprint:{x509Certificate.Thumbprint}");
                }
            }
            else
            {
                using (StreamWriter writer = File.CreateText(Settings))
                {
                    writer.WriteLine($"Thumbprint:{x509Certificate.Thumbprint}");
                }
            }

        }

        public static bool? Compare_x509_Thumbprint_With_Config(string Thumbprint)
        {
            if (File.Exists(Settings))
            {
                using (StreamReader reader = File.OpenText(Settings))
                {
                    string line = String.Empty;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] splitted = line.Split(':');
                        string key = splitted[0].Trim();
                        string value = splitted[1].Trim();
                        return value.Equals(Thumbprint) ? true : false;
                    }
                }
            }
            SetConfig();
            return null;
        }

        public static void Init()
        {
            DualCheck dualCheck = CheckStores();
            if (dualCheck.Root.Equals(false) && dualCheck.My.Equals(false))
            {
                x509Certificate = buildSelfSignedServerCertificate();
                InstallCertificate();
                int codeSet = SetSSLCertificate();
                int? codeUnset = null;
                if (codeSet.Equals(1))
                {
                    codeUnset = UnSetSSLCertificate();
                    codeSet = SetSSLCertificate();
                    SetConfig();
                }
                SetConfig();
            }
            else if (dualCheck.Root.Equals(true) && dualCheck.My.Equals(true))
            {
                bool isEqual = ThumbprintIsEqual().Item1;
                string Thumbprint = ThumbprintIsEqual().Item2;
                if (isEqual && !Thumbprint.Equals(null))
                {
                    bool? configFlag = Compare_x509_Thumbprint_With_Config(Thumbprint);
                    if (configFlag.Equals(false))
                    {
                        int codeSet = SetSSLCertificate();
                        int? codeUnset = null;
                        if (codeSet.Equals(1))
                        {
                            codeUnset = UnSetSSLCertificate();
                            codeSet = SetSSLCertificate();
                            SetConfig();
                        }
                    }
                }
            }
            else
            {
                List<StoreName> StoreCollection = GetPropertyValues(dualCheck);
                foreach (StoreName storeName in StoreCollection)
                {
                    X509Store store = new X509Store(storeName, StoreLocation);
                    X509Certificate2 x509Certificate_old = GetCertificateStore(store);
                    UninstallCertificate(storeName, x509Certificate_old);
                }
                int codeUnset = UnSetSSLCertificate();
                Init();
            }
        }

        private static (bool, string) ThumbprintIsEqual()
        {
            X509Store storeR = new X509Store(StoreRoot, StoreLocation);
            X509Store storeM = new X509Store(StoreMy, StoreLocation);
            X509Certificate2 x509_R = GetCertificateStore(storeR);
            X509Certificate2 x509_M = GetCertificateStore(storeM);

            if (x509_R.Thumbprint.Equals(x509_M.Thumbprint))
            {
                x509Certificate = x509_R;
                return (true, x509_R.Thumbprint);
            }
            return (false, null);
        }

        private static List<StoreName> GetPropertyValues(Object obj)
        {
            List<StoreName> list = new List<StoreName>();
            Type t = obj.GetType();
            PropertyInfo[] props = t.GetProperties();
            foreach (var prop in props)
            {
                if (prop.GetValue(obj).Equals(true))
                {
                    StoreName enumName = (StoreName)Enum.Parse(typeof(StoreName), prop.Name);
                    list.Add(enumName);
                }
            }
            return list;
        }

        private static string RandomString()
        {
            const int length = 20;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

        private static DualCheck CheckStores()
        {
            DualCheck dualCheck = new DualCheck();
            X509Store CollectionRoot = new X509Store(StoreRoot, StoreLocation);
            X509Store CollectionMy = new X509Store(StoreMy, StoreLocation);

            if (CheckStores(CollectionRoot).Equals(false))
            {
                if (CheckStores(CollectionMy).Equals(false))
                {
                    dualCheck.Root = false;
                    dualCheck.My = false;
                    return dualCheck;
                }
                dualCheck.Root = false;
                dualCheck.My = true;
                return dualCheck;
            }
            else if (CheckStores(CollectionMy).Equals(false))
            {
                dualCheck.Root = true;
                dualCheck.My = false;
                return dualCheck;
            }
            dualCheck.Root = true;
            dualCheck.My = true;
            return dualCheck;
        }

        private static bool CheckStores(X509Store store)
        {
            X509Certificate2 myCertificate = GetCertificateStore(store);
            if (myCertificate != null)
                return true;
            return false;
        }

        private static X509Certificate2 GetCertificateStore(X509Store store)
        {
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
            return collection.OfType<X509Certificate2>().Where(e => e.Subject == $"CN={CertificateName}").FirstOrDefault();
        }

        private static void InstallCertificate()
        {
            X509Store CollectionRoot = new X509Store(StoreRoot, StoreLocation);
            X509Store CollectionMy = new X509Store(StoreMy, StoreLocation);
            InstallCertificate(StoreRoot, x509Certificate);
            InstallCertificate(StoreMy, x509Certificate);
        }

        private static void InstallCertificate(StoreName storeName, X509Certificate2 certi)
        {
            try
            {
                X509Store store = new X509Store(storeName, StoreLocation);
                X509Certificate2 cert = certi;
                store.Open(OpenFlags.ReadWrite);
                store.Add(cert);
                store.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Install Certificate in {storeName}: {ex.StackTrace}");
            }
        }

        public static void UninstallCertificate(StoreName storeName, X509Certificate2 certi)
        {
            try
            {
                X509Store store = new X509Store(storeName, StoreLocation);
                X509Certificate2 cert = certi;
                store.Open(OpenFlags.ReadWrite);
                store.Remove(cert);
                store.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Uninstall Certificate in {storeName}: {ex.StackTrace}");
            }
        }

        private static X509Certificate2 buildSelfSignedServerCertificate()
        {
            SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName("localhost");
            sanBuilder.AddDnsName(Environment.MachineName);

            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN={CertificateName}");

            using (RSA rsa = RSA.Create(KeyLength))
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

                return new X509Certificate2(certificate.Export(X509ContentType.Pfx, Password), Password, X509KeyStorageFlags.MachineKeySet);
            }
        }

        private static int SetSSLCertificate()
        {
            using (Process process = new Process())
            {
                process.StartInfo.Arguments = "http add sslcert ipport=0.0.0.0:" + Port + " certhash=" + x509Certificate.Thumbprint + " appid = \"{" + assemblyGuid + "}\" ";
                process.StartInfo.FileName = "netsh.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                return process.ExitCode;
            }
        }

        private static int UnSetSSLCertificate()
        {
            using (Process process = new Process())
            {
                process.StartInfo.Arguments = $"http delete sslcert ipport=0.0.0.0:{Port}";
                process.StartInfo.FileName = "netsh.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
                return process.ExitCode;
            }
        }

        #region Extras
        /// <summary>
        /// Obtener identificador de propio ensamblado
        /// </summary>
        private static string AssemblyGuidString(Assembly assembly)
        {
            object[] objects = assembly.GetCustomAttributes(typeof(GuidAttribute), false);
            if (objects.Length > 0)
                return ((GuidAttribute)objects[0]).Value;
            return String.Empty;
        }

        /// <summary>
        /// Ver datos de un certificado
        /// </summary>
        public static void ViewDataCertificate()
        {
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
            Console.WriteLine("Friendly Name: {0}{1}", x509Certificate.FriendlyName, Environment.NewLine);
            Console.WriteLine("Name: {0}{1}", x509Certificate.GetName(), Environment.NewLine);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos
            Console.WriteLine("Simple Name: {0}{1}", x509Certificate.GetNameInfo(X509NameType.SimpleName, true), Environment.NewLine);
            Console.WriteLine("Thumbprint: {0}{1}", x509Certificate.Thumbprint, Environment.NewLine);
            Console.WriteLine("SerialNumber: {0}{1}", x509Certificate.SerialNumber, Environment.NewLine);
            Console.WriteLine("Signature Algorithm: {0}{1}", x509Certificate.SignatureAlgorithm.FriendlyName, Environment.NewLine);
            Console.WriteLine("Certificate Verified?: {0}{1}", x509Certificate.Verify(), Environment.NewLine);
            Console.WriteLine("Length of Raw Data: {0}{1}", x509Certificate.RawData.Length, Environment.NewLine);

            //Name: CN=firmador.test, OU=DESKTOP-ON7GE0B\ay_al@DESKTOP-ON7GE0B (Patricio Arena), O=mkcert development certificate
            //Simple Name: mkcert DESKTOP-ON7GE0B\ay_al@DESKTOP-ON7GE0B (Patricio Arena)
            //Thumbprint: 4D0357D0BD54BE2544B6F5D25DEE4C73CD11466F
            //SerialNumber: 4FC14139EB6A78E135F9D587933F5DD3
            //Signature Algorithm: sha256RSA
            //Certificate Archived?: False
            //Length of Raw Data: 1195

        }

        #endregion Extras
    }
}