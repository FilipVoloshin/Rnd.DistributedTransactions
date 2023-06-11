using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Rnd.Payment.Service.Data;
using Rnd.Payment.Service.Services;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5300, o => o.Protocols = HttpProtocols.Http2);
});
builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();
builder.Services.AddDbContextPool<PaymentsContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

var app = builder.Build();


app.MapGrpcService<PaymentsService>();
app.MapGrpcReflectionService();

app.Run();