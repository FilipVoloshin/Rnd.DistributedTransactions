using Grpc;
using Grpc.Core;
using MongoDB.Driver;
using Rnd.Inventory.Service.Data;

namespace Rnd.Inventory.Service.Services;

public class InventoryService: Grpc.InventoryService.InventoryServiceBase
{
    private readonly InventoryContext _context;
    
    public InventoryService(InventoryContext context)
    {
        _context = context;
    }

    public override async Task<CheckInventoryResponse> CheckInventory(CheckInventoryRequest request, ServerCallContext context)
    {
        var filter = Builders<InventoryItem>.Filter.Eq(s => s.ItemId, request.ItemId);
        var inventoryItem = await _context.InventoryItems.Find(filter)
            .FirstOrDefaultAsync();
    
        return new CheckInventoryResponse
        {
            IsAvailable = inventoryItem is not null &&
                          inventoryItem.Quantity >= request.Quantity,
            
        };
    }

    public override async Task<ReserveInventoryResponse> ReserveInventory(ReserveInventoryRequest request, ServerCallContext context)
    {
        var filter = Builders<InventoryItem>.Filter.Eq(s => s.ItemId, request.ItemId);
        var inventoryItem = await _context.InventoryItems.Find(filter).FirstOrDefaultAsync();

        try
        {
            
            if (inventoryItem == null)
            {
                return new ReserveInventoryResponse { IsSuccess = false };
            }
            
            var update = Builders<InventoryItem>.Update
                .Inc(s => s.Reserved, request.Quantity)
                .Inc(s => s.Quantity, -request.Quantity);
            
            await _context.InventoryItems.UpdateOneAsync(filter, update);
        }
        catch (Exception)
        {
            return new ReserveInventoryResponse { IsSuccess = false };
        }

        return new ReserveInventoryResponse { IsSuccess = true, Price = inventoryItem.Price * request.Quantity};
    }
    
    public override async Task<ReturnInventoryResponse> ReturnInventory(ReturnInventoryRequest request, ServerCallContext context)
    {
        try
        {
            var filter = Builders<InventoryItem>.Filter.Eq(s => s.ItemId, request.ItemId);
            var update = Builders<InventoryItem>.Update
                .Inc(s => s.Quantity, request.Quantity)
                .Inc(s => s.Reserved, -request.Quantity);
        
            await _context.InventoryItems.UpdateOneAsync(filter, update);
        }
        catch (Exception)
        {
            return new() { IsSuccess = false };
        }

        return new() { IsSuccess = true };
    }
}