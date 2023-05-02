using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureblobStorageDemoApi.Constants;
using AzureblobStorageDemoApi.Models;
using AzureblobStorageDemoApi.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace AzureblobStorageDemoApi.Services;

public class AzureBlobStorageDemoService : IAzureBlobStorageDemoService
{
    private readonly BlobServiceClient _serviceClient;
    private readonly IMemoryCache _cache;
    private readonly MemoryCacheEntryOptions _cacheOptions;

    public AzureBlobStorageDemoService(IOptions<BlobStorageOptions> options, IMemoryCache cache)
    {
        _cache = cache;
        _cacheOptions = new MemoryCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(10),
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
        };
        _serviceClient = new BlobServiceClient(options.Value.Endpoint);
    }

    public async Task<StorageAccountData> GetStorageAccountData()
    {
        try
        {
            if (_cache.TryGetValue(StringConstants.AccountDataCacheKey, out StorageAccountData accountData))
            {
                return accountData;
            }

            var accountInfo = (await _serviceClient.GetAccountInfoAsync()).Value;
            accountData = new StorageAccountData
            {
                AccountName = _serviceClient.AccountName,
                Uri = _serviceClient.Uri.ToString(),
                Sku = accountInfo.SkuName.ToString(),
                AccountKind = accountInfo.AccountKind.ToString()
            };

            _cache.Set(StringConstants.AccountDataCacheKey, accountData, _cacheOptions);

            return accountData;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<ContainerData>> GetAccountContainers()
    {
        try
        {
            if (_cache.TryGetValue(StringConstants.AccountContainersCacheKey, out List<ContainerData> containers))
            {
                return containers;
            }

            containers = new List<ContainerData>();
            var resultSegment = _serviceClient.GetBlobContainersAsync(BlobContainerTraits.Metadata, "", default)
                .AsPages();
            await foreach (var containerPage in resultSegment)
            {
                foreach (var containerItem in containerPage.Values)
                {
                    var container = new ContainerData
                    {
                        Name = containerItem.Name
                    };


                    container.Metadata = containerItem.Properties.Metadata != null ? new Dictionary<string, string>(containerItem.Properties.Metadata) : new Dictionary<string, string>();

                    containers.Add(container);
                }
            }

            _cache.Set(StringConstants.AccountContainersCacheKey, containers, _cacheOptions);

            return containers;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ContainerData> CreateContainer(string containerName)
    {
        try
        {
            var container = _serviceClient.GetBlobContainerClient(containerName);

            if (!await container.ExistsAsync())
            {
                await container.CreateAsync();
            }

            var properties = (await container.GetPropertiesAsync()).Value;
            
            _cache.Remove(StringConstants.AccountContainersCacheKey);

            return new ContainerData
            {
                Name = container.Name,
                Uri = container.Uri.ToString(),
                Metadata = new Dictionary<string, string>(properties.Metadata)
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}