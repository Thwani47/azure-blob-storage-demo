using AzureblobStorageDemoApi.Extensions;
using AzureblobStorageDemoApi.Services;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
builder.Configuration.SetBasePath(env.ContentRootPath);
builder.Configuration.AddJsonFile("appsettings.json", true, true);
builder.Configuration.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddAzureBlobServiceCollection(builder.Configuration);
builder.Services.AddCors();
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

app.UseCors(builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });


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
}).WithName("GetAccountData");

app.MapGet("/account/containers", async (IAzureBlobStorageDemoService service) =>
{
    try
    {
        var data = await service.GetAccountContainers();
        return Results.Ok(data);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        return Results.StatusCode(500);
    }
}).WithName("GetContainers");

app.MapPost("account/container/{name}", async (string name, IAzureBlobStorageDemoService service) =>
{
    try
    {
        var response = await service.CreateContainer(name);
        return Results.Created(response.Uri, response);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}).WithName("AddContainer");

app.Run();