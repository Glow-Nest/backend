using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Application.AppEntry.Commands.Client.UpdateClient;
using Application.AppEntry.Commands.Order;
using Application.AppEntry.Commands.Product;
using Application.AppEntry.Commands.Product.UpdateProduct;
using Application.AppEntry.Commands.ProductReview;
using Application.AppEntry.Commands.Schedule;
using Application.AppEntry.Commands.ServiceCategory;
using Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;
using Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;
using Application.AppEntry.Commands.ServiceReview;
using Application.AppEntry.Decorators;
using Application.AppEntry.Dispatchers;
using Application.Handlers.ClientHandlers;
using Application.Handlers.ClientHandlers.UpdateClientHandler;
using Application.Handlers.DomainEvents;
using Application.Handlers.OrderHandlers;
using Application.Handlers.ProductHandlers;
using Application.Handlers.ProductHandlers.UpdateProductHandler;
using Application.Handlers.ProductReviewHandlers;
using Application.Handlers.ScheduleHandlers;
using Application.Handlers.ServiceCategoryHandlers;
using Application.Handlers.ServiceCategoryHandlers.UpdateCategoryHandler;
using Application.Handlers.ServiceCategoryHandlers.UpdateServiceHandler;
using Application.Handlers.ServiceReviewHandler;
using Domain.Aggregates.Client.DomainEvents;
using Domain.Aggregates.Order.Values;
using Domain.Aggregates.Schedule.DomainEvents;
using Domain.Common;
using Microsoft.Extensions.DependencyInjection;
using OperationResult;
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
        serviceCollection.AddScoped<ICommandHandler<CreateClientCommand, None>, CreateClientHandler>();
        serviceCollection.AddScoped<ICommandHandler<CreateOtpCommand, None>, CreateOtpHandler>();
        serviceCollection.AddScoped<ICommandHandler<VerifyOtpCommand, None>, VerifyOtpHandler>();
        serviceCollection.AddScoped<ICommandHandler<ResetPasswordCommand, None>, ResetPasswordHandler>();
        serviceCollection.AddScoped<ICommandHandler<InitiateResetPasswordCommand, None>, InitiateResetPasswordHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateFullNameCommand, None>, UpdateFullNameHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdatePhoneNumberCommand, None>, UpdatePhoneNumberHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdatePasswordCommand, None>, UpdatePasswordHandler>();

        // schedule
        serviceCollection.AddScoped<ICommandHandler<CreateAppointmentCommand, None>, CreateAppointmentHandler>();
        serviceCollection.AddScoped<ICommandHandler<AddBlockedTimeCommand, None>, AddBlockedTimeHandler>();
        serviceCollection.AddScoped<ICommandHandler<CreateFutureSchedulesCommand, None>, CreateFutureSchedulesHandler>();

        // category and service
        serviceCollection.AddScoped<ICommandHandler<CreateCategoryCommand, None>, CreateCategoryHandler>();
        serviceCollection.AddScoped<ICommandHandler<AddServiceInCategoryCommand, None>, AddServiceInCategoryHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateCategoryNameCommand, None>, UpdateCategoryNameHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateCategoryDescriptionCommand, None>, UpdateCategoryDescriptionHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateMediaUrlCommand, None>, UpdateMediaUrlHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateServiceNameCommand, None>, UpdateServiceNameHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateServiceDurationCommand, None>, UpdateServiceDurationHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateServicePriceCommand, None>, UpdateServicePriceHandler>();
        serviceCollection.AddScoped<ICommandHandler<DeleteCategoryCommand, None>, DeleteCategoryHandler>();
        serviceCollection.AddScoped<ICommandHandler<DeleteServiceCommand, None>, DeleteServiceHandler>();
        
        // product
        serviceCollection.AddScoped<ICommandHandler<CreateProductCommand, None>, CreateProductHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateProductNameCommand, None>, UpdateProductNameHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateProductPriceCommand, None>, UpdateProductPriceHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateProductImageUrlCommand, None>, UpdateProductImageUrlHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateProductInventoryCountCommand, None>, UpdateProductInventoryCountHandler>();
        serviceCollection.AddScoped<ICommandHandler<UpdateProductDescriptionCommand, None>, UpdateProductDescriptionHandler>();
        serviceCollection.AddScoped<ICommandHandler<DeleteProductCommand, None>, DeleteProductHandler>();
        
        // order
        serviceCollection.AddScoped<ICommandHandler<CreateOrderCommand, OrderId>, CreateOrderHandler>();
        
        //service review
        serviceCollection.AddScoped<ICommandHandler<CreateServiceReviewCommand, None>, CreateServiceReviewHandler>();
        
        // product review
        serviceCollection.AddScoped<ICommandHandler<CreateProductReviewCommand, None>, CreateProductReviewHandler>();
        
        // domain events
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