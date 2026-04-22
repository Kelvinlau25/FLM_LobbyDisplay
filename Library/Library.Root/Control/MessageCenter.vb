Namespace Control
    Public Class MessageCenter
        Public Shared Sub ShowAJAXMessageBox(ByVal msg_page As Web.UI.Page, ByVal ajax_msg As String)
            ajax_msg = System.Web.HttpContext.Current.Server.HtmlEncode(ajax_msg.Replace("'", """"))
            Dim Msg As String = String.Format("<script type=""text/javascript"">alert('" & ajax_msg.Replace("||", "\n") & "');</script>")
            Web.UI.ScriptManager.RegisterStartupScript(msg_page, msg_page.GetType(), "Msg", Msg, False)
        End Sub

        Public Shared Sub ShowJqueryMessageBox(ByVal currentpage As Web.UI.Page, ByVal Str As String)
            Str = System.Web.HttpContext.Current.Server.HtmlEncode(Str.Replace("'", """"))
            Dim prompt As String = "<script>$(document).ready(function(){{$.prompt('{0}!');}});</script>"
            Dim message As String = String.Format(prompt, Str)
            currentpage.ClientScript.RegisterStartupScript(GetType(Web.UI.Page), "alert", message)
        End Sub
    End Class
End Namespace


