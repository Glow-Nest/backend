using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Schedule.Entities;
using Domain.Aggregates.Schedule.Values.Appointment;
using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs.ScheduleConfigs;

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