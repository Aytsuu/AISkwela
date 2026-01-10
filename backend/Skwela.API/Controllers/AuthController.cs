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
        try
        {
            var token = await _authService.LoginAsync(
                request.username,
                request.password
            );

            return Ok(new { token });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid credentials.");
        }
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup(SignupRequest request)
    {   
        try
        {
            var userId = await _authService.SignupAsync(
                request.username,
                request.password
            );
            return Ok(new { userId });
        }
        catch (InvalidDataException)
        {
            return BadRequest("Signup Failed.");
        }

    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequest request)
    {
        try
        {
            var result = await _authService.RefreshTokenAsync(request.accessToken, request.refreshToken);
            return Ok(result);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid token.");
        }
    }
}