Imports System.Data.SqlClient
Imports System.Data
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Configuration

Partial Class Acc_TVDisplay2_Mst
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Cbutton.Visible = True
        End If
    End Sub

    Private Sub navigate(ByVal ID As String)
        Dim script As String = ""
        If ID = "Disp1" Then
            script = "window.open('lobby2_mainDisplay.aspx', '_blank');"
        ElseIf ID = "Disp2" Then
            script = "window.open('lobby2_2ndDisplay.aspx', '_blank');"
        End If
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "alertscript", script, True)
    End Sub

#Region "ButtonClick"
    Protected Sub BtnDisp1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDisp1.Click
        navigate("Disp1")
    End Sub
    Protected Sub BtnDisp2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnDisp2.Click
        navigate("Disp2")
    End Sub
#End Region

End Class
