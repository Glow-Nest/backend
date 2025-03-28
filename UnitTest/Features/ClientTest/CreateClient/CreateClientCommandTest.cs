using Application.AppEntry.Commands.Client;
using Domain.Aggregates.Client;

namespace UnitTest.Features.ClientTest.CreateClient;

public class CreateClientCommandTest
{
    [Fact]
    public void ShouldSucceed_WhenValidInputsProvided()
    {
        // Arrange
        var firstNameStr = "Valid";
        var lastNameStr = "User";
        var emailStr = "validU@gmail.com";
        var passwordStr = "Password@123";
        var phoneNumberStr = "91000000";

        // Act
        var result = CreateClientCommand.Create(firstNameStr, lastNameStr, emailStr, passwordStr, phoneNumberStr);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ShouldFail_WhenInvalidFirstNameProvided()
    {
        // Arrange
        var firstNameStr = "V";
        var lastNameStr = "User";
        var emailStr = "validU@gmail.com";
        var passwordStr = "Password@123";
        var phoneNumberStr = "91000000";
        
        // Act
        var result = CreateClientCommand.Create(firstNameStr, lastNameStr, emailStr, passwordStr, phoneNumberStr);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ClientErrorMessage.InvalidFirstName(), result.Errors);
    }
}