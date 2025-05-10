using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;
using EfcQueries.Queries;
using EfcQueries.Queries.Category;
using EfcQueries.Queries.Product;
using EfcQueries.Queries.Schedules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QueryContracts.Contracts;
using QueryContracts.Queries;
using QueryContracts.Queries.Product;
using QueryContracts.Queries.Schedule;
using QueryContracts.Queries.Service;

namespace EfcQueries.Extension;

public static class EfcQueriesExtension
{
    public static void RegisterQueryHandlers(this IServiceCollection services)
    {
        // client
        services.AddScoped<IQueryHandler<LoginUserQuery, Result<LoginUserResponse>>, LoginUserQueryHandler>();
        
        // schedule
        services.AddScoped<IQueryHandler<GetBlockedTime.Query, Result<GetBlockedTime.Answer>>, GetBlockedTimeQueryHandler>();
        services.AddScoped<IQueryHandler<GetAvailableSlotsForDate.Query, Result<GetAvailableSlotsForDate.Answer>>, GetAvailableSlotsForDateQueryHandler>();
        services.AddScoped<IQueryHandler<GetAppointmentsByDate.Query, Result<GetAppointmentsByDate.Answer>>, GetAppointmentsByDateQueryHandler>();
        services.AddScoped<IQueryHandler<GetClientsAppointment.Query, Result<GetClientsAppointment.Answer>>, GetClientAppointmentsQueryHandler>();
        
        // category and service
        services.AddScoped<IQueryHandler<GetAllCategory.Query, Result<GetAllCategory.Answer>>, GetAllCategoryQueryHandler>();
        services.AddScoped<IQueryHandler<GetAllCategoriesWithServices.Query, Result<GetAllCategoriesWithServices.Answer>>, GetAllCategoryWithServiceQueryHandler>();
        
        // product
        services.AddScoped<IQueryHandler<GetAllProductsQuery.Query, Result<GetAllProductsQuery.Answer>>, GetAllProductsQueryHandler>();
        services.AddScoped<IQueryHandler<GetProductByIdQuery.Query, Result<GetProductByIdQuery.Answer>>, GetProductByIdQueryHandler>();
    }

    public static void RegisterDatabase(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        serviceCollection.AddDbContext<DomainModelContext>(options =>
            options.UseNpgsql(connectionString!, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorCodesToAdd: null);
            }));
    }
}