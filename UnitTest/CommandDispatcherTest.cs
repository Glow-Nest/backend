using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Application.Handlers.ClientHandlers;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Common;
using Microsoft.Extensions.DependencyInjection;
using UnitTest.Features.Helpers;

namespace UnitTest;

public class CommandDispatcherTest
{
    [Fact]
    public async Task DispatchAsync_ShouldCallCorrectHandler_AndReturnResult()
    {
        // Arrange
        var firstNameStr = "Valid";
        var lastNameStr = "User";
        var emailStr = "validU@gmail.com";
        var passwordStr = "Password@123";
        var phoneNumberStr = "91000000";
        
        var command = CreateClientCommand.Create(firstNameStr, lastNameStr, emailStr, passwordStr, phoneNumberStr).Data;
        
        var serviceProvider = new ServiceCollection()
            .AddTransient<ICommandHandler<CreateClientCommand>, CreateClientHandler>()
            .AddSingleton<IEmailUniqueChecker, FakeEmailUniqueChecker>()
            .AddSingleton<IClientRepository, FakeClientRepository>()
            .AddSingleton<IUnitOfWork, FakeUnitOfWork>()
            .BuildServiceProvider();
        
        var dispatcher = new CommandDispatcher(serviceProvider);
        
        // Act
        var result = await dispatcher.DispatchAsync(command);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
}