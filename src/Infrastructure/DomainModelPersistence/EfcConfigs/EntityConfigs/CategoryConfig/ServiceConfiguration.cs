using Domain.Aggregates.ServiceCategory.Entities;
using Domain.Aggregates.ServiceCategory.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs.CategoryConfig;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> entityBuilder)
    {
        entityBuilder.HasKey(c => c.ServiceId);
        
        entityBuilder.Property(c => c.ServiceId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => ServiceId.FromGuid(value));

        entityBuilder.ComplexProperty(p => p.Name, b =>
        {
            b.Property(v => v.Value).HasColumnName("Name");
        });
        
        entityBuilder.ComplexProperty(p => p.Price, b =>
        {
            b.Property(v => v.Value).HasColumnName("Price");
        });

        entityBuilder.Property(service => service.Duration)
            .IsRequired()
            .HasConversion(
                t => t.TotalMinutes,
                m => TimeSpan.FromMinutes(m));
    }
}