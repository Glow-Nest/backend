using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Application.Handlers.ClientHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationExtensions
{
    public static void RegisterHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICommandHandler<CreateClientCommand>, CreateClientHandler>();
    }

    public static void RegisterDispatcher(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICommandDispatcher, CommandDispatcher>();
    }
}