Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Timers

Partial Class acc_LobbyDisplay2_lobby_2ndDisplay
    Inherits System.Web.UI.Page

    Protected video_lists As String = String.Empty
    Protected video_lists2 As String = String.Empty
    Protected seek_starts As String = String.Empty
    Protected seek_ends As String = String.Empty
    Protected seek_starts2 As String = String.Empty
    Protected seek_ends2 As String = String.Empty
    Protected period_starts As String = String.Empty
    Protected period_ends As String = String.Empty
    Protected period_starts2 As String = String.Empty
    Protected period_ends2 As String = String.Empty
    Protected scrollingText As String = String.Empty

    Private str_MSSQL_Connstr As String = ConfigurationManager.ConnectionStrings("filmDisplay").ConnectionString
    Dim SQLConnection As OleDb.OleDbConnection

    Public Function db_connSQLSel(ByVal sql As String, ByVal dtDBTable As Data.DataTable) As Boolean
        Try
            db_connSQLSel = True
            Dim cnnstring As String = str_MSSQL_Connstr
            Using con As New SqlConnection(cnnstring)
                Using cmd As New SqlCommand(sql)
                    Using sda As New SqlDataAdapter()
                        cmd.Connection = con
                        sda.SelectCommand = cmd
                        Using dt As New DataTable()
                            sda.Fill(dtDBTable)
                        End Using
                    End Using
                End Using
            End Using

        Catch ex As Exception
            REM Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, ex.Message)
        End Try
    End Function


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim sql As String = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID=8"
        Dim dt As New DataTable
        If Not db_connSQLSel(sql, dt) Then
            Exit Sub
        End If

        Dim sql2 As String = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID=9"
        Dim dt2 As New DataTable
        If Not db_connSQLSel(sql2, dt2) Then
            Exit Sub
        End If

        Dim path1 As String = "\acc\LobbyDisplay2\secscrtop\"
        Dim path2 As String = "\acc\LobbyDisplay2\secscrbtm\"

        Dim videocount As Integer = 0

        If dt.Rows.Count <> 0 Then

            video_lists += "["
            seek_starts += "["
            seek_ends += "["
            period_starts += "["
            period_ends += "["

            For Each Row As DataRow In dt.Rows
                If videocount = 0 Then
                    video_lists += "'" & path1 & (Row("ATTACH_FILE")).ToString() & "'"
                    seek_starts += "'" & (Row("SEEK_START")).ToString() & "'"
                    seek_ends += "'" & (Row("SEEK_END")).ToString() & "'"
                    period_starts += "'" & (Row("PERIOD_START")).ToString() & "'"
                    period_ends += "'" & (Row("PERIOD_END")).ToString() & "'"
                Else
                    video_lists += ",'" & path1 & (Row("ATTACH_FILE")).ToString() & "'"
                    seek_starts += ",'" & (Row("SEEK_START")).ToString() & "'"
                    seek_ends += ",'" & (Row("SEEK_END")).ToString() & "'"
                    period_starts += ",'" & (Row("PERIOD_START")).ToString() & "'"
                    period_ends += ",'" & (Row("PERIOD_END")).ToString() & "'"
                End If

                videocount = +1

            Next

            video_lists += "]"
            seek_starts += "]"
            seek_ends += "]"
            period_starts += "]"
            period_ends += "]"

        End If

        video_lists = video_lists.Replace("\", "/")

        If dt2.Rows.Count <> 0 Then

            video_lists2 += "["
            seek_starts2 += "["
            seek_ends2 += "["
            period_starts2 += "["
            period_ends2 += "["

            For Each Row As DataRow In dt2.Rows
                video_lists2 += "'" & path2 & (Row("ATTACH_FILE")).ToString() & "',"
                seek_starts2 += "'" & (Row("SEEK_START")).ToString() & "',"
                seek_ends2 += "'" & (Row("SEEK_END")).ToString() & "',"
                period_starts2 += "'" & (Row("PERIOD_START")).ToString() & "',"
                period_ends2 += "'" & (Row("PERIOD_END")).ToString() & "',"
            Next

            video_lists2 += "]"
            seek_starts2 += "]"
            seek_ends2 += "]"
            period_starts2 += "]"
            period_ends2 += "]"

        End If

        video_lists2 = video_lists2.Replace("\", "/")

        Try
            Dim TextPath As String = Server.MapPath("..\MstMainLobby2\scrollingtext.txt")
            scrollingText = My.Computer.FileSystem.ReadAllText(TextPath).ToString
        Catch ex As Exception
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, ex.Message)
        End Try

        Session("Checkpoint") = "1"

        If IsPostBack Then

            Dim video_lists_tr = video_lists.Replace("[", "")
            video_lists_tr = video_lists_tr.Replace("]", "")
            video_lists_tr = video_lists_tr.Substring(0, video_lists_tr.Length - 1)

            Dim seek_starts_tr = seek_starts.Replace("[", "")
            seek_starts_tr = seek_starts_tr.Replace("]", "")
            seek_starts_tr = seek_starts_tr.Substring(0, seek_starts_tr.Length - 1)

            Dim seek_ends_tr = seek_ends.Replace("[", "")
            seek_ends_tr = seek_ends_tr.Replace("]", "")
            seek_ends_tr = seek_ends_tr.Substring(0, seek_ends_tr.Length - 1)

            Dim period_starts_tr = period_starts.Replace("[", "")
            period_starts_tr = period_starts_tr.Replace("]", "")
            period_starts_tr = period_starts_tr.Substring(0, period_starts_tr.Length - 1)

            Dim period_ends_tr = period_ends.Replace("[", "")
            period_ends_tr = period_ends_tr.Replace("]", "")
            period_ends_tr = period_ends_tr.Substring(0, period_ends_tr.Length - 1)

            Dim video_lists_tr2 = video_lists2.Replace("[", "")
            video_lists_tr2 = video_lists_tr2.Replace("]", "")
            video_lists_tr2 = video_lists_tr2.Substring(0, video_lists_tr2.Length - 1)

            Dim seek_starts_tr2 = seek_starts2.Replace("[", "")
            seek_starts_tr2 = seek_starts_tr2.Replace("]", "")
            seek_starts_tr2 = seek_starts_tr2.Substring(0, seek_starts_tr2.Length - 1)

            Dim seek_ends_tr2 = seek_ends2.Replace("[", "")
            seek_ends_tr2 = seek_ends_tr2.Replace("]", "")
            seek_ends_tr2 = seek_ends_tr2.Substring(0, seek_ends_tr2.Length - 1)

            Dim period_starts_tr2 = period_starts2.Replace("[", "")
            period_starts_tr2 = period_starts_tr2.Replace("]", "")
            period_starts_tr2 = period_starts_tr2.Substring(0, period_starts_tr2.Length - 1)

            Dim period_ends_tr2 = period_ends2.Replace("[", "")
            period_ends_tr2 = period_ends_tr2.Replace("]", "")
            period_ends_tr2 = period_ends_tr2.Substring(0, period_ends_tr2.Length - 1)

            ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "Javascript", "loaded(""" + video_lists_tr + """, """ + seek_starts_tr + """, """ + seek_ends_tr + """, """ + period_starts_tr + """, """ + period_ends_tr + """, """ + video_lists_tr2 + """, """ + seek_starts_tr2 + """, """ + seek_ends_tr2 + """, """ + period_starts_tr2 + """, """ + period_ends_tr2 + """);onPageLoad();", True)
        Else
            ScriptManager.RegisterClientScriptBlock(Me, [GetType](), "Javascript", "loaded2(); ", True)

        End If

    End Sub

End Class
