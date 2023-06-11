using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Rnd.Payment.Service.Data;

public class PaymentsContext: DbContext
{
    public DbSet<Payment> Payments { get; set; } = null!;

    public PaymentsContext(DbContextOptions options) :base(options) {} 
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}