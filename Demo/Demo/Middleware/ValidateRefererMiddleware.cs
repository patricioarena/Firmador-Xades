using Demo.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Demo.Middleware;

public class ValidateRefererMiddleware
{
    private const string Referer = "Referer";

    private const string Forbidden = "Forbidden";

    private const string Whitelist = "\\whitelist.txt";

    private readonly RequestDelegate next;

    public ValidateRefererMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var referer = context.Request.Headers[Referer].ToString();

        if (ObjectsExtension.IsEmpty(referer))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync(Forbidden);
            return;
        }

        var resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), "ResourceData");
        if (!Directory.Exists(resourcesPath))
        {
            Directory.CreateDirectory(resourcesPath);
        }

        var result = Handle(resourcesPath, referer);
        if (result)
        {
            await next(context);
        }

        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync(Forbidden);
        return;
    }

    private bool Handle(string resourcesPath, string referer)
    {
        if (ExistsWhitelist(resourcesPath))
        {
            if (IsEmpty(resourcesPath))
            {
                //TODO: Descargar la whitelist y reemplazarla por la descargada
                ReWriteWhitelist(resourcesPath, referer);
            }
            else
            {
                return ExistsRegister(resourcesPath, referer);
            }
        }
        else
        {
            //TODO: Descargar la whitelist y guardarla en la carpeta
            MockDownload(resourcesPath);
        }

        return false;
    }

    private bool ReWriteWhitelist(string resourcesPath, string referer) => throw new NotImplementedException();

    private bool IsEmpty(string resourcesPath) => new FileInfo(resourcesPath + Whitelist).Length == 0;

    private bool ExistsWhitelist(string resourcesPath) => File.Exists(resourcesPath + Whitelist);

    private bool ExistsRegister(string resourcesPath, string entry)
    {
        foreach (string line in File.ReadLines(resourcesPath + Whitelist))
        {
            if (line.Trim() == entry)
            {
                Console.WriteLine("El registro existe en el archivo.");
                return true;
            }
        }

        Console.WriteLine("El registro no existe en el archivo.");
        return false;
    }

    private static void MockDownload(string resourcesPath)
    {
        File.Create(resourcesPath + Whitelist).Dispose();
        string[] registros =
        [
            "web-ar.com",
            "web-ar.com.ar",
            "arba.com",
            "other.com"
        ];
        File.WriteAllLines(resourcesPath + Whitelist, registros);
    }
}