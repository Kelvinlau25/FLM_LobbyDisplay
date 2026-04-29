using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FLM_LobbyDisplay.Services;

/// <summary>
/// Base class for log/audit trail Razor Pages.
/// Replaces Control.LogBase from App_Code.
/// </summary>
public abstract class LogPageBase : PageModel
{
    public abstract string LogTitle { get; }
    public abstract string LogPage  { get; }

    public string NormalTitle  => LogTitle;
    public string DisplayTitle => LogTitle + " Audit Trail";
}
