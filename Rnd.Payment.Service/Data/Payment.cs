using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rnd.Payment.Service.Data;

public class Payment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid UserId { get; set; }
    public double Amount { get; set; }
    public PaymentStatus Status { get; set; }
}

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.OrderId)
            .IsRequired();
            
        builder.Property(p => p.UserId)
            .IsRequired();
            
        builder.Property(p => p.Amount)
            .IsRequired();
            
        builder.Property(p => p.Status)
            .IsRequired();
    }
}