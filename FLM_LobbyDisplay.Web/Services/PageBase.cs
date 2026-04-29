using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace FLM_LobbyDisplay.Services;

/// <summary>
/// Base class for Razor Page PageModels.
/// Provides culture helpers, session helpers, and resource lookup.
/// Replaces Control.Base / Control.BaseForm from App_Code.
/// </summary>
public abstract class PageBase : PageModel
{
    protected IStringLocalizer? Localizer { get; private set; }

    /// <summary>Sets the localizer — call from derived page constructors.</summary>
    protected void SetLocalizer(IStringLocalizer localizer) => Localizer = localizer;

    public string DateTimeFormat => "dd / MMM / yyyy hh:mm:ss";

    public int UserID
    {
        get
        {
            var val = HttpContext.Session.GetString("gstrUserID");
            return int.TryParse(val, out var id) ? id : 0;
        }
    }

    public string AlertMessage { get; protected set; } = string.Empty;

    protected void ShowAlert(string message)
    {
        AlertMessage = message;
    }

    public enum LanguagePack { English = 0, Malay = 1 }

    public LanguagePack Language =>
        System.Threading.Thread.CurrentThread.CurrentCulture.Name.Equals("ms-MY")
            ? LanguagePack.Malay
            : LanguagePack.English;

    public string ImageLanguagePrefix =>
        System.Threading.Thread.CurrentThread.CurrentCulture.Name.Equals("ms-MY") ? "_m" : "";
}
