using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace FLM_LobbyDisplay.Services;

/// <summary>
/// Base class for View Components.
/// Replaces Control.BaseUC from App_Code.
/// </summary>
public abstract class ViewComponentBase : ViewComponent
{
    public enum LanguagePack { English = 0, Malay = 1 }

    public LanguagePack Language =>
        Thread.CurrentThread.CurrentCulture.Name.Equals("ms-MY")
            ? LanguagePack.Malay
            : LanguagePack.English;

    public string ImageLanguagePrefix =>
        Thread.CurrentThread.CurrentCulture.Name.Equals("ms-MY") ? "_m" : "";
}
