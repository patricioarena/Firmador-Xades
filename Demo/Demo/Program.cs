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
using System.Configuration;

namespace Demo
{
    static class Program
    {
        private static int port = Int32.Parse(ConfigurationManager.AppSettings["WebApp.Port"]);
        private static string baseUrl = ConfigurationManager.AppSettings["WebApp.BaseUrl"];

        public static string AssemblyGuidString(Assembly assembly)
        {
            object[] objects = assembly.GetCustomAttributes(typeof(GuidAttribute), false);
            if (objects.Length > 0)
                return ((GuidAttribute)objects[0]).Value;
            return String.Empty;
        }

        public static int ExecuteCertUtilCustom()
        {
            int exitCode;
            string arguments = Program.AssemblyGuidString(typeof(Program).Assembly) + " " + Program.port;
            string assamblyName = Assembly.GetExecutingAssembly().GetName().Name.ToString()+".exe";
            var dir = Assembly.GetExecutingAssembly().Location.Replace(assamblyName, "") + "CertUtilCustom.exe";
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
            return exitCode;
        }

        public static void Start()
        {
            WebApp.Start<Startup>($"{Program.baseUrl}:{Program.port}/");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Signature());
        }

        [STAThread]
        static void Main()
        {
            bool alreadyRunning = false;
            using (Mutex mutex = new Mutex(true, Assembly.GetExecutingAssembly().GetName().Name.ToString(), out alreadyRunning))
            {
                if (alreadyRunning)
                {
                    DualCheck dual = CertificateControl.CheckStores();

                    if (dual.My.Equals(false) || dual.Root.Equals(false))
                    {
                        WindowsIdentity wi = WindowsIdentity.GetCurrent();
                        WindowsPrincipal wp = new WindowsPrincipal(wi);

                        if (!wp.IsInRole(WindowsBuiltInRole.Administrator))
                        {
                            Program.ExecuteCertUtilCustom();
                        }
                        else
                        {
                            Program.ExecuteCertUtilCustom();
                        }
                        Program.Start();
                    }
                    else
                    {
                        Program.Start();
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