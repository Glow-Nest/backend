using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Entities;
using Domain.Aggregates.Client.Values;
using Services.Contracts.Common;
using UnitTest.Features.Helpers.Builders;

namespace UnitTest.Features.ClientTest.PasswordTest;

public class ResetPasswordAggregateTest
{
    
    [Fact]
    public async Task Should_ResetPassword_Successfully_WhenOtpIsUsedAndPurposeIsPasswordReset()
    {
        // Arrange
        var result = await ClientBuilder.CreateValid().BuildAsync();
        var client = result.Data;

        var newPassword = Password.Create("NewPassword123!").Data;

        var otpSession = OtpSession.Create(client.Email, Purpose.PasswordReset, new DateTimeProvider()).Data;
        otpSession.IsUsed = true;

        SetPrivateOtpSession(client, otpSession);

        // Act
        var resetResult = client.ResetPassword(newPassword);

        // Assert
        Assert.True(resetResult.IsSuccess);
    }
    
    [Fact]
    public async Task ShouldFail_ResetPassword_WhenOtpSessionIsNull()
    {
        // Arrange
        var result = await ClientBuilder.CreateValid().BuildAsync();
        var client = result.Data;

        var newPassword = Password.Create("AnotherNewPassword123!").Data;

        // Act
        var resetResult = client.ResetPassword(newPassword);

        // Assert
        Assert.False(resetResult.IsSuccess);
    }
    
    [Fact]
    public async Task ShouldFail_ResetPassword_WhenOtpSessionHasWrongPurpose()
    {
        // Arrange
        var result = await ClientBuilder.CreateValid().BuildAsync();
        var client = result.Data;

        var otpSession = OtpSession.Create(client.Email, Purpose.Registration, new DateTimeProvider()).Data;
        otpSession.IsUsed = true;

        SetPrivateOtpSession(client, otpSession);

        var newPassword = Password.Create("MismatchPurpose123!").Data;

        // Act
        var resetResult = client.ResetPassword(newPassword);

        // Assert
        Assert.False(resetResult.IsSuccess);
    }
    
    [Fact]
    public async Task ShouldFail_ResetPassword_WhenOtpSessionIsNotUsed()
    {
        // Arrange
        var result = await ClientBuilder.CreateValid().BuildAsync();
        var client = result.Data;

        var otpSession = OtpSession.Create(client.Email, Purpose.PasswordReset, new DateTimeProvider()).Data;
        otpSession.IsUsed = false;

        SetPrivateOtpSession(client, otpSession);

        var newPassword = Password.Create("UnverifiedOTP123!").Data;

        // Act
        var resetResult = client.ResetPassword(newPassword);

        // Assert
        Assert.False(resetResult.IsSuccess);
    }
    private void SetPrivateOtpSession(Client client, OtpSession otpSession)
    {
        var field = typeof(Client).GetProperty("OtpSession", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field?.SetValue(client, otpSession);
    }
}