using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Service.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs;

public class AppointmentServiceConfiguration : IEntityTypeConfiguration<AppointmentService>
{
    public void Configure(EntityTypeBuilder<AppointmentService> entityBuilder)
    {
        entityBuilder.HasKey(x => new { x.AppointmentId, x.ServiceId });

        entityBuilder.Property(x => x.AppointmentId)
            .HasConversion(id => id.Value, db => AppointmentId.FromGuid(db));

        entityBuilder.Property(x => x.ServiceId)
            .HasConversion(id => id.Value, db => ServiceId.FromGuid(db));
    }
}