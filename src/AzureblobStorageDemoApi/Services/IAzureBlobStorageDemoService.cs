using AzureblobStorageDemoApi.Models;

namespace AzureblobStorageDemoApi.Services;

public interface IAzureBlobStorageDemoService
{
    public Task<StorageAccountData> GetStorageAccountData();
    public Task<List<ContainerData>> GetAccountContainers();
    public Task<ContainerData> CreateContainer(string containerName);
}