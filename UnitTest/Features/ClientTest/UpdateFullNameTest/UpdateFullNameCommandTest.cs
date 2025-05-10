using Application.AppEntry.Commands.Client.UpdateClient;
using Domain.Aggregates.Client;

namespace UnitTest.Features.ClientTest.UpdateFullNameTest;

public class UpdateFullNameCommandTest
{
    
    [Fact]
    public void ShouldSucceed_WhenValidInputsProvided()
    {
        // Arrange
        var firstNameStr = "Valid";
        var lastNameStr = "User";
        var clientIdStr = "12345678-1234-1234-1234-123456789012";

        // Act
        var result = UpdateFullNameCommand.Create(clientIdStr,firstNameStr, lastNameStr);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ShouldFail_WhenInvalidFirstNameProvided()
    {
        // Arrange
        var firstNameStr = "";
        var lastNameStr = "User";
        var clientIdStr = "12345678-1234-1234-1234-123456789012";

        // Act
        var result = UpdateFullNameCommand.Create(clientIdStr,firstNameStr, lastNameStr);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ClientErrorMessage.InvalidFirstName(), result.Errors);
    }
}