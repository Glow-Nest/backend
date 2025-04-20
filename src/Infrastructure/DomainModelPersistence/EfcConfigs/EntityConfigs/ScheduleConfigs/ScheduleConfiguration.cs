using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Entities;
using Domain.Aggregates.Schedule.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs.ScheduleConfigs;

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
        entityBuilder.HasMany<BlockedTime>(schedule => schedule.BlockedTimeSlots)
            .WithOne()
            .HasForeignKey("ScheduleId")
            .OnDelete(DeleteBehavior.Cascade);
        
        // configure appointments
        entityBuilder.HasMany<Appointment>(schedule => schedule.Appointments)
            .WithOne()
            .HasForeignKey("ScheduleId")
            .OnDelete(DeleteBehavior.Cascade);
    }
}