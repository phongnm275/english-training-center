# Authentication Module Documentation

## Overview
The Authentication Module provides JWT-based authentication for the English Training Center Management System. It includes user login, registration, token refresh, and password management.

## Architecture

### Components

1. **ITokenService / JwtTokenService** (`Services/Auth/ITokenService.cs`)
   - JWT token generation and validation
   - Refresh token generation
   - Claims extraction

2. **IAuthService / AuthService** (`Services/Auth/AuthService.cs`)
   - User authentication logic
   - User registration
   - Password management
   - Session tracking

3. **AuthController** (`Controllers/AuthController.cs`)
   - REST API endpoints for authentication
   - Login, Register, RefreshToken, ChangePassword, Logout
   - User profile retrieval

4. **PasswordHasher** (`Common/Utilities/PasswordHasher.cs`)
   - PBKDF2-based password hashing
   - Password verification
   - Password complexity validation

## Configuration

### JWT Settings (appsettings.json)

```json
{
  "Jwt": {
    "Secret": "your-super-secret-key-that-is-at-least-32-characters-long",
    "Issuer": "EnglishTrainingCenter",
    "Audience": "EnglishTrainingCenterUsers",
    "ExpiryMinutes": 60
  }
}
```

**Important**: In production, use a strong secret key (minimum 32 characters for HS256).

### Environment Variables (Optional)

For security, you can override JWT settings with environment variables:

```powershell
$env:Jwt__Secret = "your-production-secret-key"
$env:Jwt__Issuer = "EnglishTrainingCenter"
$env:Jwt__Audience = "EnglishTrainingCenterUsers"
$env:Jwt__ExpiryMinutes = "60"
```

## API Endpoints

### 1. Login
```http
POST /api/v1/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin@123456"
}
```

**Response (Success - 200)**
```json
{
  "success": true,
  "message": "Login successful",
  "statusCode": 200,
  "data": {
    "success": true,
    "message": "Login successful",
    "user": {
      "userId": 1,
      "username": "admin",
      "email": "admin@trainingcenter.com",
      "fullName": "Administrator",
      "role": "Admin",
      "isActive": true
    },
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "base64-encoded-refresh-token",
    "expiresIn": 3600
  }
}
```

**Response (Failure - 400)**
```json
{
  "success": false,
  "message": "Invalid username or password",
  "statusCode": 400,
  "errors": null
}
```

### 2. Register
```http
POST /api/v1/auth/register
Content-Type: application/json

{
  "username": "newuser",
  "email": "newuser@example.com",
  "password": "SecurePass@123",
  "confirmPassword": "SecurePass@123",
  "fullName": "New User",
  "phone": "0912345678"
}
```

**Response (Success - 201)**
```json
{
  "success": true,
  "message": "Registration successful",
  "statusCode": 201,
  "data": {
    "success": true,
    "message": "Registration successful",
    "user": {
      "userId": 12,
      "username": "newuser",
      "email": "newuser@example.com",
      "fullName": "New User",
      "role": "User",
      "isActive": true
    },
    "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refreshToken": "base64-encoded-refresh-token",
    "expiresIn": 3600
  }
}
```

### 3. Refresh Token
```http
POST /api/v1/auth/refresh
Content-Type: application/json

{
  "refreshToken": "base64-encoded-refresh-token"
}
```

**Response (Success - 200)**
```json
{
  "success": true,
  "message": "Token refreshed successfully",
  "statusCode": 200,
  "data": {
    "success": true,
    "message": "Token refreshed successfully",
    "accessToken": "new-jwt-token",
    "expiresIn": 3600
  }
}
```

### 4. Change Password
```http
POST /api/v1/auth/change-password
Authorization: Bearer {accessToken}
Content-Type: application/json

{
  "currentPassword": "OldPass@123",
  "newPassword": "NewPass@456",
  "confirmNewPassword": "NewPass@456"
}
```

**Response (Success - 200)**
```json
{
  "success": true,
  "message": "Success",
  "statusCode": 200,
  "data": {
    "message": "Password changed successfully"
  }
}
```

### 5. Logout
```http
POST /api/v1/auth/logout
Authorization: Bearer {accessToken}
```

**Response (Success - 200)**
```json
{
  "success": true,
  "message": "Success",
  "statusCode": 200,
  "data": {
    "message": "Logout successful"
  }
}
```

### 6. Get Current User Profile
```http
GET /api/v1/auth/me
Authorization: Bearer {accessToken}
```

**Response (Success - 200)**
```json
{
  "success": true,
  "message": "Success",
  "statusCode": 200,
  "data": {
    "username": "admin",
    "role": "Admin",
    "timestamp": "2024-01-15T10:30:45.123Z"
  }
}
```

## Password Requirements

Passwords must meet the following complexity requirements:

- **Minimum Length**: 8 characters
- **Uppercase Letters**: At least 1 (A-Z)
- **Lowercase Letters**: At least 1 (a-z)
- **Numbers**: At least 1 (0-9)
- **Special Characters**: At least 1 (!@#$%^&*)

**Example Valid Password**: `SecurePass@123`

## Security Features

### 1. Password Hashing
- Uses PBKDF2 (Password-Based Key Derivation Function 2)
- Algorithm: SHA256
- Iterations: 10,000
- Salt Size: 16 bytes
- Hash Size: 20 bytes

### 2. JWT Token Security
- Algorithm: HS256 (HMAC with SHA256)
- Token Expiry: Configurable (default 60 minutes)
- Refresh Token: Base64-encoded random string
- Signature Verification: Issuer, Audience, Lifetime validation

### 3. Password Change Validation
- Requires current password verification
- New password must meet complexity requirements
- Password confirmation must match

### 4. Account Security
- User account status tracking (IsActive)
- Last login/logout timestamps
- Failed login prevention (inactive accounts)

## Implementation Guide

### 1. Enable JWT Authentication in Program.cs

The authentication is already configured in `Program.cs`:

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true
        };
    });
```

### 2. Use [Authorize] Attribute

To protect endpoints, use the `[Authorize]` attribute:

```csharp
[HttpPost("admin-only")]
[Authorize(Roles = "Admin")]
public IActionResult AdminOnlyEndpoint()
{
    return Ok("Admin access only");
}
```

### 3. Extract User Information

Use the `BaseController` helper methods:

```csharp
[Authorize]
public IActionResult GetUserInfo()
{
    var userId = GetUserIdFromToken();
    var username = GetUsernameFromToken();
    var role = GetRoleFromToken();
    
    return Ok(new { userId, username, role });
}
```

## Database Schema

### Users Table Modifications

```sql
ALTER TABLE [Users]
ADD [LastLogin] DATETIME2 NULL,
    [LastLogout] DATETIME2 NULL,
    [Phone] NVARCHAR(20) NULL;

CREATE INDEX [IX_Users_Username] ON [Users]([Username]);
CREATE INDEX [IX_Users_Email] ON [Users]([Email]);
CREATE INDEX [IX_Users_IsActive] ON [Users]([IsActive]);
```

## Error Handling

The authentication module returns appropriate HTTP status codes:

- **200 OK**: Successful login/token refresh
- **201 Created**: Successful registration
- **400 Bad Request**: Invalid credentials, password requirements not met
- **401 Unauthorized**: Missing or invalid token
- **403 Forbidden**: Insufficient permissions for the resource
- **500 Internal Server Error**: Server-side error

## Testing with Postman

### 1. Login Request
```
POST http://localhost:5000/api/v1/auth/login
Headers:
  Content-Type: application/json
Body:
{
  "username": "admin",
  "password": "Admin@123456"
}
```

### 2. Protected Endpoint with Token
```
GET http://localhost:5000/api/v1/auth/me
Headers:
  Authorization: Bearer {accessToken}
  Content-Type: application/json
```

### 3. Register New User
```
POST http://localhost:5000/api/v1/auth/register
Headers:
  Content-Type: application/json
Body:
{
  "username": "testuser",
  "email": "test@example.com",
  "password": "Test@12345",
  "confirmPassword": "Test@12345",
  "fullName": "Test User",
  "phone": "0901234567"
}
```

## Troubleshooting

### Issue: "JWT Secret is not configured"
**Solution**: Verify `appsettings.json` has a `Jwt` section with a `Secret` key.

### Issue: "Token expired"
**Solution**: Use the refresh endpoint to get a new access token.

### Issue: "Invalid credentials"
**Solutions**:
- Verify username and password are correct
- Check user account is active (IsActive = true)
- Ensure database has the user record

### Issue: "Password does not meet requirements"
**Solution**: Password must be 8+ characters with uppercase, lowercase, digit, and special character.

## Future Enhancements

1. Implement refresh token storage and validation in database
2. Add two-factor authentication (2FA)
3. Implement account lockout after failed login attempts
4. Add OAuth2/OIDC integration
5. Implement API key authentication for service-to-service calls
6. Add role-based authorization policies
7. Implement audit logging for authentication events

## Related Files

- Token Service: `Application/Services/Auth/ITokenService.cs`
- Auth Service: `Application/Services/Auth/AuthService.cs`
- Auth Controller: `API/Controllers/AuthController.cs`
- Password Hasher: `Common/Utilities/PasswordHasher.cs`
- Auth DTOs: `Application/DTOs/Auth/AuthDTOs.cs`
- Auth Validators: `Application/Validators/Auth/AuthValidators.cs`
- Program Configuration: `API/Program.cs`
- Settings: `API/appsettings.json`

## References

- [JWT.io](https://jwt.io)
- [OWASP Password Guidelines](https://cheatsheetseries.owasp.org/cheatsheets/Authentication_Cheat_Sheet.html)
- [Microsoft JWT Bearer Documentation](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.authentication.jwtbearer)
