using Application.AppEntry.Commands.Client.UpdateClient;
using Domain.Aggregates.Client;

namespace UnitTest.Features.ClientTest.UpdatePhoneNumberTest;

public class UpdatePhoneNumberCommandTest
{
    [Fact]
    public void ShouldSucceed_WhenValidInputsProvided()
    {
        // Arrange
        var phoneNumberStr = "91000000";
        var clientIdStr = "12345678-1234-1234-1234-123456789012";

        // Act
        var result = UpdatePhoneNumberCommand.Create(clientIdStr, phoneNumberStr);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ShouldFail_WhenInvalidPhoneNumberProvided()
    {
        // Arrange
        var phoneNumberStr = "";
        var clientIdStr = "12345678-1234-1234-1234-123456789012";

        // Act
        var result = UpdatePhoneNumberCommand.Create(clientIdStr, phoneNumberStr);

        // Assert
        Assert.False(result.IsSuccess);
    }
}