using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DomainModelPersistence.EfcConfigs;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<DomainModelContext>
{
    public DomainModelContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DomainModelContext>();
        var connectionString = "Host=thermally-subtle-anhinga.data-1.euc1.tembo.io;Port=5432;Database=postgres;Username=postgres;Password=JQ9sV2dZLfqQCGXI;Ssl Mode=Require;Trust Server Certificate=true";
        optionsBuilder.UseNpgsql(connectionString);
        return new DomainModelContext(optionsBuilder.Options);
    }
}