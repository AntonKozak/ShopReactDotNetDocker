using System.Text.Json;

using MongoDB.Driver;
using MongoDB.Entities;

using SearchService.Models;
using SearchService.Services;

namespace SearchService.Data;

public class DbInitializer
{
    public static async Task InitializeAsync(WebApplication app)
    {

        await DB.InitAsync("SearchServiceDB", MongoClientSettings.FromConnectionString(app.Configuration.GetConnectionString("MongoDBConnection")));

        await DB.Index<Item>()
            .Key(i => i.Make, KeyType.Text)
            .Key(i => i.Model, KeyType.Text)
            .Key(i => i.Color, KeyType.Text)
            .CreateAsync();

        var count = await DB.CountAsync<Item>();

        using var scope = app.Services.CreateScope();

        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionServiceHttpClient>();

        var items = await httpClient.GetItemsForSearchDb();

        Console.WriteLine($"Fetched {items.Count} items from Auction Service");
        if (count == 0 && items.Count > 0)
        {
            Console.WriteLine("Initializing Search Service database with items from Auction Service...");

            var tasks = items.Select(item => DB.SaveAsync(item));
            await Task.WhenAll(tasks);

            Console.WriteLine($"Successfully initialized Search Service database with {items.Count} items.");
        }
        else
        {
            Console.WriteLine("Search Service database already initialized or no new items to add.");
        }
    }
}
