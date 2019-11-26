using Demo.Model;
using Demo.Services;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
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
                    DualCheck dual = CertificateControl.CheckStores();

                    if (dual.My.Equals(false) || dual.Root.Equals(false))
                    {
                        var wi = WindowsIdentity.GetCurrent();
                        var wp = new WindowsPrincipal(wi);

                        bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);

                        if (!runAsAdmin)
                        {
                            // No es posible iniciar una aplicación ClickOnce como administrador directamente,
                            // así que en su lugar, iniciamos la aplicación como administrador en un nuevo proceso.
                            var dir = Assembly.GetExecutingAssembly().Location.Replace("Demo.exe", "") + "CertUtilCustom.exe";
                            var processInfo = new ProcessStartInfo(dir);
                            processInfo.UseShellExecute = true;
                            processInfo.Verb = "runas";

                            Process.Start(processInfo);
                        }

                        WebApp.Start<Startup>($"https://localhost:{port}/");
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Signature());
                    }
                    else
                    {
                        WebApp.Start<Startup>($"https://localhost:{port}/");
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new Signature());
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