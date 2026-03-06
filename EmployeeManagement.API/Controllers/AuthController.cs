using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Infrastructure.Auth;

namespace EmployeeManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwt;

    public AuthController(JwtService jwt)
    {
        _jwt = jwt;
    }

    [HttpPost("login")]
    public IActionResult Login(string username)
    {
        var token = _jwt.GenerateToken(username);
        return Ok(new { token });
    }
}