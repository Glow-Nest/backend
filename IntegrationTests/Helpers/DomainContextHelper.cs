using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.SalonOwner;
using Domain.Aggregates.SalonOwner.Values;
using DomainModelPersistence.EfcConfigs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationTests.Helpers;

public class DomainContextHelper : DomainModelContext
{
    public DomainContextHelper(DbContextOptions<DomainModelContext> options) 
        : base(options)
    {
    }
    
    public static DomainContextHelper SetupContext()
    {
        DbContextOptionsBuilder<DomainModelContext> optionsBuilder = new();
        string testDbName = "Test" + Guid.NewGuid() +".db";
        optionsBuilder.UseSqlite(@"Data Source = " + testDbName);
        DomainContextHelper context = new(optionsBuilder.Options);
        
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }
    
    public static async Task SaveAndClearAsync<T>(T entity, DomainContextHelper context) 
        where T : class
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureClient(modelBuilder.Entity<Client>());
        ConfigureSalonOwner(modelBuilder.Entity<SalonOwner>());
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
            otpSession.Property(p => p.Email)
                .HasColumnName("Email")
                .IsRequired()
                .HasConversion(
                    eId => eId.Value,
                    dbValue => Email.Create(dbValue).Data
                );

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
                    dbValue => DateTimeOffset.UtcNow // Might need adjustment if preserving exact values is important
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
            otpSession.HasKey("ClientId", "Email");
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

        entityBuilder.ComplexProperty<Email>(p => p.Email, builder =>
        {
            builder.Property(value => value.Value).HasColumnName("Email");
        });

        entityBuilder.ComplexProperty<Password>(p => p.Password, propBuilder =>
        {
            propBuilder.Property(valueObject => valueObject.Value)
                .HasColumnName("Password");
        });
    }
}