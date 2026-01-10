using Microsoft.AspNetCore.Mvc;
using Skwela.Application.Interfaces;

namespace Skwela.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var token = await _authService.LoginAsync(
            request.Username,
            request.Password
        );

        return Ok(new { token });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupRequest request)
    {
        var userId = await _authService.SignupAsync(
            request.Username,
            request.Password
        );

        return Ok(new { userId });
    }
}