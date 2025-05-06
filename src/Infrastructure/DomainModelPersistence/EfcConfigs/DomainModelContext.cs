using Microsoft.EntityFrameworkCore;

namespace DomainModelPersistence.EfcConfigs;

public class DomainModelContext(DbContextOptions<DomainModelContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DomainModelContext).Assembly);
    }
}