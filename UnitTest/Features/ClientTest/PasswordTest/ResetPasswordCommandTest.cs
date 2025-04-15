using Application.AppEntry.Commands.Client;

namespace UnitTest.Features.ClientTest.PasswordTest;

public class ResetPasswordCommandTest
{
    [Fact]
    public void ShouldReturnSuccess_WhenInputsAreValid()
    {
        // Arrange
        var email = "user@example.com";
        var password = "ValidPassword123!";
        var otp = "1234";

        // Act
        var result = ResetPasswordCommand.Create(email, password, password, otp);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
    }
    
    [Fact]
    public void ShouldReturnFailure_WhenPasswordsDoNotMatch()
    {
        // Arrange
        var email = "user@example.com";
        var newPassword = "Password123!";
        var confirmPassword = "DifferentPassword!";
        var otp = "1234";

        // Act
        var result = ResetPasswordCommand.Create(email, newPassword, confirmPassword, otp);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Message.Contains("Password"));
    }
    
    [Fact]
    public void ShouldReturnFailure_WhenEmailIsInvalid()
    {
        // Arrange
        var email = "not-an-email";
        var password = "ValidPassword123!";
        var otp = "1234";

        // Act
        var result = ResetPasswordCommand.Create(email, password, password, otp);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Message.Contains("Email"));
    }

}