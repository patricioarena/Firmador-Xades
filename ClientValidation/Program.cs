var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/ping", async http => await http.Response.WriteAsync("Pong"));

app.MapGet("/download", async http =>
{
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "whitelist.txt");
    if (!File.Exists(filePath))
    {
        http.Response.StatusCode = 404;
        await http.Response.WriteAsync("Archivo no encontrado.");
        return;
    }
    var fileBytes = await File.ReadAllBytesAsync(filePath);
    http.Response.ContentType = "text/plain";
    http.Response.Headers.Add("Content-Disposition", $"attachment; filename=whitelist.txt");
    await http.Response.Body.WriteAsync(fileBytes);
});

app.Run();