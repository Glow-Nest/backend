using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Application.AppEntry.Dispatchers;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Common;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OperationResult;

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
        
        var mockHandler = new Mock<ICommandHandler<CreateClientCommand, None>>();
        mockHandler.Setup(h => h.HandleAsync(It.IsAny<CreateClientCommand>())).ReturnsAsync(Result<None>.Success(None.Value));
        
        var mockEmailChecker = new Mock<IEmailUniqueChecker>();
        var mockClientRepository = new Mock<IClientRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        
        var serviceProvider = new ServiceCollection()
            .AddSingleton(_ => mockHandler.Object)
            .AddSingleton(_ => mockEmailChecker.Object)
            .AddSingleton(_ => mockClientRepository.Object)
            .AddSingleton(_ => mockUnitOfWork.Object)
            .BuildServiceProvider();
        
        var dispatcher = new CommandDispatcher(serviceProvider);
        
        // Act
        var result = await dispatcher.DispatchAsync<CreateClientCommand, None>(command);
        
        // Assert
        Assert.True(result.IsSuccess);
        mockHandler.Verify(h => h.HandleAsync(It.IsAny<CreateClientCommand>()), Times.Once);
    }
}
