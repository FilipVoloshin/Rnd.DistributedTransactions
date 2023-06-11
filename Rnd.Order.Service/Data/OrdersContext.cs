using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Rnd.Order.Service.Data;

public class OrdersContext: DbContext
{
    public DbSet<Order> Orders { get; set; } = null!;

    public OrdersContext(DbContextOptions options) :base(options) {} 
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}