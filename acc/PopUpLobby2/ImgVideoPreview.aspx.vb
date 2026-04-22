Imports System.IO

Partial Class acc_PopUpLobby2_ImgVideoPreview
    Inherits System.Web.UI.Page

    Public Shared paths As String

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init



        If Len(Request.QueryString("id").ToString) > 5 Then

            Dim ext As String = Mid(Request.QueryString("id").ToString, Len(Request.QueryString("id").ToString) - 3)

            ext = Mid(ext.ToLower, 1, Len(ext.ToLower) - 1)

            Dim file As String = Mid(Request.QueryString("id").ToString, 2)

            file = Mid(file, 1, Len(file) - 1)

            If ext.Equals("mp4") Or ext.Equals("wmv") Then

                Dim i As String() = Request.Url.ToString.Split("/")
                paths = i(0) & "//" & i(2)

                If Request.QueryString("scr").ToString = 1 Then

                    If Request.IsLocal Then
                        paths = paths & "/LobbyDisplay/acc/LobbyDisplay2/mainscr/" & file
                    Else
                        paths = paths & "/acc/LobbyDisplay2/mainscr/" & file
                    End If

                ElseIf Request.QueryString("scr").ToString = 2 Then

                    If Request.IsLocal Then
                        paths = paths & "/LobbyDisplay/acc/LobbyDisplay2/secscrtop/" & file
                    Else
                        paths = paths & "/acc/LobbyDisplay2/secscrtop/" & file
                    End If

                Else

                    If Request.IsLocal Then
                        paths = paths & "/LobbyDisplay/acc/LobbyDisplay2/secscrbtm/" & file
                    Else
                        paths = paths & "/acc/LobbyDisplay2/secscrbtm/" & file
                    End If

                End If

            End If

        Else

            Exit Sub
        End If




    End Sub

End Class
