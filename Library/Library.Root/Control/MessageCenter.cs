using System.Net;

namespace Library.Root.Control
{
    /// <summary>
    /// Message center (net8.0 migration — ScriptManager and Web.UI.Page removed)
    /// BuildAlertScript returns a script string for inline Razor injection.
    /// </summary>
    public class MessageCenter
    {
        public static string BuildAlertScript(string ajax_msg)
        {
            string encoded = WebUtility.HtmlEncode(ajax_msg.Replace("'", "\""));
            return string.Format("<script type=\"text/javascript\">alert('{0}');</script>", encoded.Replace("||", "\\n"));
        }
    }
}
