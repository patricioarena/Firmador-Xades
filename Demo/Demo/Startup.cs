using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;
using Autofac;
using Autofac.Integration.WebApi;
using Helper.Services;
using Demo.Handlers;
using System.Reflection;

[assembly: OwinStartup(typeof(Demo.Startup))]

namespace Demo
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configuración de Autofac
            var builder = new ContainerBuilder();

            // Registra los controladores de Web API
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Registra tus dependencias aquí
            builder.RegisterType<CoreHandler>().As<ICoreHandler>();
            builder.RegisterType<DecisionHandler>().As<IDecisionHandler>();
            builder.RegisterType<VerificationHandler>().As<IVerificationHandler>();

            var container = builder.Build();

            // Configura Autofac para Web API
            var config = new HttpConfiguration();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            // Resto de tu configuración de Owin
            FileServerOptions options = new FileServerOptions();
            options.EnableDirectoryBrowsing = true;
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
