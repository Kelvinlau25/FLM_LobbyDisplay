
Partial Class SessionExpired
    Inherits System.Web.UI.Page

    Private _default As String = String.Empty
    Public ReadOnly Property ReturnURL() As String
        Get
            Return "Default.aspx"
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Abandon()
        If Request.QueryString("ID") = "1" Then
            pnl.Visible = False
        Else
            ClientScript.RegisterStartupScript(Me.GetType(), "Load", "<script type='text/javascript'>window.parent.location.href='Default.aspx'; </script>")
        End If

    End Sub
End Class
