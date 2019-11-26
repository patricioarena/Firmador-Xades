using CertUtilCustom.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CertUtilCustom
{
    class Program
    {
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void Main(string[] args)
        {
            IntPtr hWnd = GetConsoleWindow();
            ShowWindow(hWnd, 0);
            SelfSignedCertificateService.Port = 8400;
            SelfSignedCertificateService.Init();
        }
    }
}
