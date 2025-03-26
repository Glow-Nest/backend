using Application.AppEntry.Commands.Client;
using Domain.Aggregates.Client;

namespace UnitTest.Features.ClientTest.CreateClient.CreateOtp;

public class CreateOtpCommandTest
{
    [Fact]
    public void ShouldSucceed_WhenValidCommandProvided()
    {
        // Arrange
        string email = "validU@gmail.com";
        string purpose = "Registration";

        // Act
        var result = CreateOtpCommand.Create(email, purpose);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void ShouldFail_WhenInvalidPurposeProvided()
    {
        // Arrange
        string email = "validU@gmail.com";
        string purpose = "InvalidPurpose";

        // Act
        var result = CreateOtpCommand.Create(email, purpose);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ClientErrorMessage.OtpPurposeMismatch(), result.Errors);
    }
}