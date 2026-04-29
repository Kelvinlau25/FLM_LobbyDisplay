Namespace Control
    ''' <summary>
    ''' Message center (net8.0 migration — ScriptManager and Web.UI.Page removed)
    ''' BuildAlertScript returns a script string for inline Razor injection.
    ''' </summary>
    Public Class MessageCenter
        Public Shared Function BuildAlertScript(ByVal ajax_msg As String) As String
            Dim encoded As String = System.Web.HttpUtility.HtmlEncode(ajax_msg.Replace("'", """"))
            Return String.Format("<script type=""text/javascript"">alert('{0}');</script>", encoded.Replace("||", "\n"))
        End Function
    End Class
End Namespace
