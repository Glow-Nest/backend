using Domain.Aggregates.Schedule.Entities;
using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Schedule.Values.BlockedTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs.ScheduleConfigs;

public class BlockedTimeConfiguration : IEntityTypeConfiguration<BlockedTime>
{
    public void Configure(EntityTypeBuilder<BlockedTime> entityBuilder)
    {
        entityBuilder.HasKey(time => time.Id);
        entityBuilder.Property(m => m.Id)
            .IsRequired()
            .HasConversion(
                mId => mId.Value,
                dbvalue => BlockedTimeId.FromGuid(dbvalue));
        
        entityBuilder.Property(bt => bt.ScheduledDate)
            .IsRequired()
            .HasConversion(
                date => date.ToString("yyyy-MM-dd"),
                dbValue => DateOnly.Parse(dbValue));
        
        entityBuilder.ComplexProperty<TimeSlot>(time => time.TimeSlot, propBuilder =>
        {
            propBuilder.Property(valueObject => valueObject.Start)
                .HasColumnName("StartTime");
            propBuilder.Property(valueObject => valueObject.End)
                .HasColumnName("EndTime");
        });

        entityBuilder.ComplexProperty<BlockReason>(time => time.Reason, builder =>
        {
            builder.Property(valueObject => valueObject.Value)
                .HasColumnName("Reason");
        });
    }
}