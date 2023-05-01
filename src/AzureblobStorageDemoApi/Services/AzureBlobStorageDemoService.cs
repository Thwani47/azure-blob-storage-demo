using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using AzureblobStorageDemoApi.Models;
using AzureblobStorageDemoApi.Options;
using Microsoft.Extensions.Options;

namespace AzureblobStorageDemoApi.Services;

public class AzureBlobStorageDemoService : IAzureBlobStorageDemoService
{
    private readonly BlobServiceClient _serviceClient;

    public AzureBlobStorageDemoService(IOptions<BlobStorageOptions> options)
    {
        _serviceClient = new BlobServiceClient(options.Value.Endpoint);
    }

    public async Task<StorageAccountData> GetStorageAccountData()
    {
        try
        {
            var accountInfo = (await _serviceClient.GetAccountInfoAsync()).Value;
            return new StorageAccountData
            {
                AccountName = _serviceClient.AccountName,
                Uri = _serviceClient.Uri.ToString(),
                Sku = accountInfo.SkuName.ToString(),
                AccountKind = accountInfo.AccountKind.ToString()
            };
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
            var containers = new List<ContainerData>();
            var containerClient = _serviceClient.GetBlobContainersAsync();
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
                    
                    
                    if (containerItem.Properties.Metadata != null)
                    {
                        container.Metadata = new Dictionary<string, string>(containerItem.Properties.Metadata);
                    }
                    else
                    {
                        container.Metadata = new Dictionary<string, string>();
                    }
                    containers.Add(container);
                }
            }

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