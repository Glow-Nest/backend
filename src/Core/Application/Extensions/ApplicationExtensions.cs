using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Application.AppEntry.Commands.Client.UpdateClient;
using Application.AppEntry.Commands.Product;
using Application.AppEntry.Commands.Product.UpdateProduct;
using Application.AppEntry.Commands.Schedule;
using Application.AppEntry.Commands.ServiceCategory;
using Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;
using Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;
using Application.AppEntry.Decorators;
using Application.AppEntry.Dispatchers;
using Application.Handlers.ClientHandlers;
using Application.Handlers.ClientHandlers.UpdateClientHandler;
using Application.Handlers.DomainEvents;
using Application.Handlers.ProductHandlers;
using Application.Handlers.ProductHandlers.UpdateProductHandler;
using Application.Handlers.ScheduleHandlers;
using Application.Handlers.ServiceCategoryHandlers;
using Application.Handlers.ServiceCategoryHandlers.UpdateCategoryHandler;
using Application.Handlers.ServiceCategoryHandlers.UpdateServiceHandler;
using Domain.Aggregates.Client.DomainEvents;
using Domain.Aggregates.Schedule.DomainEvents;
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
        serviceCollection.AddScoped<ICommandHandler<UpdateFullNameCommand>, UpdateFullNameHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdatePhoneNumberCommand>, UpdatePhoneNumberHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdatePasswordCommand>, UpdatePasswordHandler>();

        // schedule
        serviceCollection.AddScoped<ICommandHandler<CreateAppointmentCommand>, CreateAppointmentHandler>();
        serviceCollection.AddScoped<ICommandHandler<AddBlockedTimeCommand>, AddBlockedTimeHandler>();
        serviceCollection.AddScoped<ICommandHandler<CreateFutureSchedulesCommand>, CreateFutureSchedulesHandler>();

        // category and service
        serviceCollection.AddScoped<ICommandHandler<CreateCategoryCommand>, CreateCategoryHandler>();
        serviceCollection.AddScoped<ICommandHandler<AddServiceInCategoryCommand>, AddServiceInCategoryHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateCategoryNameCommand>, UpdateCategoryNameHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateCategoryDescriptionCommand>, UpdateCategoryDescriptionHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateMediaUrlCommand>, UpdateMediaUrlHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateServiceNameCommand>, UpdateServiceNameHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateServiceDurationCommand>, UpdateServiceDurationHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateServicePriceCommand>, UpdateServicePriceHandler>();
        serviceCollection.AddScoped<ICommandHandler<DeleteCategoryCommand>, DeleteCategoryHandler>();
        serviceCollection.AddScoped<ICommandHandler<DeleteServiceCommand>, DeleteServiceHandler>();
        
        // product
        serviceCollection.AddScoped<ICommandHandler<CreateProductCommand>, CreateProductHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateProductNameCommand>, UpdateProductNameHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateProductPriceCommand>, UpdateProductPriceHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateProductImageUrlCommand>, UpdateProductImageUrlHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateProductInventoryCountCommand>, UpdateProductInventoryCountHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateProductDescriptionCommand>, UpdateProductDescriptionHandler>();
        serviceCollection.AddScoped<ICommandHandler<DeleteProductCommand>, DeleteProductHandler>();
        
        serviceCollection.AddScoped<IDomainEventHandler<OtpCreatedDomainEvent>, OtpCreatedDomainEventHandler>();
        serviceCollection.AddScoped<IDomainEventHandler<AppointmentCreatedDomainEvent>, AppointmentCreatedDomainEventHandler>();
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