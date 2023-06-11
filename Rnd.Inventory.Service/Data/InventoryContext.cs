using MongoDB.Driver;

namespace Rnd.Inventory.Service.Data;

public class InventoryContext
{
    private readonly IMongoDatabase _database;
    public MongoClient Client { get; }

    public InventoryContext(IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("MongoConnection");
        Client = new MongoClient(connection);
        _database = Client.GetDatabase("internet-shop");
    }

    public IMongoCollection<InventoryItem> InventoryItems => _database.GetCollection<InventoryItem>("inventory");
}