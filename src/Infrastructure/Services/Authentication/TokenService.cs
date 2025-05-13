using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OperationResult;

namespace Services.Authentication;

public class TokenService:ITokenService
{
    private readonly IConfiguration _configuration;
    
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task<Result<TokenInfo>> GenerateTokenAsync(string email,string role)
    {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["JwtSetting:SecretKey"];
            
            if (string.IsNullOrEmpty(secretKey))
            {
                return Task.FromResult(Result<TokenInfo>.Fail(GenericErrorMessage.MissingConfiguration()));
            }

            // Ensure key is at least 128 bits long (16 characters for UTF-8 encoding)
            var key = Encoding.UTF8.GetBytes(secretKey);
            if (key.Length < 16)
            {
                return Task.FromResult(Result<TokenInfo>.Fail(GenericErrorMessage.KeyIsTooShort()));
            }
            
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Role, role),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(24),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Audience = "GlowNestAPI",
                Issuer = "GlowNestServer",
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            
            var tokenInfoResult = TokenInfo.Create(tokenString, role, tokenDescriptor.Expires.Value);
            
            return Task.FromResult(tokenInfoResult);
        }
}