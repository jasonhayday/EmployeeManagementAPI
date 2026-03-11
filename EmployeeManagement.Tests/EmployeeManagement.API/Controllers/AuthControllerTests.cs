using EmployeeManagement.Infrastructure.Auth;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers.Tests;

public class AuthControllerTests
{
    private const string Secret = "THIS_IS_A_SUPER_SECRET_KEY_FOR_TESTING";

    [Fact]
    public void Login_Should_Return_Ok()
    {
        // Arrange
        var jwtService = new JwtService(Secret);
        var controller = new AuthController(jwtService);

        // Act
        var result = controller.Login("jason");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public void Login_Should_Return_Token()
    {
        // Arrange
        var jwtService = new JwtService(Secret);
        var controller = new AuthController(jwtService);

        // Act
        var result = controller.Login("jason");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);

        var value = okResult.Value!;
        var tokenProperty = value.GetType().GetProperty("token");

        Assert.NotNull(tokenProperty);

        var token = tokenProperty!.GetValue(value) as string;

        Assert.False(string.IsNullOrEmpty(token));
    }
}