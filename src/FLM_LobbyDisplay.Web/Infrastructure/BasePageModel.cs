using System.Security.Claims;
using Library.Root.Other;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FLM_LobbyDisplay.Web.Infrastructure;

/// <summary>
/// Base class for every Razor Page in the application. Replaces the legacy
/// <c>Library.Control.Base.Page</c> (which inherited from
/// <c>System.Web.UI.Page</c>) and provides the same
/// "current user / company / max-rows / flash-message" surface area, but in a
/// form that fits ASP.NET Core idioms (claims, <c>ISession</c>, <c>TempData</c>).
///
/// Flash-message contract (replaces <c>MessageCenter</c>):
///   * <see cref="SetFlashMessage"/> sets <c>TempData["FlashMessage"]</c> and
///     <c>TempData["FlashMessageType"]</c>.
///   * The shared <c>_Layout.cshtml</c> renders the message via a JS
///     <c>alert()</c> if present, mirroring the legacy
///     <c>MessageCenter.Show()</c> behaviour.
///
/// Audit-stamp contract (replaces <c>HttpContext.Current.Session</c> use in
/// <c>Library.Root.Object.Base</c>): call <see cref="StampAudit"/> on a domain
/// object before persisting it; this fills in CreatedBy/UpdatedBy and the
/// Created/Updated location from the current request.
/// </summary>
public abstract class BasePageModel : PageModel
{
    /// <summary>The legacy <c>Session["gstrUserID"]</c>.</summary>
    public string CurrentUserId =>
        User?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? HttpContext?.Session?.GetString("gstrUserID")
        ?? string.Empty;

    /// <summary>The legacy <c>Session["gstrUsername"]</c>.</summary>
    public string CurrentUsername =>
        User?.FindFirstValue(ClaimTypes.Name)
        ?? HttpContext?.Session?.GetString("gstrUsername")
        ?? string.Empty;

    /// <summary>The legacy <c>Session["gettemp"]</c> (employee display name).</summary>
    public string CurrentEmployeeName =>
        User?.FindFirstValue("EmployeeName")
        ?? HttpContext?.Session?.GetString("gettemp")
        ?? string.Empty;

    /// <summary>The legacy <c>Session["gstrUserCompCode"]</c> / <c>Session["com"]</c>.</summary>
    public string CurrentCompanyCode =>
        User?.FindFirstValue("CompanyCode")
        ?? HttpContext?.Session?.GetString("gstrUserCompCode")
        ?? string.Empty;

    /// <summary>
    /// Pagination size, sourced from <c>AppSettings.MaxRowPerPage</c>. Mirrors
    /// the legacy <c>BusinessLogicBase.MaxQuantityPerPage</c>.
    /// </summary>
    public int MaxRowPerPage => BusinessLogicBase.MaxQuantityPerPage;

    /// <summary>
    /// True if the request is from a user who has logged in.
    /// </summary>
    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

    /// <summary>
    /// Coarse-grained ACL helper. Phase 2 will replace the body with a real
    /// lookup against the ACL store (claim-based), keyed by
    /// <c>permissionCode</c>. Today every authenticated user is granted every
    /// permission, matching the legacy "if you can sign in you can use the
    /// app" behaviour.
    /// </summary>
    public virtual bool HasPermission(string permissionCode) => IsAuthenticated;

    /// <summary>
    /// Replacement for <c>MessageCenter.Show()</c>. Surfaces a single message
    /// after a redirect (PRG pattern). The shared layout renders it as a JS
    /// alert. <paramref name="messageType"/> is one of "info", "success",
    /// "warning", "error".
    /// </summary>
    public void SetFlashMessage(string message, string messageType = "info")
    {
        TempData["FlashMessage"] = message;
        TempData["FlashMessageType"] = messageType;
    }

    /// <summary>
    /// Stamp the audit fields on a <see cref="Library.Root.Object.Base"/>
    /// derived domain object using values from the current request. Replaces
    /// the legacy <c>HttpContext.Current.Session</c> reads in
    /// <c>Library.Root.Object.Base</c>'s constructor.
    /// </summary>
    public void StampAudit(Library.Root.Object.Base entity, bool isNew)
    {
        if (entity is null) return;

        var loc = HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;
        var now = DateTime.Now;
        var user = CurrentUserId;

        if (isNew)
        {
            entity.CreatedBy = user;
            entity.CreatedDate = now;
            entity.CreatedLoc = loc;
        }

        entity.UpdatedBy = user;
        entity.UpdatedDate = now;
        entity.UpdatedLoc = loc;
    }
}
