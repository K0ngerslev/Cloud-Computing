using Microsoft.Azure.Cosmos;
using SupportWebApp.Model;

namespace SupportWebApp.Service;

// Håndterer forbindelsen til CosmosDB og læsning/skrivning af support-henvendelser.
public class DataService
{
    private readonly Container _container;

    public DataService(string connectionString, string databaseName, string containerName)
    {
        var client = new CosmosClient(connectionString);
        _container = client.GetContainer(databaseName, containerName);
    }

    // Indsætter en ny support-henvendelse i databasen.
    // Partition key udledes automatisk af SDK'et ud fra containerens opsætning.
    public async Task AddSupportMessageAsync(SupportMessage message)
    {
        await _container.CreateItemAsync(message);
    }

    // Henter alle support-henvendelser (bruges i aktivitet 3).
    public async Task<List<SupportMessage>> GetSupportMessagesAsync()
    {
        var results = new List<SupportMessage>();
        using var iterator = _container.GetItemQueryIterator<SupportMessage>("SELECT * FROM c");

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            results.AddRange(response);
        }

        return results;
    }
}
