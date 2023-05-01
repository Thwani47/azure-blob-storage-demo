namespace AzureblobStorageDemoApi.Models;

public class ContainerData
{
    public string Name { get; set; }
    public string Uri { get; set; }
    public Dictionary<string, string> Metadata { get; set; }
}