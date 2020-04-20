using Helper.Model;
using Helper.Services;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo
{
    static class Program
    {
        private static string AssemblyGuidString(Assembly assembly)
        {
            object[] objects = assembly.GetCustomAttributes(typeof(GuidAttribute), false);
            if (objects.Length > 0)
                return ((GuidAttribute)objects[0]).Value;
            return String.Empty;
        }

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
                        WindowsIdentity wi = WindowsIdentity.GetCurrent();
                        WindowsPrincipal wp = new WindowsPrincipal(wi);

                        string assemblyGuid = AssemblyGuidString(typeof(Program).Assembly);
                        string arguments = assemblyGuid + " " + port;
                        bool runAsAdmin = wp.IsInRole(WindowsBuiltInRole.Administrator);
                        int exitCode;

                        if (!runAsAdmin)
                        {
                            var dir = Assembly.GetExecutingAssembly().Location.Replace("CertiFisc.exe", "") + "CertUtilCustom.exe";
                            using (Process process = new Process())
                            {
                                process.StartInfo.Arguments = arguments;
                                process.StartInfo.FileName = dir;
                                process.StartInfo.UseShellExecute = true;
                                process.StartInfo.CreateNoWindow = true;
                                process.StartInfo.Verb = "runas";
                                process.Start();
                                process.WaitForExit();
                                exitCode = process.ExitCode;

                            }
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