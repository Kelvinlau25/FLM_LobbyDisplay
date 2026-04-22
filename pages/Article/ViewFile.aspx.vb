
Partial Class Pages_Article_PlayVideo
    Inherits Control.BaseForm

    Public _IsPreviousLink As Boolean = False
    Public _IsValid As Boolean = False
    Public _V As String = ""



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("p") IsNot Nothing Then
            _IsPreviousLink = Request.QueryString("p").Equals("1")
        End If

        If Request.QueryString("v") IsNot Nothing Then
            _V = HttpUtility.UrlDecode(Request.QueryString("v").ToString.Trim).Replace(ConfigurationManager.AppSettings.Get("FILESERVER_KEY").ToString, ConfigurationManager.AppSettings.Get("FILESERVER_URL").ToString)
            Dim ext As String = _V.Substring(_V.LastIndexOf("."))
            If Not ext.ToUpper.Equals(".PDF") Then
                _V = ""
            End If

            If _V.Length = 0 Then
                litJava.Text = "alert('Invalid File Format, accept PDF only.'); window.close();"
            Else
                _IsValid = True
            End If
        Else
            litJava.Text = "alert('File Not Found.'); window.close();"
        End If

    End Sub


End Class
