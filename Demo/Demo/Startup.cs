﻿using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Helper.Services;

[assembly: OwinStartup(typeof(Demo.Startup))]

namespace Demo
{
    /// <summary>
    /// Configuraciones pertenecientes a Microsoft Owin utilizado para 
    /// server web embebido en aplicacion principal.
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            HttpConfiguration config = new HttpConfiguration();
            FileServerOptions options = new FileServerOptions();

            options.EnableDirectoryBrowsing = true;
            // Habilitar mostrar pagina web alojada en Site de la aplicacion principal
            //options.FileSystem = new PhysicalFileSystem("./Site"); 
            options.StaticFileOptions.ServeUnknownFileTypes = true;

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { id = RouteParameter.Optional },
                constraints: new { id = "\\d+" }
            );

            config.MapHttpAttributeRoutes();
            appBuilder.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            appBuilder.UseFileServer(options);
            appBuilder.UseWebApi(config);

        }
    }
}
