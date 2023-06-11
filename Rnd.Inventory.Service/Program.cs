using Microsoft.AspNetCore.Server.Kestrel.Core;
using MongoDB.Driver;
using Rnd.Inventory.Service.Data;
using Rnd.Inventory.Service.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5100, o => o.Protocols = HttpProtocols.Http2);
});
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("MongoConnection");
    return new MongoClient(connectionString);
}); 
builder.Services.AddScoped<InventoryContext>();

var app = builder.Build();

app.MapGrpcService<InventoryService>();
app.MapGrpcReflectionService();


app.Run();