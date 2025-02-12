using Demo.Properties;
using Demo.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Usecases
{
    /// <summary>
    /// Verificar la existencia del certificado en el equipo del usuario
    /// </summary>
    public static class IsInstalledCertificate
    {
        private static readonly string CertificateName = Settings.Default.CertificateName;

        public static async Task<bool> TestAsync()
        {
            var storeTasks = CertificateUtils.GetDefaultStores()
                .Select(CertificateUtils.ExistInStoreAsync);

            var results = await Task.WhenAll(storeTasks);

            // Devuelve true si todos los resultados son true, de lo contrario, false
            return !results.Contains(false);
        }
    }
}