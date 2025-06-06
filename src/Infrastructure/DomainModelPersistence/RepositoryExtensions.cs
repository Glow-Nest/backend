using Domain.Aggregates.Client;
using Domain.Aggregates.Order;
using Domain.Aggregates.Product;
using Domain.Aggregates.ProductReview;
using Domain.Aggregates.SalonOwner;
using Domain.Aggregates.Schedule;
using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceReview;
using Domain.Common;
using DomainModelPersistence.ClientPersistence;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories;
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
        serviceCollection.AddScoped<IScheduleRepository, ScheduleRepository>();
        serviceCollection.AddScoped<ICategoryRepository, CategoryRepository>();
        serviceCollection.AddScoped<IProductRepository, ProductRepository>();
        serviceCollection.AddScoped<IOrderRepository, OrderRepository>();
        serviceCollection.AddScoped<IServiceReviewRepository, ServiceReviewRepository>();
        serviceCollection.AddScoped<IProductReviewRepository, ProductReviewRepository>();
    }
    
    private static void RegisterUnitOfWork(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}