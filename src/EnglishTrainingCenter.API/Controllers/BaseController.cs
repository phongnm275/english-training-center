using Microsoft.AspNetCore.Mvc;

namespace EnglishTrainingCenter.API.Controllers;

/// <summary>
/// Base controller with common functionality
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Get current user ID from claims
    /// </summary>
    protected int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : 0;
    }

    /// <summary>
    /// Get current user role
    /// </summary>
    protected string? GetCurrentUserRole()
    {
        return User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
    }
}
