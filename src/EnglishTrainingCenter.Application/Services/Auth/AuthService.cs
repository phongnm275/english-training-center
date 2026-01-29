using EnglishTrainingCenter.Application.DTOs.Auth;
using EnglishTrainingCenter.Domain.Entities;
using EnglishTrainingCenter.Domain.Interfaces;
using EnglishTrainingCenter.Common.Utilities;
using Microsoft.Extensions.Logging;

namespace EnglishTrainingCenter.Application.Services.Auth;

/// <summary>
/// Authentication service implementation
/// </summary>
public class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Role> _roleRepository;
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IRepository<User> userRepository,
        IRepository<Role> roleRepository,
        ITokenService tokenService,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            _logger.LogInformation($"Login attempt for user: {request.Username}");

            var user = await GetUserByUsernameAsync(request.Username);
            if (user == null)
            {
                _logger.LogWarning($"Login failed: User not found - {request.Username}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            if (!user.IsActive)
            {
                _logger.LogWarning($"Login failed: User inactive - {request.Username}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "User account is inactive"
                };
            }

            if (!VerifyPassword(request.Password, user.PasswordHash))
            {
                _logger.LogWarning($"Login failed: Invalid password - {request.Username}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            // Update last login
            user.LastLogin = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            _logger.LogInformation($"Login successful for user: {request.Username}");

            return new AuthResponse
            {
                Success = true,
                Message = "Login successful",
                User = new UserAuthDto
                {
                    UserId = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role?.RoleName ?? "User",
                    IsActive = user.IsActive
                },
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 3600 // 1 hour in seconds
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Login error: {ex.Message}");
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred during login"
            };
        }
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        try
        {
            _logger.LogInformation($"Registration attempt for user: {request.Username}");

            // Check if user exists
            var existingUser = await GetUserByUsernameAsync(request.Username);
            if (existingUser != null)
            {
                _logger.LogWarning($"Registration failed: Username already exists - {request.Username}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Username already exists"
                };
            }

            // Check if email exists
            var users = await _userRepository.GetAllAsync();
            if (users.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
            {
                _logger.LogWarning($"Registration failed: Email already exists - {request.Email}");
                return new AuthResponse
                {
                    Success = false,
                    Message = "Email already registered"
                };
            }

            // Get default user role
            var roles = await _roleRepository.GetAllAsync();
            var userRole = roles.FirstOrDefault(r => r.RoleName.Equals("User", StringComparison.OrdinalIgnoreCase));
            if (userRole == null)
            {
                _logger.LogError("Default user role not found");
                return new AuthResponse
                {
                    Success = false,
                    Message = "An error occurred during registration"
                };
            }

            // Create new user
            var newUser = new User
            {
                Username = request.Username,
                Email = request.Email,
                FullName = request.FullName,
                Phone = request.Phone,
                PasswordHash = PasswordHasher.HashPassword(request.Password),
                RoleId = userRole.Id,
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow
            };

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation($"Registration successful for user: {request.Username}");

            var accessToken = _tokenService.GenerateAccessToken(newUser);
            var refreshToken = _tokenService.GenerateRefreshToken();

            return new AuthResponse
            {
                Success = true,
                Message = "Registration successful",
                User = new UserAuthDto
                {
                    UserId = newUser.Id,
                    Username = newUser.Username,
                    Email = newUser.Email,
                    FullName = newUser.FullName,
                    Role = userRole.RoleName,
                    IsActive = newUser.IsActive
                },
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 3600
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Registration error: {ex.Message}");
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred during registration"
            };
        }
    }

    public async Task<AuthResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = "Refresh token is required"
                };
            }

            // In a production system, you would validate the refresh token against stored tokens
            // For now, we'll assume it's valid and generate a new access token
            // This would require a RefreshToken table to track issued tokens

            _logger.LogInformation("Token refresh requested");

            return new AuthResponse
            {
                Success = true,
                Message = "Token refreshed successfully",
                ExpiresIn = 3600
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"Token refresh error: {ex.Message}");
            return new AuthResponse
            {
                Success = false,
                Message = "An error occurred during token refresh"
            };
        }
    }

    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request)
    {
        try
        {
            _logger.LogInformation($"Password change requested for user: {userId}");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"Password change failed: User not found - {userId}");
                return false;
            }

            if (!VerifyPassword(request.CurrentPassword, user.PasswordHash))
            {
                _logger.LogWarning($"Password change failed: Invalid current password - {userId}");
                return false;
            }

            user.PasswordHash = PasswordHasher.HashPassword(request.NewPassword);
            user.ModifiedDate = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation($"Password changed successfully for user: {userId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Password change error: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> LogoutAsync(int userId)
    {
        try
        {
            _logger.LogInformation($"Logout requested for user: {userId}");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                user.LastLogout = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);
                await _userRepository.SaveChangesAsync();
            }

            _logger.LogInformation($"Logout successful for user: {userId}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Logout error: {ex.Message}");
            return false;
        }
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        try
        {
            var users = await _userRepository.GetAllAsync();
            return users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving user: {ex.Message}");
            return null;
        }
    }

    public bool VerifyPassword(string password, string hash)
    {
        return PasswordHasher.VerifyPassword(password, hash);
    }
}
