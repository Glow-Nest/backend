using Microsoft.EntityFrameworkCore;

namespace DomainModelPersistence.EfcConfigs;

public class DomainModelContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DomainModelContext).Assembly);
    }
}