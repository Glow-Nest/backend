using Domain.Aggregates.Client.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Services.Contracts.Client;

namespace Services;

public static class ServicesExtension
{
    public static void RegisterContracts(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IEmailUniqueChecker, EmailUniqueChecker>();
    }
}