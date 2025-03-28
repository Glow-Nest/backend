using Application.Interfaces;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Services.Contracts.Client;
using Services.Email;

namespace Services;

public static class ServicesExtension
{
    public static void RegisterContracts(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IEmailUniqueChecker, EmailUniqueChecker>();
        serviceCollection.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }

    public static void RegisterServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IEmailSender, EmailSender>();
    }
}