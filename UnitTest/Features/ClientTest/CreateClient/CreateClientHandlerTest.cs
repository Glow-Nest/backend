using Application.CommandsDispatching.Commands.Client;
using Application.Handlers.ClientHandlers;
using UnitTest.Features.Helpers;

namespace UnitTest.Features.ClientTest.CreateClient;

public class CreateClientHandlerTest
{
    [Fact]
    public async void ShouldSucceed_WhenValidCommandProvided()
    {
        // Arrange
        var clientRepository = new FakeClientRepository();
        var emailUniqueChecker = new FakeEmailUniqueChecker(clientRepository);
        var unitOfWork = new FakeUnitOfWork();

        var firstNameStr = "Valid";
        var lastNameStr = "User";
        var emailStr = "validU@gmail.com";
        var passwordStr = "Password@123";
        var phoneNumberStr = "91000000";

        var command = CreateClientCommand.Create(firstNameStr, lastNameStr, emailStr, passwordStr, phoneNumberStr).Data;

        var handler = new CreateClientHandler(emailUniqueChecker, clientRepository, unitOfWork);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
    }
}