using Demo.Utils;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Demo.UseCases
{
    public static class InstallCertificate
    {
        public static async Task ExecuteAsync()
        {
            var certificate = CertificateUtils.GetCertificateWithoutPrivateKey();
            var stores = CertificateUtils.GetDefaultStores();

            // Ejecuta las instalaciones de certificados en paralelo
            var installTasks = new List<Task>();
            foreach (var store in stores)
            {
                installTasks.Add(InstallAsync(store, certificate));
            }

            // Espera a que todas las tareas de instalación terminen
            await Task.WhenAll(installTasks);
        }

        private static async Task InstallAsync(X509Store store, X509Certificate2 certificate)
        {
            try
            {
                await Task.Yield(); // Asegura que se ceda el control a otros procesos
                using var x509Store = store;
                x509Store.Open(OpenFlags.ReadWrite);
                x509Store.Add(certificate);
                x509Store.Close();
            }
            catch (CryptographicException ex)
            {
                // Manejar específicamente el error de criptografía
                Console.WriteLine($"Error al instalar el certificado: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                // Manejar el acceso no autorizado
                Console.WriteLine($"Acceso denegado: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Manejo general de errores
                Console.WriteLine($"Error inesperado: {ex.Message}");
            }
        }
    }
}
