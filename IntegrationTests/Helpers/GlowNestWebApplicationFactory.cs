using DomainModelPersistence.EfcConfigs;
using EfcQueries.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace IntegrationTests.Helpers;

public class GlowNestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:15-alpine")
        .WithCleanUp(true)
        .WithDatabase("glownest_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove existing context registrations
            services.RemoveAll<DbContextOptions<DomainModelContext>>();
            services.RemoveAll<DbContextOptions<PostgresContext>>();
            services.RemoveAll<DomainModelContext>();
            services.RemoveAll<PostgresContext>();

            services.AddDbContext<DomainModelContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));

            services.AddDbContext<PostgresContext>(options =>
                options.UseNpgsql(_dbContainer.GetConnectionString()));

            // Ensure schema is created before tests run
            using var scope = services.BuildServiceProvider().CreateScope();
            var domainDb = scope.ServiceProvider.GetRequiredService<DomainModelContext>();
            var queryDb = scope.ServiceProvider.GetRequiredService<PostgresContext>();
            domainDb.Database.EnsureCreated();
            queryDb.Database.EnsureCreated();
        });
    }

    public async Task InitializeAsync() => await _dbContainer.StartAsync();
    public async Task DisposeAsync() => await _dbContainer.DisposeAsync();
}
