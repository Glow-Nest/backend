using Domain.Aggregates.Client;
using Domain.Common;
using Microsoft.Extensions.DependencyInjection;
using Repositories.Repositories;

namespace Repositories;

public static class RepositoryExtensions
{
    public static void RegisterRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IClientRepository, ClientRepository>();
    }

    public static void RegisterUnitOfWork(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IUnitOfWork, UnitOfWork>();
    }
}