using Demo.Services;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
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


            int port = 8400;
            SelfSignedCertificateService.Port = port;
            SelfSignedCertificateService.Init();

            WebApp.Start<Startup>($"https://localhost:{port}/");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Configuración());
        }
    }
}

//Error al agregar el certificado SSL.Error: 183 -> No se puede crear un archivo que ya existe.
//Error al agregar el certificado SSL.Error: 1312 -> Installar el certificado en el asistente como "Equipo local" y en "Entidades de certificacion raiz de confianza"  y en "Personal":
//netsh http add sslcert ipport=0.0.0.0:8088 certhash=4d5da0a8d8784b6146a1b3059667d1580eea9616 appid = "{6EA4FE96-C6B1-41A3-B3FD-F9CE949569DF}"