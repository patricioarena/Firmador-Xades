using Autofac;
using Demo.Handlers;
using Demo.Middleware;
using Demo.Properties;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Linq;

namespace Demo
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        private readonly bool isSwaggerConfigured;
        
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
            isSwaggerConfigured = this.configuration.GetSection("KestrelSettings")
                .GetChildren().Any(x => x.Key == "Swagger");
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Agrega Swagger al contenedor de servicios
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = Settings.Default.ApplicationName,  
                    Version = Settings.Default.Version,           
                    Description = "Embedded kestrel server in application", 
                });
            });

            // Configuración de CORS para aceptar cualquier dominio
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Registra tus dependencias aquí
            builder.RegisterType<CoreHandler>().As<ICoreHandler>();
            builder.RegisterType<SignatureSelectorHandler>().As<ISignatureSelectorHandler>();
            builder.RegisterType<VerificationHandler>().As<IVerificationHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            if (isSwaggerConfigured)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                });
            }
            
            app.UseHttpsRedirection();
            app.UseRouting();

            // Habilitar CORS usando la política configurada
            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseMiddleware<ValidateRefererMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
