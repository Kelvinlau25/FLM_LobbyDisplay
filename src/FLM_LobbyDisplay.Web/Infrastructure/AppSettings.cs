namespace FLM_LobbyDisplay.Web.Infrastructure;

/// <summary>
/// Strongly-typed binding for the <c>AppSettings</c> section of
/// <c>appsettings.json</c>. Mirrors the values that lived in the legacy
/// <c>web.config</c> &lt;appSettings&gt; block.
/// </summary>
public class AppSettings
{
    public int MaxRowPerPage { get; set; } = 10;
    public string Title { get; set; } = "Film Signage";
    public string SystemName { get; set; } = "PFR Film Display Signage System";
    public string MISSignout { get; set; } = string.Empty;
    public string MISHome { get; set; } = string.Empty;

    /// <summary>
    /// "1" = company drop-down is shown on the login page (legacy
    /// CrossCompany behaviour). Anything else hides it.
    /// </summary>
    public string CrossCompany { get; set; } = "0";
}
