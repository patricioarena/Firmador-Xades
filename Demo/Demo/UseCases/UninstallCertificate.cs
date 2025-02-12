using Demo.Utils;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Demo.UseCases
{
    public static class UninstallCertificate
    {
        public static async Task ExecuteAsync()
        {
            List<X509Store> stores = CertificateUtils.GetDefaultStores();

            // Ejecuta las desinstalaciones de certificados en paralelo
            var uninstallTasks = new List<Task>();
            foreach (var store in stores)
            {
                if (await CertificateUtils.ExistInStoreAsync(store))
                {
                    uninstallTasks.Add(UninstallAsync(store, CertificateUtils.GetCertificateWithoutPrivateKey()));
                }
            }

            // Espera a que todas las tareas de desinstalación terminen
            await Task.WhenAll(uninstallTasks);
        }

        private static async Task UninstallAsync(X509Store store, X509Certificate2 certificate) =>
            await Task.Run(() =>
            {
                using var x509Store = store;
                x509Store.Open(OpenFlags.ReadWrite);
                x509Store.Remove(certificate);
                x509Store.Close();
            });
    }
}
