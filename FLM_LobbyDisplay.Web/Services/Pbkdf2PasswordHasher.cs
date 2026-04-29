using System.Security.Cryptography;

namespace FLM_LobbyDisplay.Services;

/// <summary>
/// PBKDF2 password hasher compatible with the FILM_Sparepart_MVC reference implementation.
/// Byte layout: [0x00 header (1 byte)] [salt (16 bytes)] [derived key (32 bytes)] = 49 bytes total.
/// Uses HMAC-SHA1 with 1000 iterations to match existing hashes in the MSSQL ACL database.
/// </summary>
public static class Pbkdf2PasswordHasher
{
    private const int SaltSize = 16;       // 0x10
    private const int KeySize = 32;        // 0x20
    private const int Iterations = 1000;   // 0x3E8
    private const int HeaderSize = 1;
    private const int TotalSize = HeaderSize + SaltSize + KeySize; // 49

    /// <summary>
    /// Hashes a plaintext password using PBKDF2 (HMAC-SHA1, 1000 iterations).
    /// Returns a Base64-encoded string of the 49-byte hash (0x00 + salt + derived key).
    /// </summary>
    public static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] derivedKey = Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            Iterations,
            HashAlgorithmName.SHA1,
            KeySize);

        byte[] output = new byte[TotalSize];
        output[0] = 0x00; // header byte
        Buffer.BlockCopy(salt, 0, output, HeaderSize, SaltSize);
        Buffer.BlockCopy(derivedKey, 0, output, HeaderSize + SaltSize, KeySize);

        return Convert.ToBase64String(output);
    }

    /// <summary>
    /// Verifies a plaintext password against a stored PBKDF2 hash.
    /// Extracts the salt from the stored hash, recomputes the derived key,
    /// and uses fixed-time comparison to prevent timing attacks.
    /// </summary>
    public static bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        byte[] hashBytes;
        try
        {
            hashBytes = Convert.FromBase64String(hashedPassword);
        }
        catch (FormatException)
        {
            return false;
        }

        if (hashBytes.Length != TotalSize)
            return false;

        byte[] salt = new byte[SaltSize];
        Buffer.BlockCopy(hashBytes, HeaderSize, salt, 0, SaltSize);

        byte[] storedKey = new byte[KeySize];
        Buffer.BlockCopy(hashBytes, HeaderSize + SaltSize, storedKey, 0, KeySize);

        byte[] recomputedKey = Rfc2898DeriveBytes.Pbkdf2(
            providedPassword,
            salt,
            Iterations,
            HashAlgorithmName.SHA1,
            KeySize);

        return CryptographicOperations.FixedTimeEquals(storedKey, recomputedKey);
    }
}
