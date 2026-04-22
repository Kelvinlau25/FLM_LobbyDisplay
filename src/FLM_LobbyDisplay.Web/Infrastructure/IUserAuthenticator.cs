namespace FLM_LobbyDisplay.Web.Infrastructure;

/// <summary>
/// Result of a login attempt. Mirrors the fields the legacy
/// <c>Default.aspx.cs</c> stuffed into <c>Session</c> on success.
/// </summary>
public sealed class AuthenticationResult
{
    public bool Success { get; init; }
    public string? UserId { get; init; }
    public string? Username { get; init; }
    public string? EmployeeName { get; init; }
    public string? CompanyCode { get; init; }
    public string? FailureReason { get; init; }

    public static AuthenticationResult Failed(string reason) =>
        new() { Success = false, FailureReason = reason };
}

/// <summary>
/// Authenticates a user against the ACL store. The real implementation will
/// re-query the Oracle ACL schema (the legacy <c>ACL.OracleClass.User</c>
/// component) — that source code is not yet ported to .NET 8, so for Phase 1
/// a stub is wired in. See <see cref="StubUserAuthenticator"/>.
/// </summary>
public interface IUserAuthenticator
{
    /// <summary>
    /// Validate the user's credentials.
    /// </summary>
    /// <param name="company">Selected company (only used when CrossCompany="1").</param>
    /// <param name="username">User id.</param>
    /// <param name="password">Plain-text password as entered.</param>
    /// <param name="systemId">The application/system id resolved by name from ACL.</param>
    Task<AuthenticationResult> ValidateAsync(string? company, string username, string password, int systemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resolve the application/system id by display name from the ACL store.
    /// </summary>
    Task<int> ResolveSystemIdAsync(string systemName, CancellationToken cancellationToken = default);
}
