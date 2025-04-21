using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> entityBuilder)
    {
        entityBuilder.HasKey(service => service.ServiceId);

        entityBuilder.Property(service => service.ServiceId)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => ServiceId.FromGuid(value));

        entityBuilder.ComplexProperty(p => p.Name, b =>
        {
            b.Property(v => v.Value).HasColumnName("Name");
        });

        entityBuilder.ComplexProperty(p => p.Description, b =>
        {
            b.Property(v => v.Value).HasColumnName("Description");
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

        // entityBuilder.OwnsMany(service => service.MediaUrls, mediaBuilder =>
        // {
        //     mediaBuilder.WithOwner().HasForeignKey("ServiceId");
        //
        //     mediaBuilder.Property(m => m.Value)
        //         .HasColumnName("Url")
        //         .IsRequired();
        //
        //     mediaBuilder.HasKey("ServiceId", "Url");
        //
        //     mediaBuilder.ToTable("ServiceMediaUrls");
        // });
        //
        // entityBuilder.Navigation(service => service.MediaUrls).AutoInclude();
    }
}
