using Application.Interfaces;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Services.Authentication;
using Services.Contracts.Client;
using Services.Email;

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
    }

    private static void RegisterApplicationServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IEmailSender, EmailSender>();
        serviceCollection.AddScoped<ITokenService, TokenService>();
    }
}