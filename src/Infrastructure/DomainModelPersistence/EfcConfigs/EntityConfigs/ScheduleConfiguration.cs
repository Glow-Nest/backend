using Domain.Aggregates.Appointment;
using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Client;
using Domain.Aggregates.DailyAppointmentSchedule;
using Domain.Aggregates.DailyAppointmentSchedule.Values.Appointment;
using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Entities;
using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Service.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs;

public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> entityBuilder)
    {
        entityBuilder.HasKey(schedule => schedule.ScheduleId);

        entityBuilder.Property(m => m.ScheduleId)
            .IsRequired()
            .HasConversion(
                mId => mId.Value,
                dbvalue => ScheduleId.FromGuid(dbvalue));
        
        entityBuilder.Property(m => m.ScheduleDate)
            .IsRequired()
            .HasConversion(
                date => date.ToString("yyyy-MM-dd"),
                dbValue => DateOnly.Parse(dbValue));

        // configure blocked time slots
        entityBuilder.OwnsMany(schedule => schedule.BlockedTimeSlots, timeSlot =>
        {
            timeSlot.ToTable("BlockedTimeSlots");
            timeSlot.WithOwner().HasForeignKey("ScheduleId");

            timeSlot.Property(t => t.Start).HasColumnName("StartTime");
            timeSlot.Property(t => t.End).HasColumnName("EndTime");
        });

        // configure appointments
        entityBuilder.HasMany<Appointment>(schedule => schedule.Appointments)
            .WithOne()
            .HasForeignKey("ScheduleId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}