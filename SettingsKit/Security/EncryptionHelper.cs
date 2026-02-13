#region copyright

/*****************************************************************************************
*                                     ______________________________________________     *
*                              o O   |                                              |    *
*                     (((((  o      <                  SettingsKit                  |    *
*                    ( o o )         |______________________________________________|    *
* ------------oOOO-----(_)-----OOOo----------------------------------------------------- *
*             Project: SettingsKit                                                       *
*            Filename: EncryptionHelper.cs                                               *
*              Author: Stanley Omoregie                                                  *
*        Created Date: 07.02.2026                                                        *
*       Modified Date: 07.02.2026                                                        *
*          Created By: Stanley Omoregie                                                  *
*    Last Modified By: Stanley Omoregie                                                  *
*           CopyRight: copyright © 2026 Omotech Digital Solutions                        *
*                  .oooO  Oooo.                                                          *
*                  (   )  (   )                                                          *
* ------------------\ (----) /---------------------------------------------------------- *
*                    \_)  (_/                                                            *
*****************************************************************************************/

#endregion copyright

using System.Security.Cryptography;
using System.Text;

namespace SettingsKit.Security;

/// <summary>
/// Provides helper methods for encrypting and decrypting strings using Windows Data Protection API (DPAPI).
/// </summary>
/// <remarks>
/// This class uses <see cref="DataProtectionScope.CurrentUser"/> which means encrypted data can only be decrypted
/// by the same user account on the same machine.
/// </remarks>
public class EncryptionHelper
{
    public static string Encrypt(string plain)
    {
        var bytes = Encoding.UTF8.GetBytes(plain);
        var encrypted = ProtectedData.Protect(bytes, null, DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(encrypted);
    }

    private static string Decrypt(string cipher)
    {
        var bytes = Convert.FromBase64String(cipher);
        var decrypted = ProtectedData.Unprotect(bytes, null, DataProtectionScope.CurrentUser);
        return Encoding.UTF8.GetString(decrypted);
    }

    /// <summary>
    /// Attempts to decrypt a value, but returns it unchanged if it's not valid Base-64 (i.e., not encrypted).
    /// This handles cases where data wasn't properly encrypted or is corrupted.
    /// </summary>
    public static string TryDecrypt(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        // Check if the string is valid Base-64
        if (!IsValidBase64(value))
        {
            // Not encrypted, return as-is
            return value;
        }

        try
        {
            return Decrypt(value);
        }
        catch
        {
            // If decryption fails, return the original value
            // This handles corrupted or tampered data
            return value;
        }
    }

    /// <summary>
    /// Validates whether a string is valid Base-64.
    /// </summary>
    private static bool IsValidBase64(string value)
    {
        if (string.IsNullOrEmpty(value))
            return false;

        var trimmed = value.Trim();

        // Base-64 string length must be multiple of 4
        if (trimmed.Length % 4 != 0)
            return false;

        var maxDecodedLength = (trimmed.Length / 4) * 3;
        var buffer = maxDecodedLength == 0 ? Span<byte>.Empty : stackalloc byte[maxDecodedLength];
        return Convert.TryFromBase64String(trimmed, buffer, out _);
    }
}