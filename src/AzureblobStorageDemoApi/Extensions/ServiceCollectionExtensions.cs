using AzureblobStorageDemoApi.Options;
using AzureblobStorageDemoApi.Services;

namespace AzureblobStorageDemoApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureBlobServiceCollection(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMemoryCache();
        services.Configure<BlobStorageOptions>(configuration.GetSection("AzureBlobStorage"));
        services.AddSingleton<IAzureBlobStorageDemoService, AzureBlobStorageDemoService>();
        return services;
    }
}