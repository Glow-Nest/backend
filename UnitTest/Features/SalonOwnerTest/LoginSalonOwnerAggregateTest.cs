using Domain.Aggregates.Client.Values;
using Domain.Aggregates.SalonOwner;
using Domain.Common.OperationResult;
using Moq;
using Services.Authentication;
using UnitTest.Features.Helpers;

namespace UnitTest.Features.SalonOwnerTest;

public class LoginSalonOwnerAggregateTest
{
    [Fact]
    public async Task ShouldSucceed_WhenEmailExists()
    {
        // Arrange
        var email = Email.Create("validSalon@gmail.com").Data;
        var password = Password.Create("Password123!").Data;
        
        var salonOwnerRepository = new FakeSalonOwnerRepository();
        var salonOwner = SalonOwner.Create(email, password);
        await salonOwnerRepository.AddAsync(salonOwner.Data);
        // Act
        var result = await salonOwnerRepository.GetAsync(email);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public async Task ShouldSucceed_WhenEmailExists_AndPasswordIsCorrect()
    {
        // Arrange
        var email = Email.Create("validSalon@example.com").Data;
        var password = Password.Create("Password123!").Data;

        var salonOwnerRepository = new FakeSalonOwnerRepository();
        var salonOwner = SalonOwner.Create(email, password);
        await salonOwnerRepository.AddAsync(salonOwner.Data);
        
        // Act
        var result = await salonOwnerRepository.GetAsync(email);
        
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
        var email = Email.Create("validSalon@example.com").Data;
        var password = Password.Create("Password123!").Data;

        var salonOwnerRepository = new FakeSalonOwnerRepository();
        var salonOwner = SalonOwner.Create(email, password);
        await salonOwnerRepository.AddAsync(salonOwner.Data);

        await salonOwnerRepository.GetAsync(email);
    
        var tokenServiceMock = new Mock<ITokenService>();

        var token = "mocked_token";
        var role = "Salon Owner";
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
        Assert.Equal("Salon Owner", result.Data.Role);
        Assert.True(result.Data.ExpiresAt > DateTime.UtcNow);
    }

    [Fact]
    public async Task ShouldFail_WhenEmailDoesNotExist()
    {
        // Arrange
        var email = Email.Create("nonexistent@example.com").Data;
        var salonRepository = new FakeSalonOwnerRepository();

        // Act
        var result = await salonRepository.GetAsync(email);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldFail_WhenPasswordIsIncorrect()
    {
        // Arrange
        var email = Email.Create("validSalon@example.com").Data;
        var correctPassword = Password.Create("Password123!").Data;
        var wrongPassword = "WrongPass456!";

        var salonOwnerRepository = new FakeSalonOwnerRepository();
        var salonOwner = SalonOwner.Create(email, correctPassword);
        await salonOwnerRepository.AddAsync(salonOwner.Data);

        // Act
        var result = await salonOwnerRepository.GetAsync(email);
        var client = result.Data;

        // Assert
        Assert.False(client.Password.Verify(wrongPassword));
    }

    [Fact]
    public async Task ShouldHashPassword_WhenSalonIsCreated()
    {
        // Arrange
        var email = Email.Create("validSalon@example.com").Data;
        var plainPassword = "Password123!";
        var password = Password.Create(plainPassword).Data;

        var salonOwnerRepository = new FakeSalonOwnerRepository();
        var salonOwner = SalonOwner.Create(email, password);
        await salonOwnerRepository.AddAsync(salonOwner.Data);

        // Act
        var client = (await salonOwnerRepository.GetAsync(email)).Data;

        // Assert
        Assert.True(client.Password.Verify(plainPassword));
    }
}