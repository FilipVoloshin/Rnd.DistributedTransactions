using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rnd.Order.Service.Data;

public class Order
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ItemId { get; set; }
    public int Quantity { get; set; }
    public OrderStatus Status { get; set; } 
}

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.UserId)
            .IsRequired();
            
        builder.Property(o => o.ItemId)
            .IsRequired();
            
        builder.Property(o => o.Quantity)
            .IsRequired();
            
        builder.Property(o => o.Status)
            .IsRequired();
    }
}