Imports System.Data
Imports System.Data.SqlClient

Partial Class acc_MstMain_Upper2ndScreen_Dtl
    Inherits Control.Base
    Private str_MSSQL_Connstr As String = ConfigurationManager.ConnectionStrings("filmDisplay").ConnectionString


    Public Overrides Sub BindData()

    End Sub

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
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, ex.Message)
        End Try
    End Function

    Public Function db_connSQLMaint(ByVal sql As String) As Boolean
        Try
            db_connSQLMaint = True

            Dim cnnstring As String = str_MSSQL_Connstr 
            Dim connection As New SqlConnection(cnnstring)
            connection.Open()
            Dim command As New SqlCommand(sql, connection)

            command.ExecuteNonQuery()
            connection.Close()
        Catch ex As Exception
            db_connSQLMaint = False
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, ex.Message)
        End Try
    End Function

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
	
        If Session("gstrUserID") Is Nothing Then
            Response.Redirect("~/SessionExpired.aspx")
        Else
            Dim dt As New Data.DataTable


            If MyBase.Action = EnumAction.Edit Then

                lblMainTitle.Text = "Master Maintenance Second Screen Upper Video - Edit"

                fileupload.Visible = False
                lblFileUpload.Visible = True

                dt = New Data.DataTable
                Dim Sql As String = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND ID_MM_VIDEOS=" & Request.QueryString("ID").ToString

                If Not db_connSQLSel(Sql, dt) Then
                    Exit Sub
                End If


                If dt.Rows.Count <> 0 Then
                    lblFileUpload.Text = dt.Rows(0).Item("ATTACH_FILE")
                    txt_skstart.Text = dt.Rows(0).Item("SEEK_START")
                    txt_skstop.Text = dt.Rows(0).Item("SEEK_END")
                    txt_tstart.Text = dt.Rows(0).Item("PERIOD_START").Substring(0, dt.Rows(0).Item("PERIOD_START").Length - 3)
                    txt_tstop.Text = dt.Rows(0).Item("PERIOD_END").Substring(0, dt.Rows(0).Item("PERIOD_END").Length - 3)
                    lblcreatedby.Text = dt.Rows(0).Item("CREATED_BY")
                    lblcreateddate.Text = dt.Rows(0).Item("CREATED_DATE")
                    lblcreatedloc.Text = dt.Rows(0).Item("CREATED_LOC")
                    lblupdatedby.Text = dt.Rows(0).Item("UPDATED_BY")
                    lblupdateddate.Text = dt.Rows(0).Item("UPDATED_DATE")
                    lblupdatedloc.Text = dt.Rows(0).Item("UPDATED_LOC")

                    lblerror.Visible = False
                Else
                    lblerror.Visible = True
                    lblerror.Attributes.Add("color", "red")
                End If

            ElseIf MyBase.Action = EnumAction.Add Then
                lblMainTitle.Text = "Master Maintenance Second Screen Upper Video - Add"
                fileupload.Visible = True
                lblFileUpload.Visible = False
                pnlAudit.Visible = False
            End If

        End If
    End Sub



    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsumit.Click
        Dim user As String = Session("gstrUserID").ToString
        Dim loc As String = System.Web.HttpContext.Current.Request.UserHostAddress.ToString

        Dim tpth = ""
        Try

            Dim name As String = String.Empty
            Dim _temp As String = "0"
            Dim seek_start As Integer = 0
            Dim seek_end As Integer = 0
            Dim start_time As String = "00:00:00"
            Dim end_time As String = "23:59:59"

            If MyBase.Action = EnumAction.Add Then
                If fileupload.HasFile Then

                    Dim ext As String = System.IO.Path.GetExtension(fileupload.FileName).ToLower
					
                    name = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + System.IO.Path.GetFileName(fileupload.FileName).ToLower

                    If ext.Equals(".mp4") Then
                        Dim fileSize As Integer = fileupload.PostedFile.ContentLength

                        If fileSize <= 4294967295 Then

                            Dim paths As String = Server.MapPath("~/acc/LobbyDisplay/secscrtop/" + name)

                            Dim saved As String = (ResolveUrl(paths))
                            tpth = saved
                            fileupload.SaveAs(saved)
                        Else
                            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, "Video File Uploaded Must Not Exceeded 400MB")
                            Exit Sub
                        End If

                    Else

                        Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, "Upload Only .mp4 Video")
                        Exit Sub

                    End If

                Else
                    Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, "Please Select Video")
                    Exit Sub
                End If
            End If

            If MyBase.Action = EnumAction.Add Then
                Dim dt As New Data.DataTable
                Dim Sql As String = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND UPPER(Attach_File)='" & name & "'"

                If Not db_connSQLSel(Sql, dt) Then
                    Exit Sub
                End If

                If dt.Rows.Count = 0 Then

                    seek_start = If(Not (txt_skstart.Text = String.Empty), txt_skstart.Text, "0")
                    seek_end = If(Not (txt_skstop.Text = String.Empty), txt_skstop.Text, "0")
                    start_time = If(Not (txt_tstart.Text = String.Empty), txt_tstart.Text & ":00", "00:00:00")
                    end_time = If(Not (txt_tstop.Text = String.Empty), txt_tstop.Text & ":59", "23:59:59")

                    Sql = "INSERT INTO MM_VIDEOS (ATTACH_FILE,SEEK_START,SEEK_END,PERIOD_START,PERIOD_END,SCR_ID,RECORD_TYP,CREATED_BY,CREATED_DATE,CREATED_LOC,UPDATED_BY,UPDATED_DATE,UPDATED_LOC) VALUES ('" _
                    & name & "','" & seek_start & "','" & seek_end & "','" & start_time & "','" & end_time & "','2','1','" & user & "',GETDATE() ,'" & loc & "','" & user & "',GETDATE() ,'" & loc & "')"

                    If Not db_connSQLMaint(Sql) Then
                        Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, "Unable to Insert-Error")
                        Exit Sub
                    End If

					ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert('Video Successfully Added.'); setTimeout(function(){window.location.href='MM_VerticalScreenFull.aspx'}, 500);", true)
                Else
                    ClientScript.RegisterStartupScript(Me.GetType, "Alert", "alert('- Duplicate data ');", True)
                End If

            ElseIf MyBase.Action = EnumAction.Edit Then

                Dim vid = Request.QueryString("ID").ToString
                Dim dt As New Data.DataTable
                Dim Sql As String = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND ID_MM_VIDEOS = '" & vid & "'"


                If Not db_connSQLSel(Sql, dt) Then
                    Exit Sub
                End If

                If dt.Rows.Count <> 0 Then

                    seek_start = If(Not (txt_skstart.Text = String.Empty), txt_skstart.Text, "0")
                    seek_end = If(Not (txt_skstop.Text = String.Empty), txt_skstop.Text, "0")
                    start_time = If(Not (txt_tstart.Text = String.Empty), txt_tstart.Text & ":00", "00:00:00")
                    end_time = If(Not (txt_tstop.Text = String.Empty), txt_tstop.Text & ":59", "23:59:59")

                    Sql = "UPDATE MM_VIDEOS SET SEEK_START = '" & seek_start & "',SEEK_END = '" & seek_end & "',PERIOD_START = '" & start_time & "',PERIOD_END = '" & end_time & "',RECORD_TYP = '3',UPDATED_BY = '" & user & "',UPDATED_DATE = GETDATE(),UPDATED_LOC = '" & loc & "' WHERE ID_MM_VIDEOS = '" & vid & "'"

                    If Not db_connSQLMaint(Sql) Then
                        Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, "Unable to Edit - Error")
                        Exit Sub
                    End If
					
					ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert('Edits Successfully Updated.'); setTimeout(function(){window.location.href='MM_VerticalScreenFull.aspx'}, 500);", true)
					
                Else
                    ClientScript.RegisterStartupScript(Me.GetType, "Alert", "alert('- Video Not Found ');", True)
                End If
            End If
        Catch ex As Exception

            If ex.Message.Contains("Access") Then
                Response.Write(ex.Message.ToString)
                Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, tpth & " Path Unable to Upload Error: Access Denied")
            Else
                Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, ex.Message)
            End If
        End Try

    End Sub

End Class
