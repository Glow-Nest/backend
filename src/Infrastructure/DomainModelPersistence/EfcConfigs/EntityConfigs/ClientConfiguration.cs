using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DomainModelPersistence.EfcConfigs.EntityConfigs;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> entityBuilder)
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
}