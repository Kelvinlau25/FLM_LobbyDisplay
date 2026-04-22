namespace Library.Root.Control
{
    public class MessageCenter
    {
        public static void ShowAJAXMessageBox(System.Web.UI.Page msg_page, string ajax_msg)
        {
            ajax_msg = System.Web.HttpContext.Current.Server.HtmlEncode(ajax_msg.Replace("'", "\""));
            string Msg = string.Format("<script type=\"text/javascript\">alert('" + ajax_msg.Replace("||", "\\n") + "');</script>");
            System.Web.UI.ScriptManager.RegisterStartupScript(msg_page, msg_page.GetType(), "Msg", Msg, false);
        }

        public static void ShowJqueryMessageBox(System.Web.UI.Page currentpage, string Str)
        {
            Str = System.Web.HttpContext.Current.Server.HtmlEncode(Str.Replace("'", "\""));
            string prompt = "<script>$(document).ready(function(){{$.prompt('{0}!');}});</script>";
            string message = string.Format(prompt, Str);
            currentpage.ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), "alert", message);
        }
    }
}
