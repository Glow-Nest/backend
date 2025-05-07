using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Application.AppEntry.Commands.Schedule;
using Application.AppEntry.Commands.ServiceCategory;
using Application.AppEntry.Decorators;
using Application.AppEntry.Dispatchers;
using Application.Handlers.ClientHandlers;
using Application.Handlers.DomainEvents;
using Application.Handlers.ScheduleHandlers;
using Application.Handlers.ServiceCategoryHandlers;
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
        // client
        serviceCollection.AddScoped<ICommandHandler<CreateClientCommand>, CreateClientHandler>();
        serviceCollection.AddScoped<ICommandHandler<CreateOtpCommand>, CreateOtpHandler>();
        serviceCollection.AddScoped<ICommandHandler<VerifyOtpCommand>, VerifyOtpHandler>();
        serviceCollection.AddScoped<ICommandHandler<ResetPasswordCommand>, ResetPasswordHandler>();
        serviceCollection.AddScoped<ICommandHandler<InitiateResetPasswordCommand>, InitiateResetPasswordHandler>();

        // schedule
        serviceCollection.AddScoped<ICommandHandler<CreateAppointmentCommand>, CreateAppointmentHandler>();
        serviceCollection.AddScoped<ICommandHandler<AddBlockedTimeCommand>, AddBlockedTimeHandler>();
        serviceCollection.AddScoped<ICommandHandler<CreateFutureSchedulesCommand>, CreateFutureSchedulesHandler>();

        // category and service
        serviceCollection.AddScoped<ICommandHandler<CreateCategoryCommand>, CreateCategoryHandler>();
        serviceCollection.AddScoped<ICommandHandler<AddServiceInCategoryCommand>, AddServiceInCategoryHandler>();

        serviceCollection.AddScoped<IDomainEventHandler<OtpCreatedDomainEvent>, OtpCreatedDomainEventHandler>();
    }

    private static void RegisterDispatcher(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICommandDispatcher>(provider =>
        {
            var dispatcher = new CommandDispatcher(provider);
            var transactionDecorator = new TransactionDecorator(dispatcher, provider.GetRequiredService<IUnitOfWork>());
            var domainEventDecorator = new DomainEventDecorator(transactionDecorator,
                provider.GetRequiredService<IUnitOfWork>(), provider.GetRequiredService<IDomainEventDispatcher>());

            return domainEventDecorator;
        });

        serviceCollection.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        serviceCollection.AddScoped<IQueryDispatcher, QueryDispatcher>();
    }
}