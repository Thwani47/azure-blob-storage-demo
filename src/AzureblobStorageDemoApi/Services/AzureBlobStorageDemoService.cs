using Azure.Storage.Blobs;
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
}