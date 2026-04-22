
Partial Class master_Main
    Inherits System.Web.UI.MasterPage

    Private _pointer As Boolean = False

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Session("gstrUserID") = "800017" 'user=62502, approver=800017
        'Session("gettemp") = "admin"
        'Session("gstrUsername") = "admin"
        'Session("gstrUserCompCode") = "System"
        'Session("gstrUserWorksNo") = "System"
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString(ACL.Control.URL.URLEMPLOYEEID) IsNot Nothing Then
            Session("gstrUserID") = ACL.Security.Encryption.Decrypt(Request.QueryString(ACL.Control.URL.URLEMPLOYEEID))
            _pointer = True
        End If

        If Request.QueryString(ACL.Control.URL.URLCOMPANYID) IsNot Nothing Then
            Session("gstrUserCompCode") = Request.QueryString(ACL.Control.URL.URLCOMPANYID)
            _pointer = True
        End If

        If Session("gstrUserID") Is Nothing Then
            If Request.UserAgent.ToLower().Contains("ipad") Then
                Response.Redirect("~/SessionExpired.aspx?ID=1")
            End If
            Response.Redirect("~/SessionExpired.aspx?ID=2")
        End If
    End Sub

End Class

