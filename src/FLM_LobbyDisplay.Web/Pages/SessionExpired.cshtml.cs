using FLM_LobbyDisplay.Web.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FLM_LobbyDisplay.Web.Pages;

/// <summary>
/// Session-expired page. Replaces <c>SessionExpired.aspx</c> /
/// <c>SessionExpired.aspx.cs</c>.
///
/// Legacy behaviour:
///   * Always abandoned the session.
///   * If <c>?ID=1</c> was supplied, the "Click here to login again" link
///     was hidden (and no auto-redirect happened).
///   * Otherwise it injected a JS snippet that bounced
///     <c>window.parent.location</c> to <c>Default.aspx</c>.
/// </summary>
[AllowAnonymous]
public class SessionExpiredModel : BasePageModel
{
    public bool ShowLoginLink { get; private set; }

    public async Task<IActionResult> OnGetAsync(string? id)
    {
        // Always sign out and clear the legacy session keys.
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        HttpContext.Session.Clear();

        // Legacy: Request.QueryString["ID"] == "1"  =>  hide the link, no auto-redirect.
        ShowLoginLink = id != "1";
        return Page();
    }
}
