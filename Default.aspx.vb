Imports System.Data

Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        If Page.IsValid Then
            Dim Dtl As String = ""
            If Not Request.Url.Query.ToString.Equals("") Then
                Dtl = Request.Url.Query
            End If
            If Dtl.Equals("") Then
                Response.Redirect("~/Menu/Menu.aspx")
            Else
                Response.Redirect("~/Menu/Menu.aspx" & Dtl)
            End If

        End If
    End Sub

    Protected Sub cusCustom_ServerValidate(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        Try
            systemCheck()

            Dim temp As New ACL.OracleClass.User(ConfigurationManager.ConnectionStrings("ORCL_ACL").ConnectionString)
            Dim userobj As New ACL.Object.User

            If ConfigurationManager.AppSettings("CrossCompany") = "1" Then
                userobj = temp.validateWithRetrieveUsernCompany(ddlcompany.SelectedValue, txtusername.Text.Trim, txtpassword.Text.Trim, Session("system"))
            Else
                userobj = temp.validateWithRetrieveUser(txtusername.Text.Trim, txtpassword.Text.Trim, Session("system"))
            End If

            If userobj.UserID > -1 Then
                Session("gstrUserID") = userobj.UserID
                Session("gettemp") = userobj.EmployeeName
                Session("gstrUsername") = txtusername.Text.Trim
                Session("gstrPassword") = txtpassword.Text.Trim
                Session("LoginHis") = DateTime.Now.ToString("dd MMMM yyyy")
                Session("gstrUserCompCode") = userobj.UserCom
                Session("com") = userobj.UserCom
                e.IsValid = True

            Else
                e.IsValid = False
                ClientScript.RegisterStartupScript(Me.GetType, "Alert", "alert('" & "" & "');", True)
            End If
        Catch ex As Exception
            'MsgBox(ex.Message)
            'Response.Write(ex.Message)
            e.IsValid = False
            ClientScript.RegisterStartupScript(Me.GetType, "Alert", "alert('- Invalid username and password ');", True)
        End Try
    End Sub

    Private Sub systemCheck()
        'Validate the system
        Session("system") = 0
        Session("system") = ACL.OracleClass.Resource.RetrieveApplicationIDByName(ConfigurationManager.ConnectionStrings("ORCL_ACL").ConnectionString, ConfigurationManager.AppSettings("SystemName"))
        'MsgBox(Session("system"))
        If Session("system") = 0 Then
            ClientScript.RegisterStartupScript(Me.GetType, "Alert", "alert('Invalid System');", True)
            Exit Sub
        End If
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Not IsPostBack Then
            ' ACL.Control.Binding.BindCompany(ConfigurationManager.ConnectionStrings("ORCL_ACL").ConnectionString, ddlcompany)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Session.Abandon()
            Session.Clear()
            trcompanyselection.Visible = ConfigurationManager.AppSettings("CrossCompany") = "1"
        End If

        txtusername.Focus()

        '' ''-- ByPass ACL
        ''Response.Redirect("~/mRawMaterial/Home.aspx")
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
        If Request.QueryString("userid") IsNot Nothing Then
            txtusername.Text = Request.QueryString("userid").ToString
        End If
    End Sub

End Class
