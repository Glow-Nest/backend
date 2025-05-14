using Domain.Aggregates.Order.Entities;
using Domain.Aggregates.Order.Values;
using Domain.Aggregates.Product;
using Domain.Common.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs.Order;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> entityBuilder)
    {
        entityBuilder.HasKey(item => new { item.Id, item.ProductId });
        entityBuilder.Property(item => item.Id)
            .IsRequired()
            .HasConversion(
                itemId => itemId.Value,
                dbValue => OrderItemId.FromGuid(dbValue).Data);

        entityBuilder.Property(item => item.Quantity)
            .IsRequired()
            .HasConversion(
                quantity => quantity.Value,
                dbValue => Quantity.Create(dbValue).Data);

        entityBuilder.Property(item => item.PriceWhenOrdering)
            .IsRequired()
            .HasConversion(
                price => price.Value,
                dbValue => Price.Create(dbValue).Data);

        entityBuilder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(item => item.ProductId);
    }
}