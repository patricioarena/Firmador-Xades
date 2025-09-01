using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Extensions;
using Helper.Results;
using Microsoft.AspNetCore.Mvc;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Demo.Middleware;
public class ValidateRefererMiddleware
{
    private const string Referer = "Referer";

    private const string Forbidden = "Forbidden";
    private const string whitelist = "\\whitelist.txt";
    private readonly RequestDelegate _next;

    public ValidateRefererMiddleware(RequestDelegate next)
    {
        _next = next;
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

        var result = handle(resourcesPath, referer);
        if (result)
        {
            await _next(context);
        }

        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync(Forbidden);
        return;
    }

    private bool handle(string resourcesPath, string referer)
    {
        if (ExistsWhitelist(resourcesPath))
        {
            if (IsEmpty(resourcesPath))
            {
                //TODO: Descargar la whitelist y reemplazarla por la descargada
                reWriteWhitelist(resourcesPath, referer);
            }
            else
            {
                return ExistsRegister(resourcesPath, referer);
            }
        }
        else
        {
            //TODO: Descargar la whitelist y guardarla en la carpeta
            mockDownload(resourcesPath);
        }
        return false;
    }

    private bool reWriteWhitelist(string resourcesPath, string referer) => throw new NotImplementedException();

    private bool IsEmpty(string resourcesPath) => new FileInfo(resourcesPath + whitelist).Length == 0;

    private bool ExistsWhitelist(string resourcesPath) => File.Exists(resourcesPath + whitelist);

    private bool ExistsRegister(string resourcesPath, string entry)
    {
        foreach (string line in File.ReadLines(resourcesPath + whitelist))
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

    private static void mockDownload(string resourcesPath)
    {
        File.Create(resourcesPath + whitelist).Dispose();
        string[] registros = new string[]
        {
                "web-ar.com",
                "web-ar.com.ar",
                "arba.com",
                "other.com"
        };
        File.WriteAllLines(resourcesPath + whitelist, registros);
    }
}