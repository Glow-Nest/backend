using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.SalonOwner;
using Domain.Aggregates.SalonOwner.Values;
using DomainModelPersistence.EfcConfigs;
using Microsoft.Data.Sqlite;
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
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<DomainModelContext>()
            .UseSqlite(connection)
            .Options;

        var context = new DomainContextHelper(options);
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DomainModelContext).Assembly);
    }
}