using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Application.AppEntry.Decorators;
using Application.AppEntry.Dispatchers;
using Application.Handlers.ClientHandlers;
using Application.Handlers.DomainEvents;
using Domain.Aggregates.Client.DomainEvents;
using Domain.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationExtensions
{
    public static void RegisterHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICommandHandler<CreateClientCommand>, CreateClientHandler>();
        serviceCollection.AddSingleton<ICommandHandler<CreateOtpCommand>, CreateOtpHandler>();
        serviceCollection.AddSingleton<ICommandHandler<VerifyOtpCommand>, VerifyOtpHandler>();

        serviceCollection.AddSingleton<IDomainEventHandler<OtpCreatedDomainEvent>, OtpCreatedDomainEventHandler>();
    }

    public static void RegisterDispatcher(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICommandDispatcher>(provider =>
        {
            var dispatcher = new CommandDispatcher(provider);
            var transactionDecorator = new TransactionDecorator(dispatcher, provider.GetRequiredService<IUnitOfWork>());
            var domainEventDecorator = new DomainEventDecorator(transactionDecorator, provider.GetRequiredService<IUnitOfWork>(), provider.GetRequiredService<IDomainEventDispatcher>());
            
            return domainEventDecorator;
        });

        serviceCollection.AddSingleton<IDomainEventDispatcher, DomainEventDispatcher>();
    }
}