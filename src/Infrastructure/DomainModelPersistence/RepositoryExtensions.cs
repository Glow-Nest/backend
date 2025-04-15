using Domain.Aggregates.Appointment;
using Domain.Aggregates.Client;
using Domain.Aggregates.SalonOwner;
using Domain.Common;
using DomainModelPersistence.ClientPersistence;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories;
using DomainModelPersistence.SalonOwnerPersistence;
using Microsoft.Extensions.DependencyInjection;

namespace DomainModelPersistence;

public static class RepositoryExtensions
{
    public static void RegisterDmPersistence(this IServiceCollection serviceCollection)
    {
        RegisterRepositories(serviceCollection);
        RegisterUnitOfWork(serviceCollection);
    }
    
    private static void RegisterRepositories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<DomainModelContext>();

        serviceCollection.AddScoped<IClientRepository, ClientRepository>();
        serviceCollection.AddScoped<ISalonOwnerRepository, SalonOwnerRepository>();
        serviceCollection.AddScoped<IAppointmentRepository, AppointmentRepository>();
    }
    
    private static void RegisterUnitOfWork(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}