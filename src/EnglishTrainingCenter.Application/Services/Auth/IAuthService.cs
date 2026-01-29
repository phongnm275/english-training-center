using EnglishTrainingCenter.Application.DTOs.Auth;
using EnglishTrainingCenter.Domain.Entities;

namespace EnglishTrainingCenter.Application.Services.Auth;

/// <summary>
/// Authentication service interface for login, registration, and token management
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Login user with credentials
    /// </summary>
    Task<AuthResponse> LoginAsync(LoginRequest request);

    /// <summary>
    /// Register new user
    /// </summary>
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Refresh access token
    /// </summary>
    Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request);

    /// <summary>
    /// Change user password
    /// </summary>
    Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request);

    /// <summary>
    /// Logout user
    /// </summary>
    Task<bool> LogoutAsync(int userId);

    /// <summary>
    /// Get user by username
    /// </summary>
    Task<User?> GetUserByUsernameAsync(string username);

    /// <summary>
    /// Verify user password
    /// </summary>
    bool VerifyPassword(string password, string hash);
}
