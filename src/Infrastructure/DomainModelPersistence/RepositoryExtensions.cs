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
        serviceCollection.AddScoped<IClientRepository, ClientRepository>();
        serviceCollection.AddScoped<ISalonOwnerRepository, SalonOwnerRepository>();
    }

    public static void RegisterUnitOfWork(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}