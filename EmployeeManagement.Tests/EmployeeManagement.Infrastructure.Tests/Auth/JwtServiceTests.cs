using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EmployeeManagement.Infrastructure.Auth.Tests;

public class JwtServiceTests
{
    private readonly JwtService _jwtService;

    public JwtServiceTests()
    {
        _jwtService = new JwtService("THIS_IS_SUPER_SECRET_KEY_FOR_TESTING_123456");
    }

    [Fact]
    public void GenerateToken_ShouldReturnToken()
    {
        var token = _jwtService.GenerateToken("jason");

        Assert.False(string.IsNullOrEmpty(token));
    }

    [Fact]
    public void GenerateToken_ShouldContainUsernameClaim()
    {
        var token = _jwtService.GenerateToken("jason");

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var usernameClaim = jwtToken.Claims.First(c => c.Type == ClaimTypes.Name);

        Assert.Equal("jason", usernameClaim.Value);
    }

    [Fact]
    public void GenerateToken_ShouldHaveExpiration()
    {
        var token = _jwtService.GenerateToken("jason");

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
    }
}