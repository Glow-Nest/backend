using Domain.Aggregates.Client.Values;
using Domain.Aggregates.SalonOwner;
using Domain.Aggregates.SalonOwner.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs;

public class SalonOwnerConfiguration : IEntityTypeConfiguration<SalonOwner>
{
    public void Configure(EntityTypeBuilder<SalonOwner> entityBuilder)
    {
        entityBuilder.HasKey(salonOwner => salonOwner.SalonOwnerId);

        entityBuilder.Property(m => m.SalonOwnerId)
            .IsRequired()
            .HasConversion(
                mId => mId.Value,
                dbvalue => SalonOwnerId.FromGuid(dbvalue));

        entityBuilder.ComplexProperty<Email>(p => p.Email,
            builder => { builder.Property(value => value.Value).HasColumnName("Email"); });

        entityBuilder.ComplexProperty<Password>(p => p.Password, propBuilder =>
        {
            propBuilder.Property(valueObject => valueObject.Value)
                .HasColumnName("Password");
        });
    }
}