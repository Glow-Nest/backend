using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Entities;
using Domain.Aggregates.Client.Values;
using Domain.Common;
using Domain.Common.Contracts;
using Moq;
using UnitTest.Features.Helpers.Builders;

namespace UnitTest.Features.ClientTest.CreateClient.VerifyOtp;

public class VerifyOtpAggregateTest
{
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
    private readonly Mock<IUnitOfWork> _mockUnitOfWork = new();

    [Fact]
    public async Task VerifyOtp_ShouldReturnSuccess_WhenOtpIsCorrectAndNotExpired()
    {
        // Arrange
        var now = DateTime.UtcNow;
        _mockDateTimeProvider.Setup(d => d.GetNow()).Returns(now.AddMinutes(5));

        var client = (await ClientBuilder.CreateValid().BuildAsync()).Data;
        var otpSession = client.CreateOtp(Purpose.Registration, _mockDateTimeProvider.Object).Data;

        // Act
        var result = client.VerifyOtp(otpSession.OtpCode, _mockDateTimeProvider.Object);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(client.IsVerified);
    }

    [Fact]
    public async Task VerifyOtp_ShouldFail_WhenOtpIsIncorrect()
    {
        // Arrange
        var now = DateTime.UtcNow;
        _mockDateTimeProvider.Setup(d => d.GetNow()).Returns(now.AddMinutes(5));

        var client = (await ClientBuilder.CreateValid().BuildAsync()).Data;

        var newOtpCode = OtpCode.New().Data;
        
        var otpSession = client.CreateOtp(Purpose.Registration, _mockDateTimeProvider.Object).Data;

        // Act
        var result = client.VerifyOtp(newOtpCode, _mockDateTimeProvider.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ClientErrorMessage.InvalidOtp(), result.Errors);
    }

    [Fact]
    public async Task VerifyOtp_ShouldFail_WhenOtpIsExpired()
    {
        // Arrange
        var now = DateTime.UtcNow;
        _mockDateTimeProvider.Setup(d => d.GetNow()).Returns(now);

        var client = (await ClientBuilder.CreateValid().BuildAsync()).Data;
        var otpSession = client.CreateOtp(Purpose.Registration, _mockDateTimeProvider.Object).Data;

        _mockDateTimeProvider.Setup(d => d.GetNow()).Returns(now.AddMinutes(11));

        // Act
        var result = otpSession.VerifyOtp(otpSession.OtpCode, client.Email, _mockDateTimeProvider.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ClientErrorMessage.OtpExpired(), result.Errors);
    }
}