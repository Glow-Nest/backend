using Application.AppEntry.Commands.Client;
using Application.Handlers.ClientHandlers;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Common;
using Moq;
using UnitTest.Features.Helpers;
using UnitTest.Features.Helpers.Builders;
using Xunit;

namespace UnitTest.Features.ClientTest.CreateClient.VerifyOtp;

public class VerifyOtpHandlerTest
{
    private readonly IClientRepository _repository;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly IUnitOfWork _unitOfWork;

    public VerifyOtpHandlerTest()
    {
        _repository =  new FakeClientRepository();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _unitOfWork = new FakeUnitOfWork();
    }

    [Fact]
    public async Task HandleAsync_ClientNotFound_ReturnsClientNotFoundError()
    {
        // Arrange
        var command = VerifyOtpCommand.Create("test@example.com","1234").Data ;
        var handler = new VerifyOtpHandler(_repository, _dateTimeProviderMock.Object, _unitOfWork);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ClientErrorMessage.ClientNotFound(), result.Errors);
    }
    

    [Fact]
    public async Task HandleAsync_ValidOtp_ReturnsSuccess()
    {
        // Arrange
        var clientResult = await ClientBuilder.CreateValid().BuildAsync();
        await _repository.AddAsync(clientResult.Data);

        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.UtcNow);
        
        var otp = clientResult.Data.CreateOtp(Purpose.Registration, _dateTimeProviderMock.Object, _unitOfWork).Data;

        var handler = new VerifyOtpHandler(_repository, _dateTimeProviderMock.Object, _unitOfWork);
        var command = VerifyOtpCommand.Create(clientResult.Data.Email.Value,otp.OtpCode.Value).Data ;

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
