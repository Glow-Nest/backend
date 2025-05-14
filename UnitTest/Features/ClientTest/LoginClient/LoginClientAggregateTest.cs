using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Moq;
using OperationResult;
using Services.Authentication;
using UnitTest.Features.Helpers;

namespace UnitTest.Features.ClientTest.LoginClient;

public class LoginClientAggregateTest
{

    [Fact]
    public async Task ShouldSucceed_WhenEmailExists()
    {
        // Arrange
        var email = Email.Create("validClient@example.com").Data;
        var password = Password.Create("Password123!").Data;
        var fullname = FullName.Create("Suhani", "Pandey").Data;
        var phoneNumber = PhoneNumber.Create("91000000").Data;
        
        var clientRepository = new FakeClientRepository();
        var emailUniqueChecker = new FakeEmailUniqueChecker(clientRepository);
        
        var client=Client.Create(fullname, email, password, phoneNumber, emailUniqueChecker);
        await clientRepository.AddAsync(client.Result.Data);
        
        // Act
        var result = await clientRepository.GetAsync(email);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public async Task ShouldSucceed_WhenEmailExists_AndPasswordIsCorrect()
    {
        // Arrange
        var email = Email.Create("validClient@example.com").Data;
        var password = Password.Create("Password123!").Data;
        var fullname = FullName.Create("Suhani", "Pandey").Data;
        var phoneNumber = PhoneNumber.Create("91000000").Data;

        var clientRepository = new FakeClientRepository();
        var emailUniqueChecker = new FakeEmailUniqueChecker(clientRepository);
        
        var clientResult = Client.Create(fullname, email, password, phoneNumber, emailUniqueChecker);
        await clientRepository.AddAsync(clientResult.Result.Data);
        
        // Act
        var result = await clientRepository.GetAsync(email);
        
        // Assert
        Assert.True(result.IsSuccess);
        
        // Verify if password is correct
        var client = result.Data;
        Assert.True(client.Password.Verify("Password123!"));
    }

    [Fact]
    public async Task ShouldSucceed_WhenGeneratingToken()
    {
        // Arrange
        var email = Email.Create("validClient@example.com").Data;
        var password = Password.Create("Password123!").Data;
        var fullname = FullName.Create("Suhani", "Pandey").Data;
        var phoneNumber = PhoneNumber.Create("91000000").Data;

        var clientRepository = new FakeClientRepository();
        var emailUniqueChecker = new FakeEmailUniqueChecker(clientRepository);

        var clientResult = Client.Create(fullname, email, password, phoneNumber, emailUniqueChecker);
        await clientRepository.AddAsync(clientResult.Result.Data);

        await clientRepository.GetAsync(email);
    
        var tokenServiceMock = new Mock<ITokenService>();

        var token = "mocked_token";
        var role = "Client";
        var expiresAt = DateTime.UtcNow.AddHours(1);
        var tokenInfo = TokenInfo.Create(token, role, expiresAt).Data;

        tokenServiceMock
            .Setup(t => t.GenerateTokenAsync(email.Value, role))
            .ReturnsAsync(Result<TokenInfo>.Success(tokenInfo));

        // Act
        var result = await tokenServiceMock.Object.GenerateTokenAsync(email.Value, role);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("mocked_token", result.Data.Token);
        Assert.Equal("Client", result.Data.Role);
        Assert.True(result.Data.ExpiresAt > DateTime.UtcNow);
    }

    [Fact]
    public async Task ShouldFail_WhenEmailDoesNotExist()
    {
        // Arrange
        var email = Email.Create("nonexistent@example.com").Data;
        var clientRepository = new FakeClientRepository();

        // Act
        var result = await clientRepository.GetAsync(email);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldFail_WhenPasswordIsIncorrect()
    {
        // Arrange
        var email = Email.Create("validClient@example.com").Data;
        var correctPassword = Password.Create("Password123!").Data;
        var wrongPassword = "WrongPass456!";
        var fullname = FullName.Create("Suhani", "Pandey").Data;
        var phoneNumber = PhoneNumber.Create("91000000").Data;

        var clientRepository = new FakeClientRepository();
        var emailUniqueChecker = new FakeEmailUniqueChecker(clientRepository);

        var clientResult = Client.Create(fullname, email, correctPassword, phoneNumber, emailUniqueChecker);
        await clientRepository.AddAsync(clientResult.Result.Data);

        // Act
        var result = await clientRepository.GetAsync(email);
        var client = result.Data;

        // Assert
        Assert.False(client.Password.Verify(wrongPassword));
    }

    [Fact]
    public async Task ShouldHashPassword_WhenClientIsCreated()
    {
        // Arrange
        var email = Email.Create("validClient@example.com").Data;
        var plainPassword = "Password123!";
        var password = Password.Create(plainPassword).Data;
        var fullname = FullName.Create("Suhani", "Pandey").Data;
        var phoneNumber = PhoneNumber.Create("91000000").Data;

        var clientRepository = new FakeClientRepository();
        var emailUniqueChecker = new FakeEmailUniqueChecker(clientRepository);

        var clientResult = Client.Create(fullname, email, password, phoneNumber, emailUniqueChecker);
        await clientRepository.AddAsync(clientResult.Result.Data);

        // Act
        var client = (await clientRepository.GetAsync(email)).Data;

        // Assert
        Assert.True(client.Password.Verify(plainPassword));
    }

    
}