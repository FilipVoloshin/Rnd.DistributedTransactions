using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rnd.Inventory.Service.Data;

[Table("inventory")]
public class InventoryItem
{
    public ObjectId Id { get; set; }
    
    [BsonElement("iid")]
    public string ItemId { get; set; } = null!;
    
    [BsonElement("in")]
    public string ItemName { get; set; } = null!;
    
    [BsonElement("qnt")]
    public int Quantity { get; set; }
    
    [BsonElement("rsv")]
    public int Reserved { get; set; }
    
    [BsonElement("prc")]
    public double Price { get; set; }
}