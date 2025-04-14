using Domain.Aggregates.Appointment;
using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.SalonOwner;
using Domain.Aggregates.SalonOwner.Values;
using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs;

public class DomainModelContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DomainModelContext).Assembly);

        ConfigureClient(modelBuilder.Entity<Client>());
        ConfigureSalonOwner(modelBuilder.Entity<SalonOwner>());
        ConfigureAppointment(modelBuilder.Entity<Appointment>());
        ConfigureAppointmentService(modelBuilder.Entity<AppointmentService>());
    }

    private static void ConfigureClient(EntityTypeBuilder<Client> entityBuilder)
    {
        entityBuilder.HasKey(client => client.ClientId);

        entityBuilder.Property(id => id.ClientId)
            .IsRequired()
            .HasConversion(
                mId => mId.Value,
                dbvalue => ClientId.FromGuid(dbvalue));

        entityBuilder.ComplexProperty<FullName>(p => p.FullName, propBuilder =>
        {
            propBuilder.Property(valueObject => valueObject.FirstName)
                .HasColumnName("FirstName");
            propBuilder.Property(valueObject => valueObject.LastName)
                .HasColumnName("LastName");
        });

        entityBuilder.ComplexProperty<PhoneNumber>(p => p.PhoneNumber, propBuilder =>
        {
            propBuilder.Property(valueObject => valueObject.Value)
                .HasColumnName("PhoneNumber");
        });

        entityBuilder.ComplexProperty<Email>(p => p.Email, propBuilder =>
        {
            propBuilder.Property(valueObject => valueObject.Value)
                .HasColumnName("Email");
        });

        entityBuilder.ComplexProperty<Password>(p => p.Password, propBuilder =>
        {
            propBuilder.Property(valueObject => valueObject.Value)
                .HasColumnName("Password");
        });

        entityBuilder.Property(schedule => schedule.IsVerified).IsRequired();

        entityBuilder.OwnsOne(client => client.OtpSession, otpSession =>
        {
            // Email property mapping
            otpSession.Property(p => p.Email)
                .HasColumnName("Email")
                .IsRequired()
                .HasConversion(
                    eId => eId.Value,
                    dbValue => Email.Create(dbValue).Data
                );

            // Set OtpSession properties
            otpSession.Property(session => session.OtpCode)
                .HasColumnName("OtpCode")
                .IsRequired()
                .HasConversion(
                    otpCode => otpCode.Value,
                    dbValue => OtpCode.Create(dbValue).Data
                );

            otpSession.Property(session => session.CreatedAt)
                .HasColumnName("CreatedAt")
                .IsRequired()
                .HasConversion(
                    createdAt => createdAt.ToUniversalTime(),
                    dbValue => DateTimeOffset.UtcNow
                );

            otpSession.Property(session => session.Purpose)
                .HasColumnName("Purpose")
                .IsRequired()
                .HasConversion(
                    purpose => (int)purpose,
                    purpose => (Purpose)purpose
                );

            otpSession.Property(session => session.IsUsed)
                .HasColumnName("IsUsed")
                .IsRequired();

            otpSession.ToTable("OtpSessions");
            otpSession.HasKey("ClientId", "Email"); // composite key
        });
    }

    private static void ConfigureSalonOwner(EntityTypeBuilder<SalonOwner> entityBuilder)
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

    private static void ConfigureAppointment(EntityTypeBuilder<Appointment> entityBuilder)
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
    
    private static void ConfigureAppointmentService(EntityTypeBuilder<AppointmentService> entityBuilder)
    {
        entityBuilder.HasKey(x => new { x.AppointmentId, x.ServiceId });

        entityBuilder.Property(x => x.AppointmentId)
            .HasConversion(id => id.Value, db => AppointmentId.FromGuid(db));

        entityBuilder.Property(x => x.ServiceId)
            .HasConversion(id => id.Value, db => ServiceId.FromGuid(db));
    }
}