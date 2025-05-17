using Domain.Aggregates.Client;
using Domain.Aggregates.Order.Values;
using Domain.Common.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs.Order;

public class OrderConfiguration : IEntityTypeConfiguration<Domain.Aggregates.Order.Order>
{
    public void Configure(EntityTypeBuilder<Domain.Aggregates.Order.Order> entityBuilder)
    {
        entityBuilder.HasKey(order => order.OrderId);
        entityBuilder.Property(order => order.OrderId)
            .IsRequired()
            .HasConversion(
                orderId => orderId.Value,
                dbValue => OrderId.FromGuid(dbValue).Data);

        entityBuilder.Property(order => order.OrderDate)
            .IsRequired()
            .HasConversion(
                date => date.ToString("yyyy-MM-dd"),
                dbValue => DateOnly.Parse(dbValue));

        entityBuilder.Property(order => order.PickupDate)
            .IsRequired()
            .HasConversion(
                date => date.ToString("yyyy-MM-dd"),
                dbValue => DateOnly.Parse(dbValue));

        entityBuilder.Property(order => order.PaymentStatus)
            .HasConversion(
                status => (int)status,
                status => (PaymentStatus)status)
            .IsRequired();

        entityBuilder.Property(order => order.OrderStatus)
            .HasConversion(
                status => status.ToString(),
                value => (OrderStatus)Enum.Parse(typeof(OrderStatus), value))
            .IsRequired();

        entityBuilder.Property(order => order.TotalPrice)
            .IsRequired()
            .HasConversion(
                price => price.Value,
                dbValue => Price.Create(dbValue).Data);
        
        entityBuilder.HasMany(order => order.OrderItems)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        entityBuilder.HasOne<Client>()
            .WithMany()
            .HasForeignKey(order => order.ClientId);
    }
}