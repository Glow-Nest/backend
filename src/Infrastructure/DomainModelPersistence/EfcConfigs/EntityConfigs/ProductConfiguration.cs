using Domain.Aggregates.Product;
using Domain.Aggregates.Product.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.ProductId);

        builder.Property(p => p.ProductId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => ProductId.FromGuid(value));

        builder.ComplexProperty(p => p.ProductName, b =>
        {
            b.Property(v => v.Value).HasColumnName("Name");
        });

        builder.ComplexProperty(p => p.Price, b =>
        {
            b.Property(v => v.Value).HasColumnName("Price");
        });

        builder.ComplexProperty(p => p.Description, b =>
        {
            b.Property(v => v.Value).HasColumnName("Description");
        });

        builder.ComplexProperty(p => p.InventoryCount, b =>
        {
            b.Property(v => v.Value).HasColumnName("InventoryCount");
        });

        builder.ComplexProperty(p => p.ImageUrl, b =>
        {
            b.Property(v => v.Value).HasColumnName("ImageUrl");
        });
    }
}