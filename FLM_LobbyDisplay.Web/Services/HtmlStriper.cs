using System.Web;

namespace FLM_LobbyDisplay.Services;

/// <summary>HTML stripping and URL decode helpers. Replaces HtmlStriper from App_Code.</summary>
public class HtmlStriper
{
    private readonly IConfiguration _config;

    public HtmlStriper(IConfiguration config) => _config = config;

    public static string ToText(string source)
    {
        try
        {
            string result = source.Replace("\r", " ").Replace("\n", " ").Replace("\t", string.Empty);
            result = System.Text.RegularExpressions.Regex.Replace(result, "( )+", " ");
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*head([^>])*>", "<head>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<( )*(/)( )*head( )*>)", "</head>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<head>).*(</head>)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*script([^>])*>", "<script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<( )*(/)( )*script( )*>)", "</script>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<script>).*(</script>)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*style([^>])*>", "<style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<( )*(/)( )*style( )*>)", "</style>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(<style>).*(</style>)", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*td([^>])*>", "\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*br( )*>", "\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*li( )*>", "\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*div([^>])*>", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*tr([^>])*>", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "<( )*p([^>])*>", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "<[^>]*>", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "&nbsp;", " ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "&bull;", " * ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "&lsaquo;", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "&rsaquo;", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "&trade;", "(tm)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "&frasl;", "/", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "&lt;", "<", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "&gt;", ">", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "&copy;", "(c)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "&reg;", "(r)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "&(.{2,6});", string.Empty, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = result.Replace("\n", "\r");
            result = System.Text.RegularExpressions.Regex.Replace(result, "(\r)( )+(\r)", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(\t)( )+(\t)", "\t\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(\t)( )+(\r)", "\t\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(\r)( )+(\t)", "\r\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(\r)(\t)+(\r)", "\r\r", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            result = System.Text.RegularExpressions.Regex.Replace(result, "(\r)(\t)+", "\r\t", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string breaks = "\r\r\r";
            string tabs = "\t\t\t\t\t";
            for (int index = 0; index <= result.Length - 1; index++)
            {
                result = result.Replace(breaks, "\r\r");
                result = result.Replace(tabs, "\t\t\t\t");
                breaks += "\r";
                tabs += "\t";
            }
            return result.Trim();
        }
        catch { return source; }
    }

    public string FSMaskUrlDecode(string rawUrl)
    {
        var key = _config["AppSettings:FILESERVER_KEY"] ?? string.Empty;
        var url = _config["AppSettings:FILESERVER_URL"] ?? string.Empty;
        return HttpUtility.UrlDecode(rawUrl).Replace(key, url).Trim();
    }

    public string MMaskFLVPlayerDecode(string rawHtml)
    {
        var flvKey = _config["AppSettings:FLVPLAYER_KEY"] ?? string.Empty;
        var flvUrl = _config["AppSettings:FLVPLAYER_URL"] ?? string.Empty;
        var fsKey  = _config["AppSettings:FILESERVER_KEY"] ?? string.Empty;
        var fsUrl  = _config["AppSettings:FILESERVER_URL"] ?? string.Empty;
        return HttpUtility.HtmlDecode(rawHtml).Replace(flvKey, flvUrl).Replace(fsKey, fsUrl).Trim();
    }
}
