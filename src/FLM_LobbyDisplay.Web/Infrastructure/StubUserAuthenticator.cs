namespace FLM_LobbyDisplay.Web.Infrastructure;

/// <summary>
/// PLACEHOLDER implementation of <see cref="IUserAuthenticator"/>.
///
/// The legacy login pipeline (<c>Default.aspx.cs</c>) called into the
/// <c>ACL.OracleClass.User</c> assembly that ships in <c>/Bin</c> and targets
/// .NET Framework 4.5. That assembly is NOT compatible with .NET 8 and must
/// be ported (or replaced with a direct ADO.NET query against the ACL Oracle
/// schema) before real authentication can be enabled.
///
/// To keep Phase 1 self-contained and runnable, this stub:
///   * always rejects empty credentials,
///   * always rejects non-empty credentials with a clear "ACL not wired up"
///     message,
///   * returns <c>0</c> from <see cref="ResolveSystemIdAsync"/>, which
///     mirrors the legacy "Invalid System" error path.
///
/// Replace with a real implementation in a Phase 2 follow-up PR.
/// </summary>
internal sealed class StubUserAuthenticator : IUserAuthenticator
{
    public Task<AuthenticationResult> ValidateAsync(string? company, string username, string password, int systemId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return Task.FromResult(AuthenticationResult.Failed("Username and password are required."));
        }

        return Task.FromResult(AuthenticationResult.Failed(
            "Authentication backend is not yet wired up. The legacy ACL.OracleClass.User component must be ported to .NET 8 (or replaced with a direct ADO.NET implementation against the ACL Oracle schema). See MIGRATION.md."));
    }

    public Task<int> ResolveSystemIdAsync(string systemName, CancellationToken cancellationToken = default)
    {
        // 0 mirrors the "Invalid System" sentinel used by the legacy login
        // page when the system cannot be resolved.
        return Task.FromResult(0);
    }
}
