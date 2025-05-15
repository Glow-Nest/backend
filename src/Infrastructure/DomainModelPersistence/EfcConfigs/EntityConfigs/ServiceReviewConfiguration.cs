using Domain.Aggregates.Client;
using Domain.Aggregates.ServiceCategory.Entities;
using Domain.Aggregates.ServiceReview;
using Domain.Aggregates.ServiceReview.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs;

public class ServiceReviewConfiguration : IEntityTypeConfiguration<ServiceReview>
{
    public void Configure(EntityTypeBuilder<ServiceReview> builder)
    {
        builder.HasKey(s => s.ServiceReviewId);

        builder.Property(s => s.ServiceReviewId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => ServiceReviewId.FromGuid(value));
        
        builder.ComplexProperty(s => s.ReviewMessage, b => 
        {
            b.Property(r => r.Value).HasColumnName("ReviewMessage");
        });
        
        builder.ComplexProperty(s => s.Rating, b => 
        {
            b.Property(r => r.Value).HasColumnName("Rating");
        });
        
        builder.HasOne<Client>()
            .WithMany()
            .HasForeignKey(s => s.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne<Service>()
            .WithMany()
            .HasForeignKey(s => s.ServiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}