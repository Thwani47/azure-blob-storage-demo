using AzureblobStorageDemoApi.Extensions;
using AzureblobStorageDemoApi.Services;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
builder.Configuration.SetBasePath(env.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json", true, true);
builder.Configuration.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddAzureBlobServiceCollection(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = builder.Environment.ApplicationName,
        Version = "v1"
    });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1"));

app.MapGet("/account", async (IAzureBlobStorageDemoService service) =>
{
    try
    {
        var data = await service.GetStorageAccountData();
        return Results.Ok(data);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.StatusCode(500);
    }
});

app.Run();
