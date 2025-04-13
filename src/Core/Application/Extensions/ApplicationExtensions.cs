using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Application.AppEntry.Decorators;
using Application.AppEntry.Dispatchers;
using Application.Handlers.ClientHandlers;
using Application.Handlers.DomainEvents;
using Domain.Aggregates.Client.DomainEvents;
using Domain.Common;
using Microsoft.Extensions.DependencyInjection;
using QueryContracts.QueryDispatching;

namespace Application.Extensions;

public static class ApplicationExtensions
{
    public static void RegisterApplications(this IServiceCollection serviceCollection)
    {
        RegisterHandlers(serviceCollection);
        RegisterDispatcher(serviceCollection);
    }
    
    private static void RegisterHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICommandHandler<CreateClientCommand>, CreateClientHandler>();
        serviceCollection.AddScoped<ICommandHandler<CreateOtpCommand>, CreateOtpHandler>();
        serviceCollection.AddScoped<ICommandHandler<VerifyOtpCommand>, VerifyOtpHandler>();

        serviceCollection.AddScoped<IDomainEventHandler<OtpCreatedDomainEvent>, OtpCreatedDomainEventHandler>();
    }

    private static void RegisterDispatcher(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICommandDispatcher>(provider =>
        {
            var dispatcher = new CommandDispatcher(provider);
            var transactionDecorator = new TransactionDecorator(dispatcher, provider.GetRequiredService<IUnitOfWork>());
            var domainEventDecorator = new DomainEventDecorator(transactionDecorator, provider.GetRequiredService<IUnitOfWork>(), provider.GetRequiredService<IDomainEventDispatcher>());
            
            return domainEventDecorator;
        });

        serviceCollection.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        serviceCollection.AddScoped<IQueryDispatcher, QueryDispatcher>();
    }
}