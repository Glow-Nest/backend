using Application.AppEntry.Commands.Client;
using Domain.Aggregates.Client;

namespace UnitTest.Features.ClientTest.CreateClient.VerifyOtp;

public class VerifyOtpCommandTest
{
    [Fact]
    public void Create_WithValidEmailAndOtpCode_ShouldReturnSuccessResult()
    {
        // Arrange
        var email = "test@example.com";
        var otpCode = "1234";

        // Act
        var result = VerifyOtpCommand.Create(email, otpCode);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public void Create_WithInvalidEmail_ShouldReturnFailResult()
    {
        // Arrange
        var email = "invalid-email";
        var otpCode = "1234";

        // Act
        var result = VerifyOtpCommand.Create(email, otpCode);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ClientErrorMessage.InvalidEmailFormat(), result.Errors );
    }

    [Fact]
    public void Create_WithInvalidOtpCode_ShouldReturnFailResult()
    {
        // Arrange
        var email = "test@example.com";
        var otpCode = "invalid-otp";

        // Act
        var result = VerifyOtpCommand.Create(email, otpCode);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ClientErrorMessage.InvalidOtp(), result.Errors);
    }
}
