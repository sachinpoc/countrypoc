using Microsoft.Extensions.Configuration;
using Moq;
using MyCleanArchitectureApp.Infrastructure.Services;
using System.Threading.Tasks;
using Xunit;

public class TokenServiceTests
{
    [Fact]
    public async Task GenerateToken_ReturnsToken()
    {
        // Arrange
        var secretKey = "YourSecretKeyHere";
        var issuer = "YourIssuerHere";
        var audience = "YourAudienceHere";

        var tokenService = new TokenService(secretKey, issuer, audience);

        // Act
        var token = await tokenService.GenerateToken("testUser");

        // Assert
        Assert.NotNull(token);
        Assert.Contains("testUser", token); 
    }
}
