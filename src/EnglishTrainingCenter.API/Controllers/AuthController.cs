using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EnglishTrainingCenter.Application.DTOs.Auth;
using EnglishTrainingCenter.Application.Services.Auth;
using EnglishTrainingCenter.Common.Utilities;

namespace EnglishTrainingCenter.API.Controllers;

/// <summary>
/// Authentication endpoints controller
/// </summary>
[ApiController]
[Route("api/v1/auth")]
[ApiVersion("1.0")]
public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Login user with credentials
    /// </summary>
    /// <param name="request">Login request with username and password</param>
    /// <returns>Auth response with tokens</returns>
    /// <response code="200">Login successful</response>
    /// <response code="400">Invalid credentials</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation($"Login endpoint called for user: {request.Username}");

        var response = await _authService.LoginAsync(request);

        if (!response.Success)
        {
            return BadRequest(ApiResponse<AuthResponse>.Error(response.Message, new { response }));
        }

        return Ok(ApiResponse<AuthResponse>.Success(response, response.Message));
    }

    /// <summary>
    /// Register new user account
    /// </summary>
    /// <param name="request">Registration request</param>
    /// <returns>Auth response with tokens</returns>
    /// <response code="201">User registered successfully</response>
    /// <response code="400">Registration failed</response>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation($"Register endpoint called for user: {request.Username}");

        var response = await _authService.RegisterAsync(request);

        if (!response.Success)
        {
            return BadRequest(ApiResponse<AuthResponse>.Error(response.Message, new { response }));
        }

        return CreatedAtAction(nameof(Register), response);
    }

    /// <summary>
    /// Refresh access token
    /// </summary>
    /// <param name="request">Refresh token request</param>
    /// <returns>New access token</returns>
    /// <response code="200">Token refreshed successfully</response>
    /// <response code="400">Invalid refresh token</response>
    [HttpPost("refresh")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<AuthResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        _logger.LogInformation("Refresh token endpoint called");

        var response = await _authService.RefreshTokenAsync(request);

        if (!response.Success)
        {
            return BadRequest(ApiResponse<AuthResponse>.Error(response.Message, new { response }));
        }

        return Ok(ApiResponse<AuthResponse>.Success(response, response.Message));
    }

    /// <summary>
    /// Change user password
    /// </summary>
    /// <param name="request">Change password request</param>
    /// <returns>Success message</returns>
    /// <response code="200">Password changed successfully</response>
    /// <response code="400">Password change failed</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost("change-password")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var userId = GetUserIdFromToken();
        if (userId <= 0)
        {
            return Unauthorized(ApiResponse<object>.Error("User not authenticated"));
        }

        _logger.LogInformation($"Change password endpoint called for user: {userId}");

        var result = await _authService.ChangePasswordAsync(userId, request);

        if (!result)
        {
            return BadRequest(ApiResponse<object>.Error("Failed to change password"));
        }

        return Ok(ApiResponse<object>.Success(new { message = "Password changed successfully" }));
    }

    /// <summary>
    /// Logout user
    /// </summary>
    /// <returns>Success message</returns>
    /// <response code="200">Logout successful</response>
    /// <response code="401">Unauthorized</response>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        var userId = GetUserIdFromToken();
        if (userId <= 0)
        {
            return Unauthorized(ApiResponse<object>.Error("User not authenticated"));
        }

        _logger.LogInformation($"Logout endpoint called for user: {userId}");

        var result = await _authService.LogoutAsync(userId);

        if (!result)
        {
            return BadRequest(ApiResponse<object>.Error("Logout failed"));
        }

        return Ok(ApiResponse<object>.Success(new { message = "Logout successful" }));
    }

    /// <summary>
    /// Get current user profile (requires authentication)
    /// </summary>
    /// <returns>Current user information</returns>
    /// <response code="200">User profile retrieved</response>
    /// <response code="401">Unauthorized</response>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetCurrentUser()
    {
        var username = GetUsernameFromToken();
        var userRole = GetRoleFromToken();

        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized(ApiResponse<object>.Error("User not authenticated"));
        }

        return Ok(ApiResponse<object>.Success(new
        {
            username = username,
            role = userRole,
            timestamp = DateTime.UtcNow
        }));
    }
}
