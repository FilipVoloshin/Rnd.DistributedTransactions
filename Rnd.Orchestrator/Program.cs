using Microsoft.AspNetCore.Server.Kestrel.Core;
using MongoDB.Driver;
using Rnd.Orchestrator.Sagas.OrderSaga;
using Rnd.Orchestrator.Sagas.Stores;
using Rnd.Orchestrator.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000, o => o.Protocols = HttpProtocols.Http2);
});
builder.Services.AddHealthChecks();
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("MongoConnection"));
var mongoDatabase = mongoClient.GetDatabase(builder.Configuration.GetValue<string>("Saga:Store:MongoDatabaseName"));

builder.Services.AddSingleton(mongoDatabase);

builder.Services.AddScoped(typeof(IStateStore<>), typeof(MongoStateStore<>));

builder.Services.AddGrpcClient<Grpc.InventoryService.InventoryServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5100");
});

builder.Services.AddGrpcClient<Grpc.OrderService.OrderServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5200");
});

builder.Services.AddGrpcClient<Grpc.PaymentService.PaymentServiceClient>(o =>
{
    o.Address = new Uri("http://localhost:5300");
});

builder.Services.AddScoped<OrderSaga>();

var app = builder.Build();

app.MapGrpcService<OrchestratorService>();
app.MapGrpcReflectionService();

app.MapHealthChecks("/_health");
app.Run();
