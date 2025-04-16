using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Client;
using Domain.Aggregates.DailyAppointmentSchedule.Values.Appointment;
using Domain.Aggregates.Schedule.Entities;
using Domain.Aggregates.Schedule.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> entityBuilder)
    {
        entityBuilder.HasKey(appointment => appointment.Id);
        entityBuilder.Property(m => m.Id)
            .IsRequired()
            .HasConversion(
                mId => mId.Value,
                dbvalue => AppointmentId.FromGuid(dbvalue));

        entityBuilder.ComplexProperty<AppointmentNote>(appointment => appointment.AppointmentNote, propBuilder =>
        {
            propBuilder.Property(valueObject => valueObject.Value)
                .HasColumnName("Note");
        });

        entityBuilder.ComplexProperty<TimeSlot>(appointment => appointment.TimeSlot, propBuilder =>
        {
            propBuilder.Property(valueObject => valueObject.Start)
                .HasColumnName("StartTime");
            propBuilder.Property(valueObject => valueObject.End)
                .HasColumnName("EndTime");
        });

        entityBuilder.Property(appointment => appointment.AppointmentStatus)
            .HasConversion(
                status => (int)status,
                status => (AppointmentStatus)status)
            .IsRequired();

        entityBuilder.Property(appointment => appointment.AppointmentDate)
            .IsRequired()
            .HasConversion(
                date => date.ToString("yyyy-MM-dd"),
                dbValue => DateOnly.Parse(dbValue));

        entityBuilder.HasOne<Client>().WithMany().HasForeignKey(appointment => appointment.BookedByClient);
    }
}