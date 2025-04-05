using Domain.Aggregates.Client;
using Domain.Aggregates.SalonOwner;
using Domain.Common;
using DomainModelPersistence.ClientPersistence;
using DomainModelPersistence.SalonOwnerPersistence;
using Microsoft.Extensions.DependencyInjection;

namespace DomainModelPersistence;

public static class RepositoryExtensions
{
    public static void RegisterRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IClientRepository, ClientRepository>();
        serviceCollection.AddSingleton<ISalonOwnerRepository, SalonOwnerRepository>();
    }

    public static void RegisterUnitOfWork(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IUnitOfWork, UnitOfWork>();
    }
}