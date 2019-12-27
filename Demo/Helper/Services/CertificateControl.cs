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
using Helper.Model;

namespace Helper.Services
{

    /// <summary>
    /// Verificar la existencia del certificado en el equipo del usuario
    /// </summary>
    public class CertificateControl
    {
        private static StoreName storeRoot = StoreName.Root;
        private static StoreName storeMy = StoreName.My;
        private static StoreLocation storeLocation = StoreLocation.LocalMachine;
        private static string CertificateName = "Web Service Digital Signature";

        public static DualCheck CheckStores()
        {
            DualCheck dualCheck = new DualCheck();
            X509Store CollectionRoot = new X509Store(storeRoot, storeLocation);
            X509Store CollectionMy = new X509Store(storeMy, storeLocation);

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






    }
}