using Domain.Aggregates.Appointment;
using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs;

public class AppointmentServiceReferenceConfiguration : IEntityTypeConfiguration<AppointmentServiceReference>
{
    public void Configure(EntityTypeBuilder<AppointmentServiceReference> entityBuilder)
    {
        entityBuilder.Property<AppointmentId>("AppointmentId");
        entityBuilder.HasKey("AppointmentId", "ServiceId");
        entityBuilder.HasOne<Appointment>()
            .WithMany(appointment => appointment.Services)
            .HasForeignKey("AppointmentId");

        entityBuilder.Property(reference => reference.ServiceId)
            .HasConversion(
                id => id.Value,
                dbValue => ServiceId.FromGuid(dbValue));

        entityBuilder.HasOne<Service>()
            .WithMany()
            .HasForeignKey(reference => reference.ServiceId);
    }
}