using System.Text.RegularExpressions;

namespace FLM_LobbyDisplay.Services;

/// <summary>
/// Replaces Flash/SWF plugin markup with HTML5 video elements.
/// Migrated from App_Code/PluginMediaFlash.cs — Flash is no longer supported.
/// </summary>
public static class Html5MediaConverter
{
    // Matches <object ...>...</object> blocks that contain .swf references
    private static readonly Regex FlashObjectRegex = new(
        @"<object[^>]*>.*?</object>",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    // Matches standalone <embed ... .swf ...> tags
    private static readonly Regex FlashEmbedRegex = new(
        @"<embed[^>]*\.swf[^>]*/?>",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    // Extracts a video/media URL from flashvars (url= or vdo= patterns)
    private static readonly Regex FlashVarsUrlRegex = new(
        @"(?:url|vdo)=([^\s&""']+)",
        RegexOptions.IgnoreCase | RegexOptions.Compiled);

    /// <summary>
    /// Converts any Flash plugin markup in the input HTML to an HTML5 video element.
    /// Returns the converted HTML. If no Flash markup is found, returns the input unchanged.
    /// </summary>
    public static string Convert(string html)
    {
        if (string.IsNullOrEmpty(html))
            return html;

        // Replace <object>...</object> blocks containing .swf
        html = FlashObjectRegex.Replace(html, match =>
        {
            var block = match.Value;
            if (!block.Contains(".swf", StringComparison.OrdinalIgnoreCase))
                return block; // not a Flash object, leave it

            var videoUrl = ExtractVideoUrl(block);
            return BuildHtml5Video(videoUrl);
        });

        // Replace any remaining standalone <embed .swf> tags
        html = FlashEmbedRegex.Replace(html, match =>
        {
            var videoUrl = ExtractVideoUrl(match.Value);
            return BuildHtml5Video(videoUrl);
        });

        return html;
    }

    private static string ExtractVideoUrl(string flashMarkup)
    {
        var m = FlashVarsUrlRegex.Match(flashMarkup);
        return m.Success ? m.Groups[1].Value : string.Empty;
    }

    private static string BuildHtml5Video(string src)
    {
        var srcAttr = string.IsNullOrEmpty(src) ? string.Empty : $" src=\"{System.Web.HttpUtility.HtmlAttributeEncode(src)}\"";
        return $"<video{srcAttr} controls style=\"width:100%;height:auto;\"><p>Your browser does not support HTML5 video.</p></video>";
    }
}
