using FLM_LobbyDisplay.Models;

namespace FLM_LobbyDisplay.Services;

public interface IMssqlAuthService
{
    /// <summary>
    /// Validates credentials against MSSQL ACL. Returns populated AuthenticatorModel.
    /// </summary>
    AuthenticatorModel ValidateUser(string loginId, string systemName);

    /// <summary>
    /// Validates AD username against MSSQL ACL (no password check).
    /// </summary>
    AuthenticatorModel ValidateAdUser(string adUsername, string systemName);

    /// <summary>
    /// Returns menu resources for a given role ID and system name.
    /// </summary>
    List<MenuResource> GetMenuResources(int roleId, string systemName);

    /// <summary>
    /// Changes password: verifies old, stores new PBKDF2 hash.
    /// </summary>
    bool ChangePassword(string loginId, string oldPassword, string newPassword);
}
