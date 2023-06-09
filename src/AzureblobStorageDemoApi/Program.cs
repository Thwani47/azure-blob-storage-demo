using AzureblobStorageDemoApi.Extensions;
using AzureblobStorageDemoApi.Models;
using AzureblobStorageDemoApi.Services;
using Microsoft.AspNetCore.Mvc;

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

app.UseCors(policyBuilder => { policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });


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

app.MapPost("account/container/{name}/metadata",
    async (string name, [FromBody] MetadataInput metadata, IAzureBlobStorageDemoService service) =>
    {
        try
        {
            await service.AddContainerMetadata(name, metadata);
            return Results.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }).WithName("AddContainerMetadata");

app.MapGet("account/container/{name}", async (string name, IAzureBlobStorageDemoService service) =>
{
    try
    {
        var response = await service.GetContainerData(name);
        return Results.Ok(response);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}).WithName("GetContainer");

app.MapDelete("account/container/{name}", async (string name, IAzureBlobStorageDemoService service) =>
{
    try
    {
        await service.DeleteContainer(name);
        return Results.Ok();
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        throw;
    }
}).WithName("DeleteContainer");

app.Run();