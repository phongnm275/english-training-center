## Quick Reference: Authentication Module Usage

### 1. Login
```bash
curl -X POST http://localhost:5000/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "Admin@123456"
  }'
```

**Response**:
```json
{
  "success": true,
  "data": {
    "user": {
      "userId": 1,
      "username": "admin",
      "email": "admin@trainingcenter.com"
    },
    "accessToken": "eyJhbGciOiJIUzI1NiIs...",
    "refreshToken": "...",
    "expiresIn": 3600
  }
}
```

### 2. Register New User
```bash
curl -X POST http://localhost:5000/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "newuser",
    "email": "newuser@example.com",
    "password": "SecurePass@123",
    "confirmPassword": "SecurePass@123",
    "fullName": "New User",
    "phone": "0912345678"
  }'
```

### 3. Use Token to Access Protected Endpoint
```bash
curl -X GET http://localhost:5000/api/v1/auth/me \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

### 4. Refresh Expired Token
```bash
curl -X POST http://localhost:5000/api/v1/auth/refresh \
  -H "Content-Type: application/json" \
  -d '{
    "refreshToken": "REFRESH_TOKEN_HERE"
  }'
```

### 5. Change Password
```bash
curl -X POST http://localhost:5000/api/v1/auth/change-password \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "currentPassword": "OldPass@123",
    "newPassword": "NewPass@456",
    "confirmNewPassword": "NewPass@456"
  }'
```

### 6. Logout
```bash
curl -X POST http://localhost:5000/api/v1/auth/logout \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

---

## Protecting Your Endpoints

### Add [Authorize] to Controller
```csharp
[ApiController]
[Route("api/v1/students")]
[Authorize]
public class StudentController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserIdFromToken();
        // Use userId to filter results...
        return Ok(students);
    }
}
```

### Role-Based Access
```csharp
[HttpDelete("{id}")]
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Delete(int id)
{
    // Only Admin can delete
    return Ok();
}
```

---

## Password Requirements

✅ **Valid Password**: `SecurePass@123`
- ✓ At least 8 characters
- ✓ Contains uppercase (S)
- ✓ Contains lowercase (e, c)
- ✓ Contains digit (1, 2, 3)
- ✓ Contains special char (@)

❌ **Invalid Passwords**:
- `password` - No uppercase, digit, or special char
- `Pass123` - No special character
- `Pass@` - Only 5 characters
- `PASSWORD@123` - No lowercase letters

---

## Common Errors & Solutions

| Error | Cause | Solution |
|-------|-------|----------|
| 401 Unauthorized | Missing/invalid token | Ensure token is in Authorization header as `Bearer <token>` |
| 400 Bad Request | Invalid credentials | Check username and password are correct |
| 400 Bad Request | Password doesn't meet requirements | Use 8+ chars with uppercase, lowercase, digit, special char |
| Invalid username or password | User doesn't exist | Register new user first |
| User account is inactive | Account disabled | Contact administrator |

---

## Token Structure

**JWT Token contains claims**:
```
{
  "nameid": "1",           // User ID
  "unique_name": "admin",  // Username
  "email": "admin@...",    // Email
  "FullName": "Admin",     // Full name
  "role": "Admin",         // Role
  "IsActive": "True"       // Account status
}
```

---

## Database Schema

```sql
-- Required tables for authentication
CREATE TABLE [Users] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [Username] NVARCHAR(100) UNIQUE NOT NULL,
    [Email] NVARCHAR(256) UNIQUE NOT NULL,
    [PasswordHash] NVARCHAR(MAX) NOT NULL,
    [FullName] NVARCHAR(256) NOT NULL,
    [Phone] NVARCHAR(20),
    [RoleId] INT NOT NULL,
    [IsActive] BIT NOT NULL,
    [LastLogin] DATETIME2,
    [LastLogout] DATETIME2,
    [CreatedDate] DATETIME2 NOT NULL,
    [ModifiedDate] DATETIME2 NOT NULL,
    FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id])
);

CREATE TABLE [Roles] (
    [Id] INT PRIMARY KEY IDENTITY(1,1),
    [RoleName] NVARCHAR(100) UNIQUE NOT NULL,
    [Description] NVARCHAR(500),
    [CreatedDate] DATETIME2 NOT NULL,
    [ModifiedDate] DATETIME2 NOT NULL
);
```

---

## Environment Setup

### Windows PowerShell
```powershell
# Set JWT secret
$env:Jwt__Secret = "your-secure-secret-key-minimum-32-characters-long"

# Run API
dotnet run --project "src/EnglishTrainingCenter.API"
```

### Command Line
```bash
set Jwt__Secret=your-secure-secret-key-minimum-32-characters-long
dotnet run --project "src/EnglishTrainingCenter.API"
```

---

## Development Tips

1. **Use Swagger UI** to test endpoints:
   - Navigate to `http://localhost:5000/swagger`
   - Click "Authorize" button
   - Paste your access token
   - Try endpoints directly

2. **Store token in Postman**:
   - Create environment variable: `{{accessToken}}`
   - Set Authorization header: `Bearer {{accessToken}}`
   - Use in multiple requests

3. **Debug token claims**:
   - Visit [jwt.io](https://jwt.io)
   - Paste your token
   - View all claims

4. **Monitor logs**:
   - Check `logs/log-*.txt` files
   - View Serilog output in console

---

## Security Checklist

- [ ] Changed JWT secret in production
- [ ] Enabled HTTPS only
- [ ] Set appropriate token expiry
- [ ] Implemented rate limiting
- [ ] Enabled audit logging
- [ ] Configured CORS properly
- [ ] Used secure password requirements
- [ ] Tested with invalid tokens
- [ ] Verified role-based access
- [ ] Checked for SQL injection vulnerabilities

---

## Files Reference

| File | Purpose |
|------|---------|
| `Services/Auth/ITokenService.cs` | JWT token generation |
| `Services/Auth/AuthService.cs` | Authentication logic |
| `Controllers/AuthController.cs` | API endpoints |
| `Common/Utilities/PasswordHasher.cs` | Password hashing |
| `DTOs/Auth/AuthDTOs.cs` | Data transfer objects |
| `Validators/Auth/AuthValidators.cs` | Request validation |
| `appsettings.json` | Configuration |
| `Program.cs` | Dependency injection setup |

---

## Contact & Support

For issues or questions about authentication:
1. Check [AUTHENTICATION.md](./AUTHENTICATION.md) for detailed docs
2. Review [AUTH_IMPLEMENTATION_SUMMARY.md](./AUTH_IMPLEMENTATION_SUMMARY.md)
3. Check application logs in `logs/` directory
4. Test with Postman collection

---

**Version**: 1.0  
**Last Updated**: 2024  
**Status**: Production Ready ✓
