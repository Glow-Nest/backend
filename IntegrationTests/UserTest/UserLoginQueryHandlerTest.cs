using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.SalonOwner;
using EfcQueries.Queries;
using IntegrationTests.Helpers;
using Microsoft.Extensions.Configuration;
using Moq;
using OperationResult;
using QueryContracts.Queries;
using Services.Authentication;
using UnitTest.Features.Helpers;

namespace IntegrationTests.UserTest;

public class UserLoginQueryHandlerTest
{
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly DomainContextHelper _context;
    
    public UserLoginQueryHandlerTest()
    {
        _mockTokenService = new Mock<ITokenService>();
        _mockConfiguration = new Mock<IConfiguration>();
        
        _mockConfiguration
            .Setup(config => config.GetSection("SalonOwner:Email").Value)
            .Returns("salonowner@gmail.com");

        _context = DomainContextHelper.SetupContext();
    }
    
    [Fact]
    public async Task Should_Successfully_Login_SalonOwner_With_Valid_Credentials()
    {
        // Arrange
        var email = Email.Create("salonowner@gmail.com");
        var rawPassword = "Suhani1@";
        var password = Password.Create(rawPassword);
        var salonOwner = SalonOwner.Create(email.Data, password.Data).Data;

        await DomainContextHelper.SaveAndClearAsync(salonOwner, _context);
        var query = new LoginUserQuery(email.Data.Value, rawPassword);

        var token = "mocked_token";
        var role = "Salon Owner";
        var expiresAt = DateTime.UtcNow.AddDays(1);
        var tokenInfo = TokenInfo.Create(token, role, expiresAt).Data;
        
        _mockTokenService
            .Setup(t => t.GenerateTokenAsync(email.Data.Value, role))
            .ReturnsAsync(Result<TokenInfo>.Success(tokenInfo));

        var handler = new LoginUserQueryHandler(_context, _mockTokenService.Object, _mockConfiguration.Object);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("mocked_token", result.Data.Token);
        Assert.Equal("Salon Owner", result.Data.Role);
    }

    [Fact]
    public async Task Should_Fail_Login_SalonOwner_With_Invalid_Password()
    {
        // Arrange
        var email = Email.Create("salonowner@example.com");
        var password = Password.Create("Suhani1@");
        var salonOwner = SalonOwner.Create(email.Data, password.Data).Data;

        await DomainContextHelper.SaveAndClearAsync(salonOwner, _context);

        var query = new LoginUserQuery(email.Data.Value, Password.Create("WrongPassword1@").Data.Value);

        var handler = new LoginUserQueryHandler(_context, _mockTokenService.Object, _mockConfiguration.Object);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async Task Should_Successfully_Login_Client_With_Valid_Credentials()
    {
        // Arrange
        var email = Email.Create("validClient@example.com").Data;
        var rawPassword = "Suhani1@";
        var password = Password.Create(rawPassword).Data;
        var fullname = FullName.Create("Suhani", "Pandey").Data;
        var phoneNumber = PhoneNumber.Create("91000000").Data;

        var clientRepository = new FakeClientRepository();
        var emailUniqueChecker = new FakeEmailUniqueChecker(clientRepository);
        
        var clientResult = await Client.Create(fullname, email, password, phoneNumber, emailUniqueChecker);
        var client = clientResult.Data;
        client.IsVerified = true;
    
        await DomainContextHelper.SaveAndClearAsync(client, _context);

        var query = new LoginUserQuery(email.Value, rawPassword);

        var token = "mocked_token";
        var role = "Client";
        var expiresAt = DateTime.UtcNow.AddHours(1);
        var tokenInfo = TokenInfo.Create(token, role, expiresAt).Data;
    
        _mockTokenService
            .Setup(t => t.GenerateTokenAsync(email.Value, role))
            .ReturnsAsync(Result<TokenInfo>.Success(tokenInfo));

        var handler = new LoginUserQueryHandler(_context, _mockTokenService.Object, _mockConfiguration.Object);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("mocked_token", result.Data.Token);
        Assert.Equal("Client", result.Data.Role);
    }

    [Fact]
    public async Task Should_Fail_Login_Client_With_Unverified_Status()
    {
        // Arrange
        var email = Email.Create("validClient@example.com").Data;
        var password = Password.Create("Password123!").Data;
        var fullname = FullName.Create("Suhani", "Pandey").Data;
        var phoneNumber = PhoneNumber.Create("91000000").Data;

        var clientRepository = new FakeClientRepository();
        var emailUniqueChecker = new FakeEmailUniqueChecker(clientRepository);

        var clientResult = Client.Create(fullname, email, password, phoneNumber, emailUniqueChecker);
        if (clientResult.Result.IsSuccess)
        {
            var client = clientResult.Result.Data; 
            await DomainContextHelper.SaveAndClearAsync(client, _context);
        }

        var query = new LoginUserQuery(email.Value, password.Value);

        var handler = new LoginUserQueryHandler(_context, _mockTokenService.Object, _mockConfiguration.Object);

        // Act
        var result = await handler.HandleAsync(query);

        // Assert
        Assert.False(result.IsSuccess);
    }
}
