using Microsoft.Data.SqlClient;
using FLM_LobbyDisplay.Models;

namespace FLM_LobbyDisplay.Services;

/// <summary>
/// MSSQL-backed authentication service — replaces OracleAuthService.
/// Queries ACL tables directly using parameterized SQL (no stored procedures).
///
/// Actual schema:
///   ACL_User        — ID_ACL_USER, USER_ID (login name), USR_PASSWORD, USR_EMAIL, EMP_NAME, EMP_NO, COMPANY
///   ACL_USR_ROLE    — ID_ACL_USR_ROLE, ID_ACL_ROLE, ID_ACL_USER  (junction: user ↔ role)
///   ACL_ROLE        — ID_ACL_ROLE, ROLE_NAME, ROLE_DESC, ID_ACL_RESOURCE  (role → system/resource)
///   ACL_RESOURCE    — ID_ACL_RESOURCE, RESOURCE_NAME (system name), RESOURCE_DESC, RESOURCE_VIEW, RESOURCE_CONTROLLER, RESOURCE_PARENT_ID, RESOURCE_SEQ
/// </summary>
public class MssqlAuthService : IMssqlAuthService
{
    private readonly string _connectionString;
    private readonly ILogger<MssqlAuthService> _logger;

    public MssqlAuthService(IConfiguration config, ILogger<MssqlAuthService> logger)
    {
        _logger = logger;
        _connectionString = config.GetConnectionString("MSSQL_ACL") ?? string.Empty;

        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            throw new InvalidOperationException(
                "Connection string 'MSSQL_ACL' is missing or empty in configuration. " +
                "Add a valid MSSQL connection string under ConnectionStrings:MSSQL_ACL in appsettings.json.");
        }
    }

    /// <inheritdoc />
    public AuthenticatorModel ValidateUser(string loginId, string systemName)
    {
        var model = new AuthenticatorModel { VALID_USER = false };

        try
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();

            // Join user → user-role → role → resource to validate user belongs to this system
            const string sql = @"
                SELECT u.ID_ACL_USER, u.USER_ID, u.USR_PASSWORD, u.USR_EMAIL,
                       u.COMPANY, u.EMP_NO, u.EMP_NAME,
                       r.ID_ACL_ROLE, r.ROLE_NAME, r.ROLE_DESC,
                       res.ID_ACL_RESOURCE, res.RESOURCE_NAME, res.RESOURCE_DESC
                FROM ACL_User u
                INNER JOIN ACL_USR_ROLE ur ON u.ID_ACL_USER = ur.ID_ACL_USER
                INNER JOIN ACL_ROLE r ON ur.ID_ACL_ROLE = r.ID_ACL_ROLE
                INNER JOIN ACL_RESOURCE res ON r.ID_ACL_RESOURCE = res.ID_ACL_RESOURCE
                WHERE u.USER_ID = @LoginId
                  AND res.RESOURCE_NAME = @SystemName";

            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@LoginId", loginId);
            cmd.Parameters.AddWithValue("@SystemName", systemName);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                model.ID_ACL_USER     = Convert.ToInt32(reader["ID_ACL_USER"]);
                model.USER_ID         = reader["USER_ID"]?.ToString() ?? string.Empty;
                model.LOGIN_ID        = reader["USER_ID"]?.ToString() ?? string.Empty; // USER_ID is the login name
                model.PASSWORD        = reader["USR_PASSWORD"]?.ToString() ?? string.Empty;
                model.USR_EMAIL       = reader["USR_EMAIL"]?.ToString() ?? string.Empty;
                model.COMPANY         = reader["COMPANY"]?.ToString() ?? string.Empty;
                model.EMP_NO          = reader["EMP_NO"]?.ToString() ?? string.Empty;
                model.EMP_NAME        = reader["EMP_NAME"]?.ToString() ?? string.Empty;
                model.ID_ACL_ROLE     = Convert.ToInt32(reader["ID_ACL_ROLE"]);
                model.ROLE_NAME       = reader["ROLE_NAME"]?.ToString() ?? string.Empty;
                model.ROLE_DESC       = reader["ROLE_DESC"]?.ToString() ?? string.Empty;
                model.ID_ACL_RESOURCE = Convert.ToInt32(reader["ID_ACL_RESOURCE"]);
                model.RESOURCE_NAME   = reader["RESOURCE_NAME"]?.ToString() ?? string.Empty;
                model.RESOURCE_DESC   = reader["RESOURCE_DESC"]?.ToString() ?? string.Empty;
                model.VALID_USER      = true;
            }
            else
            {
                _logger.LogWarning("ValidateUser: no user found for LoginId={LoginId}, System={System}",
                    loginId, systemName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ValidateUser failed for LoginId={LoginId}, System={System}",
                loginId, systemName);
        }

        return model;
    }

    /// <inheritdoc />
    public AuthenticatorModel ValidateAdUser(string adUsername, string systemName)
    {
        var model = new AuthenticatorModel { VALID_USER = false };

        try
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();

            // AD login: match on EMP_NO (employee number used as AD identifier)
            const string sql = @"
                SELECT u.ID_ACL_USER, u.USER_ID, u.USR_EMAIL,
                       u.COMPANY, u.EMP_NO, u.EMP_NAME,
                       r.ID_ACL_ROLE, r.ROLE_NAME, r.ROLE_DESC,
                       res.ID_ACL_RESOURCE, res.RESOURCE_NAME, res.RESOURCE_DESC
                FROM ACL_User u
                INNER JOIN ACL_USR_ROLE ur ON u.ID_ACL_USER = ur.ID_ACL_USER
                INNER JOIN ACL_ROLE r ON ur.ID_ACL_ROLE = r.ID_ACL_ROLE
                INNER JOIN ACL_RESOURCE res ON r.ID_ACL_RESOURCE = res.ID_ACL_RESOURCE
                WHERE u.EMP_NO = @AdUsername
                  AND res.RESOURCE_NAME = @SystemName";

            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@AdUsername", adUsername);
            cmd.Parameters.AddWithValue("@SystemName", systemName);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                model.ID_ACL_USER     = Convert.ToInt32(reader["ID_ACL_USER"]);
                model.USER_ID         = reader["USER_ID"]?.ToString() ?? string.Empty;
                model.LOGIN_ID        = reader["USER_ID"]?.ToString() ?? string.Empty;
                model.USR_EMAIL       = reader["USR_EMAIL"]?.ToString() ?? string.Empty;
                model.COMPANY         = reader["COMPANY"]?.ToString() ?? string.Empty;
                model.EMP_NO          = reader["EMP_NO"]?.ToString() ?? string.Empty;
                model.EMP_NAME        = reader["EMP_NAME"]?.ToString() ?? string.Empty;
                model.ID_ACL_ROLE     = Convert.ToInt32(reader["ID_ACL_ROLE"]);
                model.ROLE_NAME       = reader["ROLE_NAME"]?.ToString() ?? string.Empty;
                model.ROLE_DESC       = reader["ROLE_DESC"]?.ToString() ?? string.Empty;
                model.ID_ACL_RESOURCE = Convert.ToInt32(reader["ID_ACL_RESOURCE"]);
                model.RESOURCE_NAME   = reader["RESOURCE_NAME"]?.ToString() ?? string.Empty;
                model.RESOURCE_DESC   = reader["RESOURCE_DESC"]?.ToString() ?? string.Empty;
                model.VALID_USER      = true;
            }
            else
            {
                _logger.LogWarning("ValidateAdUser: no AD user found for AdUsername={AdUsername}, System={System}",
                    adUsername, systemName);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ValidateAdUser failed for AdUsername={AdUsername}, System={System}",
                adUsername, systemName);
        }

        return model;
    }

    /// <inheritdoc />
    public List<MenuResource> GetMenuResources(int roleId, string systemName)
    {
        var list = new List<MenuResource>();

        try
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();

            // Get the full 2-level hierarchy:
            //   Level 1: direct children of the role's system resource (parent groups)
            //   Level 2: children of those parent groups (menu items)
            // Uses a CTE to get both levels in one query, ordered by hierarchy then sequence.
            const string sql = @"
                DECLARE @SystemResId INT;
                SELECT @SystemResId = r.ID_ACL_RESOURCE FROM ACL_ROLE r WHERE r.ID_ACL_ROLE = @RoleId;

                SELECT res.ID_ACL_RESOURCE, res.RESOURCE_NAME, res.RESOURCE_DESC,
                       res.RESOURCE_VIEW, res.RESOURCE_CONTROLLER,
                       res.RESOURCE_PARENT_ID, res.RESOURCE_SEQ
                FROM ACL_RESOURCE res
                WHERE (res.RESOURCE_PARENT_ID = @SystemResId
                   OR  res.RESOURCE_PARENT_ID IN (
                           SELECT c.ID_ACL_RESOURCE FROM ACL_RESOURCE c
                           WHERE c.RESOURCE_PARENT_ID = @SystemResId AND c.RESOURCE_STATUS = 'Active'
                       ))
                AND res.RESOURCE_STATUS = 'Active'
                ORDER BY res.RESOURCE_PARENT_ID, res.RESOURCE_SEQ";

            using var cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@RoleId", roleId);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var view = reader["RESOURCE_VIEW"]?.ToString() ?? string.Empty;
                var controller = reader["RESOURCE_CONTROLLER"]?.ToString() ?? string.Empty;

                // Build URL from controller/view if available
                var resourceUrl = string.Empty;
                if (!string.IsNullOrEmpty(controller) && controller != "-" &&
                    !string.IsNullOrEmpty(view) && view != "-")
                {
                    resourceUrl = $"~/{controller}/{view}";
                }

                list.Add(new MenuResource
                {
                    ResourceID   = Convert.ToInt32(reader["ID_ACL_RESOURCE"]),
                    ResourceName = reader["RESOURCE_NAME"]?.ToString() ?? string.Empty,
                    ResourceURL  = resourceUrl,
                    ResourceDesc = reader["RESOURCE_DESC"]?.ToString() ?? string.Empty,
                    ParentID     = Convert.ToInt32(reader["RESOURCE_PARENT_ID"]),
                    AppID        = 0,
                });
            }

            _logger.LogInformation("GetMenuResources returned {Count} items for RoleId={RoleId}",
                list.Count, roleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetMenuResources failed for RoleId={RoleId}, System={System}",
                roleId, systemName);
        }

        return list;
    }

    /// <inheritdoc />
    public bool ChangePassword(string loginId, string oldPassword, string newPassword)
    {
        try
        {
            using var con = new SqlConnection(_connectionString);
            con.Open();

            // Fetch current stored hash — USER_ID is the login name
            const string selectSql = "SELECT USR_PASSWORD FROM ACL_User WHERE USER_ID = @LoginId";
            using var selectCmd = new SqlCommand(selectSql, con);
            selectCmd.Parameters.AddWithValue("@LoginId", loginId);

            var storedHash = selectCmd.ExecuteScalar()?.ToString();
            if (string.IsNullOrEmpty(storedHash))
            {
                _logger.LogWarning("ChangePassword: no user found for LoginId={LoginId}", loginId);
                return false;
            }

            // Verify old password
            if (!Pbkdf2PasswordHasher.VerifyHashedPassword(storedHash, oldPassword))
            {
                _logger.LogWarning("ChangePassword: old password mismatch for LoginId={LoginId}", loginId);
                return false;
            }

            // Generate new hash and update
            var newHash = Pbkdf2PasswordHasher.HashPassword(newPassword);

            const string updateSql = "UPDATE ACL_User SET USR_PASSWORD = @NewHash WHERE USER_ID = @LoginId";
            using var updateCmd = new SqlCommand(updateSql, con);
            updateCmd.Parameters.AddWithValue("@NewHash", newHash);
            updateCmd.Parameters.AddWithValue("@LoginId", loginId);

            var rowsAffected = updateCmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                _logger.LogInformation("ChangePassword: password updated successfully for LoginId={LoginId}", loginId);
                return true;
            }

            _logger.LogWarning("ChangePassword: update affected 0 rows for LoginId={LoginId}", loginId);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ChangePassword failed for LoginId={LoginId}", loginId);
            return false;
        }
    }

    /// <summary>
    /// Extracts the username portion from a "DOMAIN\username" identity string.
    /// Returns the full string if no backslash is present.
    /// </summary>
    public static string ExtractAdUsername(string identityName)
    {
        if (string.IsNullOrEmpty(identityName))
            return string.Empty;

        var parts = identityName.Split('\\');
        return parts.Length > 1 ? parts[1] : parts[0];
    }
}
