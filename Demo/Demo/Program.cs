using Demo.Model;
using Demo.Services;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{

    #if DEBUG
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool alreadyRunning = false;

            using (Mutex mutex = new Mutex(true, Assembly.GetExecutingAssembly().GetName().Name.ToString(), out alreadyRunning))
            {
                if (alreadyRunning)
                {
                    int port = 8400;
                    SelfSignedCertificateService.Port = port;
                    SelfSignedCertificateService.Init();

                    WebApp.Start<Startup>($"https://localhost:{port}/");
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Signature());
                }
                else
                {
                    MessageBox.Show("La aplicación ya se encuentra en ejecución");
                }
            }

        }
    }
#endif
#if RELEASE
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);

            bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);

            if (!runAsAdmin)
            {
                // No es posible iniciar una aplicación ClickOnce como administrador directamente,
                // así que en su lugar, iniciamos la aplicación como administrador en un nuevo proceso.
                var processInfo = new ProcessStartInfo(Assembly.GetExecutingAssembly().CodeBase);

                // Las siguientes propiedades ejecutan el nuevo proceso como administrador
                processInfo.UseShellExecute = true;
                processInfo.Verb = "runas";

                // Comienza el nuevo proceso
                try
                {
                    Process.Start(processInfo);
                }
                catch (Exception)
                {
                }

                // Cerrar el proceso actual
                Application.Exit();
            }
            else
            {
                var alreadyRunning = false;
                using (Mutex mutex = new Mutex(true, Assembly.GetExecutingAssembly().GetName().Name.ToString(), out alreadyRunning))
                {
                    if (alreadyRunning)
                    {
                        int port = 8400;
                        SelfSignedCertificateService.Port = port;
                        SelfSignedCertificateService.Init();

                        WebApp.Start<Startup>($"https://localhost:{port}/");
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Signature());
                    }
                    else
                    {
                        MessageBox.Show("La aplicación ya se encuentra en ejecución");
                    }
                }
            }
        }
    }
#endif
}