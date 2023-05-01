using AzureblobStorageDemoApi.Models;

namespace AzureblobStorageDemoApi.Services;

public interface IAzureBlobStorageDemoService
{
    public Task<StorageAccountData> GetStorageAccountData();
}