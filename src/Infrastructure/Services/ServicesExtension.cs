using Application.Interfaces;
using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Order.Contracts;
using Domain.Aggregates.Schedule.Contracts;
using Domain.Common.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Services.Authentication;
// using Services.Cloudinary;
using Services.Contracts.Appointment;
using Services.Contracts.Client;
using Services.Contracts.Common;
using Services.Contracts.Order;
using Services.Contracts.Schedule;
using Services.Email;
using Services.Jobs;

namespace Services;

public static class ServicesExtension
{
    public static void RegisterServices(this IServiceCollection serviceCollection)
    {
        RegisterContracts(serviceCollection);
        RegisterApplicationServices(serviceCollection);
    }

    private static void RegisterContracts(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IEmailUniqueChecker, EmailUniqueChecker>();
        serviceCollection.AddScoped<IDateTimeProvider, DateTimeProvider>();
        serviceCollection.AddScoped<IClientChecker, ClientChecker>();
        serviceCollection.AddScoped<IServiceChecker, ServiceChecker>();
        serviceCollection.AddScoped<IBlockedTimeChecker, BlockedTimeChecker>();

        // order
        serviceCollection.AddScoped<IProductChecker, ProductChecker>();
    }

    private static void RegisterApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IEmailSender, EmailSender>();
        serviceCollection.AddScoped<ITokenService, TokenService>();
        // serviceCollection.AddScoped<ICloudinaryService, CloudinaryService>();

        serviceCollection.AddScoped<ScheduleSeederJob>();
    }
}