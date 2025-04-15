using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DomainModelPersistence.EfcConfigs;

public class DesignTimeContextFactory : IDesignTimeDbContextFactory<DomainModelContext>
{
    public DomainModelContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/WebAPI"))
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        
        var connectionString = config.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<DomainModelContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new DomainModelContext(optionsBuilder.Options);
    }
}