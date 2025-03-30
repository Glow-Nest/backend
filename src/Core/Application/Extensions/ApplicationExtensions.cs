using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Application.Handlers.ClientHandlers;
using Application.Login;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions;

public static class ApplicationExtensions
{
    public static void RegisterHandlers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICommandHandler<CreateClientCommand>, CreateClientHandler>();
        serviceCollection.AddSingleton<ICommandHandler<CreateOtpCommand>, CreateOtpHandler>();
        serviceCollection.AddSingleton<ICommandHandler<VerifyOtpCommand>, VerifyOtpHandler>();
        serviceCollection.AddSingleton<ICommandHandler<LoginUserCommand, LoginResponse>, LoginUserHandler>();
    }

    public static void RegisterDispatcher(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ICommandDispatcher, CommandDispatcher>();
    }
}