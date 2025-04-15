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
        entityBuilder.Property(service =>  service.ServiceId)
            .IsRequired()
            .HasConversion(
                mId => mId.Value,
                dbvalue => ServiceId.FromGuid(dbvalue));
    }
}