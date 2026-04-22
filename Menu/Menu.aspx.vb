Imports System.Collections.Generic
Imports ACL.MenuBar.Object

Partial Class Style2_Menu
    Inherits System.Web.UI.Page

    Protected _list As LeftMenuItemList
    Private _userid As Integer
    Private _systemid As Integer
    Protected _words As String
    Private _pointer As Boolean = False

    Private _SignOutURL As String = String.Empty
    Protected ReadOnly Property SignOutURL() As String
        Get
            Return _SignOutURL
        End Get
    End Property

    Private _HomeURL As String = String.Empty
    Protected ReadOnly Property HomeURL() As String
        Get
            Return _HomeURL
        End Get
    End Property

    Protected Sub Style2_Menu_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Request.QueryString("id") IsNot Nothing Then
            '  frContent.Attributes("src") = "../acc/Entry/MU_Manual_Cal_Dtl.aspx" & Request.Url.Query
            frContent.Attributes("src") = "../acc/Entry/" & Request.QueryString("page").ToString() & Request.Url.Query
        Else
            frContent.Attributes("src") = ""
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me._SignOutURL = ResolveUrl("~/SessionExpired.aspx")
        Me._HomeURL = ResolveUrl("~/Default.aspx")

        If Request.QueryString(ACL.Control.URL.URLEMPLOYEEID) IsNot Nothing Then
            Session("gstrUserID") = ACL.Security.Encryption.Decrypt(Request.QueryString(ACL.Control.URL.URLEMPLOYEEID))
            _pointer = True
        End If

        If Session("gstrUserID") Is Nothing Then
            Response.Redirect("~/SessionExpired.aspx?ReturnURL=" & Server.UrlEncode(Request.RawUrl))
        End If

        If Session("system") Is Nothing Or Session("system") = "0" Then
            systemCheck(Request.QueryString(ACL.Control.URL.URLSYSTEMNAME))
        End If

        ahrefhome.Visible = _pointer

        If _pointer Then
            Me._SignOutURL = ConfigurationManager.AppSettings("MISSignout")
            ahrefhome.HRef = ConfigurationManager.AppSettings("MISHome")
            Try
                Dim userobj As ACL.Object.User = ACL.OracleClass.User.UserInfo(ConfigurationManager.ConnectionStrings("ORCL_ACL").ConnectionString, Session("gstrUserID"))
                Session("gstrUserID") = userobj.UserID
                Session("gettemp") = userobj.EmployeeName
                Session("gstrUsername") = userobj.Username
                Session("gstrPassword") = userobj.Password
                Session("LoginHis") = DateTime.Now.ToString("dd MMMM yyyy")
                Session("gstrUserCompCode") = userobj.UserCom
                Session("com") = userobj.UserCom
            Catch ex As Exception
                Response.Redirect(ConfigurationManager.AppSettings("MISHome"))
            End Try
        End If

        _userid = Session("gstrUserID")
        _systemid = Session("system")

        If _list Is Nothing Then
            _list = New LeftMenuItemList
        End If

        Dim counter As Integer = 0
        Dim _acl As New ACL.OracleClass.Resource(ConfigurationManager.ConnectionStrings("ORCL_ACL").ConnectionString)
        Dim _sourcelist As List(Of ACL.Object.Resource) = _acl.RetrieveResource(_userid, _systemid)
        Dim _str As StringBuilder
        Dim _altcounter As Integer = 0

        For Each itm As ACL.Object.Resource In ACL.Search.GetParent(_sourcelist, _systemid)
            _list.AddItem(New LeftMenuItem("left_menu_" & counter, itm.ResouceDesc, False))
            _str = New StringBuilder
            _altcounter = 0

            For Each node As ACL.Object.Resource In ACL.Search.GetParent(_sourcelist, itm.ResourceID)
                _altcounter += 1
                _str.AppendFormat("<li class='{2}'><a {3} href='{0}'>{1}</a></li>", GenerateKeywords(node.ResourceURL, _userid, Session("gstrUserCom"), Session("gettemp"), node.ResourceName), node.ResouceDesc, If(_altcounter Mod 2 = 0, "alt", "nor"), "target='page'")
            Next

            liItems.Text &= String.Format("<div class='bar_itms' id='{0}'><ul>{1}</ul></div>", "left_menu_" & counter, _str.ToString)
            counter += 1
        Next

        If Now.Hour < 12 Then
            _words = "Good Morning"
        ElseIf Now.Hour >= 12 And Now.Hour <= 17 Then
            _words = "Good Afternoon"
        Else
            _words = "Good Evening"
        End If
    End Sub

    Public Function GenerateKeywords(ByVal URL As String, ByVal ID As String, ByVal Company As String, ByVal Name As String, ByVal System As String) As String
        ' URL = URL.Replace("http://10.200.1.12:205/", "http://127.0.0.1:3313/LSS/")
        GenerateKeywords = Server.HtmlEncode(ResolveUrl(URL))
    End Function

    Private Sub systemCheck(ByVal Systemname As String)
        'Validate the system
        Session("system") = 0
        Session("system") = ACL.OracleClass.Resource.RetrieveApplicationIDByName(ConfigurationManager.ConnectionStrings("ORCL_ACL").ConnectionString, Systemname)

        If Session("system") = 0 Then
            ClientScript.RegisterStartupScript(Me.GetType, "Alert", "alert('Invalid System');", True)
            Exit Sub
        End If
    End Sub
End Class
