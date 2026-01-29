using System.Security.Cryptography;
using System.Text;

namespace EnglishTrainingCenter.Common.Utilities;

/// <summary>
/// Utility class for password hashing and verification using bcrypt algorithm
/// </summary>
public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 20;
    private const int Iterations = 10000;

    /// <summary>
    /// Hash password using PBKDF2
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <returns>Hashed password with salt</returns>
    public static string HashPassword(string password)
    {
        using (var rng = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256))
        {
            var salt = rng.Salt;
            var hash = rng.GetBytes(HashSize);

            var hashWithSalt = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashWithSalt, 0, SaltSize);
            Array.Copy(hash, 0, hashWithSalt, SaltSize, HashSize);

            return Convert.ToBase64String(hashWithSalt);
        }
    }

    /// <summary>
    /// Verify password against hash
    /// </summary>
    /// <param name="password">Plain text password to verify</param>
    /// <param name="hash">Hash to verify against</param>
    /// <returns>True if password matches hash, false otherwise</returns>
    public static bool VerifyPassword(string password, string hash)
    {
        try
        {
            var hashBytes = Convert.FromBase64String(hash);
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                var hash2 = pbkdf2.GetBytes(HashSize);
                
                for (int i = 0; i < HashSize; i++)
                {
                    if (hashBytes[i + SaltSize] != hash2[i])
                    {
                        return false;
                    }
                }
                
                return true;
            }
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validate password complexity requirements
    /// </summary>
    /// <param name="password">Password to validate</param>
    /// <returns>Tuple of (IsValid, ErrorMessage)</returns>
    public static (bool IsValid, string ErrorMessage) ValidatePasswordComplexity(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 8)
            return (false, "Password must be at least 8 characters long");

        if (!password.Any(char.IsUpper))
            return (false, "Password must contain at least one uppercase letter");

        if (!password.Any(char.IsLower))
            return (false, "Password must contain at least one lowercase letter");

        if (!password.Any(char.IsDigit))
            return (false, "Password must contain at least one digit");

        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            return (false, "Password must contain at least one special character");

        return (true, string.Empty);
    }
}
