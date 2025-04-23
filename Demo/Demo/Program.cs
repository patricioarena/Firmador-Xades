using Autofac.Extensions.DependencyInjection;
using Demo.Extensions;
using Demo.Properties;
using Demo.Usecases;
using Demo.UseCases;
using Demo.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    internal static class Program
    {
        private static readonly int Port = Settings.Default.Port;

        //public static bool _ontiChecked = (bool)Properties.Settings.Default["OnlyOnti"];

        private static void Start(string[] args)
        {
            if (StartupRegistryHelpers.IsFirstRun())
                StartupRegistryHelpers.RegisterStartupScript(true);
            
            // Inicia Kestrel en un hilo separado
            Task.Run(() =>
            {
                CreateHostBuilder(args).Build().Run();
            });
        
            // Inicia la interfaz gráfica
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(Signature.GetInstance());
        
        }

        private static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((config) =>
            {
                config.AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true);
            })
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKestrel(options =>
                {
                    options.ListenAnyIP(Port, listenOptions =>
                    {
                        listenOptions.UseHttps(CertificateUtils.GetDefaultCertificate());
                    });
                });
                webBuilder.UseStartup<Startup>();
            });

        [STAThread]
        private static async Task Main(string[] args)
        {
            using (new Mutex(true, Assembly.GetExecutingAssembly().GetName().Name, out var alreadyRunning))
            {
                if (alreadyRunning)
                {
                    //Start(args);

                    var isInstalledCertificate = await IsInstalledCertificate.TestAsync();
                    if (isInstalledCertificate.IsFalse())
                    {
                        WindowsIdentity wi = WindowsIdentity.GetCurrent();
                        WindowsPrincipal wp = new WindowsPrincipal(wi);

                        if (wp.IsInRole(WindowsBuiltInRole.Administrator).IsTrue())
                        {
                            await UninstallCertificate.ExecuteAsync();
                            await InstallCertificate.ExecuteAsync();
                            Start(args);
                        }

                        await UninstallCertificate.ExecuteAsync();
                        await InstallCertificate.ExecuteAsync();
                        Start(args);
                    }
                    else
                    {
                        Start(args);
                    }
                }
                else
                {
                    MessageBox.Show("La aplicación ya se encuentra en ejecución");
                }
            }
        }
    }
}