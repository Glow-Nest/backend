using Domain.Aggregates.Appointment;
using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Client;
using Domain.Aggregates.Service.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> entityBuilder)
    {
        entityBuilder.HasKey(appointment => appointment.AppointmentId);
        entityBuilder.Property(m => m.AppointmentId)
            .IsRequired()
            .HasConversion(
                mId => mId.Value,
                dbvalue => AppointmentId.FromGuid(dbvalue));
        
        entityBuilder.ComplexProperty<AppointmentNote>(appointment => appointment.AppointmentNote, propBuilder =>
        {
            propBuilder.Property(valueObject => valueObject.Value)
                .HasColumnName("AppointmentNote");
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
        entityBuilder
            .HasMany<AppointmentService>()
            .WithOne()
            .HasForeignKey(x => x.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);;
        
        entityBuilder
            .Ignore(a => a.Services);
    }
}