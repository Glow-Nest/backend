using Domain.Aggregates.Client;
using Domain.Aggregates.Product;
using Domain.Aggregates.ProductReview;
using Domain.Aggregates.ProductReview.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs;

public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        builder.HasKey(s => s.ProductReviewId);
        builder.Property(s => s.ProductReviewId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => ProductReviewId.FromGuid(value));

        builder.ComplexProperty(s => s.ReviewMessage, b =>
        {
            b.Property(r => r.Value).HasColumnName("ReviewMessage");
        });
        
        builder.ComplexProperty(s => s.Rating, b =>
        {
            b.Property(r => r.Value).HasColumnName("Rating");
        });
        
        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<Client>()
            .WithMany()
            .HasForeignKey(f => f.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}