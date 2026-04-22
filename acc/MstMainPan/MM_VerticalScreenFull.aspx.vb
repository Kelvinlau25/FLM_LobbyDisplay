Imports System.Data
Imports System.Data.SqlClient
Imports System.IO


Partial Class acc_MstMain_MM_VerticalScreenFull
    Inherits System.Web.UI.Page

    Dim clear As Integer = 0
    Private str_MSSQL_Connstr As String = ConfigurationManager.ConnectionStrings("filmDisplay").ConnectionString

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
            MsgBox(ex.Message)
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
            MsgBox(ex.Message)
        End Try
    End Function

    Protected Sub ddlsearch_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlsearch.SelectedIndexChanged
        If ddlsearch.SelectedItem.Text.Equals("-") Then
            txtsearch.Enabled = False
            txtsearch.Text = ""
            ButtonAdd.Enabled = False
        Else
            txtsearch.Enabled = True
            ButtonAdd.Enabled = True
        End If
    End Sub

    Protected Sub ButtonAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonAdd.Click
        Dim str As String
        Dim dup As Integer = 0

        If ddlcondition.SelectedItem.Value.Equals("LIKE") Then
            str = ddloperate.SelectedItem.Value & " " & ddlsearch.SelectedItem.Value & " LIKE '%" & txtsearch.Text.ToUpper & "%'" & " "
            MsgBox(str)
        Else
            str = ddloperate.SelectedItem.Value.ToUpper & " " & ddlsearch.SelectedItem.Value & " " & ddlcondition.SelectedItem.Value.ToUpper & "'" & txtsearch.Text.ToUpper & "'" & " "
        End If

        If lstboxsearch.Items.Count >= 1 Then
            For i As Integer = 0 To lstboxsearch.Items.Count - 1
                If str.Equals(lstboxsearch.Items(i).Text) Then
                    dup = 1
                End If
            Next

            If dup = 0 Then
                lstboxsearch.Items.Add(str)

            End If

            ddloperate.Enabled = True
        Else
            ddloperate.Enabled = False
            lstboxsearch.Items.Add(str)
        End If
    End Sub

    Protected Sub ButtonClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonClear.Click
        Try
            If clear = 0 Then
                If lstboxsearch.Items.Count = 1 Then
                    ddloperate.Enabled = False
                End If

                For i As Integer = 0 To lstboxsearch.Items.Count - 1
                    If lstboxsearch.SelectedItem.Value.Equals(lstboxsearch.Items(i).Text) Then
                        lstboxsearch.Items.Remove(lstboxsearch.SelectedItem.Value)
                        clear = 1
                        Exit Sub
                    End If
                Next

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnkbtnbasic_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnbasic.Click
        ButtonAdd.Visible = False
        ButtonClear.Visible = False
        lstboxsearch.Visible = False
        ddlcondition.Visible = False
        ddloperate.Visible = False
        lnkbtnadv.Visible = True
        lnkbtnbasic.Visible = False
    End Sub

    Protected Sub Advance_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnadv.Click
        ButtonAdd.Visible = True
        If ddlsearch.SelectedItem.Text.Equals("-") Then
            ButtonAdd.Enabled = False
        End If
        ButtonClear.Visible = True
        lstboxsearch.Visible = True
        ddlcondition.Visible = True
        ddloperate.Visible = True
        lnkbtnadv.Visible = False
        lnkbtnbasic.Visible = True
    End Sub

    Protected Sub ddlgridView_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlgrdpage.SelectedIndexChanged
        Gridview1.PageIndex = ddlgrdpage.SelectedItem.Value
        Gridview1.DataBind()
    End Sub

    Protected Sub ddlgridView2_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlgrdpage2.SelectedIndexChanged
        Gridview2.PageIndex = ddlgrdpage2.SelectedItem.Value
        Gridview2.DataBind()
    End Sub

    Protected Sub ddlgridView3_OnSelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlgrdpage3.SelectedIndexChanged
        Gridview3.PageIndex = ddlgrdpage3.SelectedItem.Value
        Gridview3.DataBind()
    End Sub

    Protected Sub gridView_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Gridview1.PageIndex = e.NewPageIndex
        Gridview1.DataBind()
        ddlgrdpage.SelectedIndex = e.NewPageIndex
    End Sub

    Protected Sub gridView2_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Gridview2.PageIndex = e.NewPageIndex
        Gridview2.DataBind()
        ddlgrdpage2.SelectedIndex = e.NewPageIndex
    End Sub

    Protected Sub gridView3_PageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        Gridview3.PageIndex = e.NewPageIndex
        Gridview3.DataBind()
        ddlgrdpage3.SelectedIndex = e.NewPageIndex
    End Sub

    Protected Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim item As String = e.Row.Cells(0).Text

            For Each image As System.Web.UI.WebControls.Image In e.Row.Cells(0).Controls.OfType(Of System.Web.UI.WebControls.Image)()
                image.Attributes("onclick") = "if(!confirm('Do you want to delete " + item + "?')){ return false; };"
            Next
        End If
    End Sub

    Protected Sub OnRowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim index As Integer = Convert.ToInt32(e.RowIndex)
        Dim dt As New Data.DataTable
        Dim id As TableCell = DirectCast(Gridview1.Rows(index).Cells(1), TableCell)
        Dim scr_id As TableCell = DirectCast(Gridview1.Rows(index).Cells(2), TableCell)
        Dim desc As HyperLink = DirectCast(Gridview1.Rows(index).Cells(3).FindControl("linkcode"), HyperLink)
        Dim user As String = Session("gstrUserID").ToString
        Dim loc As String = System.Web.HttpContext.Current.Request.UserHostAddress.ToString
        Dim Sql As String = "SELECT * FROM MM_VIDEOS where RECORD_TYP<>5 AND SCR_ID =" & CInt(scr_id.Text)

        If Not db_connSQLSel(Sql, dt) Then
            Exit Sub
        End If

        If dt.Rows.Count < 2 Then
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, "Unable to Delete - Video are being used in Lobby Screen")
            Exit Sub
        Else
            Sql = "UPDATE MM_VIDEOS SET UPDATED_by='" & user & "', UPDATED_Loc='" & loc & "', UPDATED_DATE=GETDATE(),RECORD_TYP='5' WHERE RECORD_TYP<>'5' AND ID_MM_VIDEOS =" & CInt(id.Text)

            If Not db_connSQLMaint(Sql) Then
                Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, "Unable to Delete - Error")
                Exit Sub
            End If

            Response.Redirect(Request.RawUrl)
        End If
    End Sub

    Protected Sub OnRowDeleting2(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim index As Integer = Convert.ToInt32(e.RowIndex)
        Dim dt2 As New Data.DataTable
        Dim id As TableCell = DirectCast(Gridview2.Rows(index).Cells(1), TableCell)
        Dim scr_id As TableCell = DirectCast(Gridview2.Rows(index).Cells(2), TableCell)
        Dim desc As HyperLink = DirectCast(Gridview2.Rows(index).Cells(3).FindControl("linkcode"), HyperLink)
        Dim user As String = Session("gstrUserID").ToString
        Dim loc As String = System.Web.HttpContext.Current.Request.UserHostAddress.ToString
        Dim Sql As String = "SELECT * FROM MM_VIDEOS where RECORD_TYP<>5 AND SCR_ID =" & CInt(scr_id.Text)

        If Not db_connSQLSel(Sql, dt2) Then
            Exit Sub
        End If

        If dt2.Rows.Count < 2 Then
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, "Unable to Delete - Video are being used in Lobby Screen")
            Exit Sub
        Else

            Sql = "UPDATE MM_VIDEOS SET UPDATED_by='" & user & "', UPDATED_Loc='" & loc & "', UPDATED_DATE=GETDATE(),RECORD_TYP='5' WHERE RECORD_TYP<>'5' AND ID_MM_VIDEOS =" & CInt(id.Text)

            If Not db_connSQLMaint(Sql) Then
                Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, "Unable to Delete - Error")
                Exit Sub
            End If

            Response.Redirect(Request.RawUrl)
        End If
    End Sub

    Protected Sub OnRowDeleting3(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Dim index As Integer = Convert.ToInt32(e.RowIndex)
        Dim dt3 As New Data.DataTable
        Dim id As TableCell = DirectCast(Gridview3.Rows(index).Cells(1), TableCell)
        Dim scr_id As TableCell = DirectCast(Gridview3.Rows(index).Cells(2), TableCell)
        Dim desc As HyperLink = DirectCast(Gridview3.Rows(index).Cells(3).FindControl("linkcode"), HyperLink)
        Dim user As String = Session("gstrUserID").ToString
        Dim loc As String = System.Web.HttpContext.Current.Request.UserHostAddress.ToString
        Dim Sql As String = "SELECT * FROM MM_VIDEOS where RECORD_TYP<>5 AND SCR_ID =" & CInt(scr_id.Text)

        If Not db_connSQLSel(Sql, dt3) Then
            Exit Sub
        End If

        If dt3.Rows.Count < 2 Then
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, "Unable to Delete - Video are being used in Lobby Screen")
            Exit Sub
        Else

            Sql = "UPDATE MM_VIDEOS SET UPDATED_by='" & user & "', UPDATED_Loc='" & loc & "', UPDATED_DATE=GETDATE(),RECORD_TYP='5' WHERE RECORD_TYP<>'5' AND ID_MM_VIDEOS =" & CInt(id.Text)

            If Not db_connSQLMaint(Sql) Then
                Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, "Unable to Delete - Error")
                Exit Sub
            End If

            Response.Redirect(Request.RawUrl)
        End If
    End Sub


    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        pnl1.Visible = False
        pnl2.Visible = False
        pnl3.Visible = False
        divchange.Visible = True
        divchange2.Visible = True
        divchange3.Visible = True

		Try
			Dim TextPath as String = Server.MapPath("scrollingtext.txt")
            Dim scrollingText As String
            scrollingText = My.Computer.FileSystem.ReadAllText(TextPath).ToString
            txtFooter.Text = scrollingText
        Catch ex As Exception
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, ex.Message)
        End Try

        If Session("gstrUserID") Is Nothing Then
            Response.Redirect("~/SessionExpired.aspx")
        Else

            If Not IsPostBack Then

                Dim dt As New Data.DataTable
                Dim Sql As String = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID=4 order by ID_MM_VIDEOS desc"

                Dim dt2 As New Data.DataTable
                Dim Sql2 As String = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID=5 order by ID_MM_VIDEOS desc"

                Dim dt3 As New Data.DataTable
                Dim Sql3 As String = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID=6 order by ID_MM_VIDEOS desc"

                If Not db_connSQLSel(Sql, dt) Or Not db_connSQLSel(Sql2, dt2) Or Not db_connSQLSel(Sql3, dt3) Then
                    Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, "Error")
                    Exit Sub
                End If

                If Not Session("mstmaintopic") Is Nothing Then
                    Session("mstmaintopic") = Nothing
                    Session("mstmaintopic") = dt
                Else
                    Session("mstmaintopic") = dt
                End If

                If Not Session("mstmaintopic2") Is Nothing Then
                    Session("mstmaintopic2") = Nothing
                    Session("mstmaintopic2") = dt2
                Else
                    Session("mstmaintopic2") = dt2
                End If

                If Not Session("mstmaintopic3") Is Nothing Then
                    Session("mstmaintopic3") = Nothing
                    Session("mstmaintopic3") = dt3
                Else
                    Session("mstmaintopic3") = dt3
                End If

                If Not dt.Rows.Count = 0 Then
                    Gridview1.DataSource = dt
                    Gridview1.DataBind()

                    pnldisplaypage.Visible = True

                    ddlgrdpage.Items.Clear()
                    For i As Integer = 0 To Gridview1.PageCount - 1
                        ddlgrdpage.Items.Add(New ListItem(i + 1, i))
                    Next
                    ddlgrdpage.DataBind()
                    AddHandler ddlgrdpage.SelectedIndexChanged, AddressOf ddlgridView_OnSelectedIndexChanged
                Else
                    Gridview1.DataSource = Nothing
                    Gridview1.DataBind()

                    pnldisplaypage.Visible = False
                    divchange.Visible = False
                End If

                If Not dt2.Rows.Count = 0 Then

                    Gridview2.DataSource = dt2
                    Gridview2.DataBind()

                    pnldisplaypage2.Visible = True

                    ddlgrdpage2.Items.Clear()
                    For i As Integer = 0 To Gridview2.PageCount - 1
                        ddlgrdpage2.Items.Add(New ListItem(i + 1, i))
                    Next
                    ddlgrdpage2.DataBind()
                    AddHandler ddlgrdpage2.SelectedIndexChanged, AddressOf ddlgridView2_OnSelectedIndexChanged
                Else

                    Gridview2.DataSource = Nothing
                    Gridview2.DataBind()

                    pnldisplaypage2.Visible = False
                    divchange2.Visible = False
                End If

                If Not dt3.Rows.Count = 0 Then

                    Gridview3.DataSource = dt3
                    Gridview3.DataBind()

                    pnldisplaypage3.Visible = True

                    ddlgrdpage3.Items.Clear()
                    For i As Integer = 0 To Gridview3.PageCount - 1
                        ddlgrdpage3.Items.Add(New ListItem(i + 1, i))
                    Next
                    ddlgrdpage3.DataBind()
                    AddHandler ddlgrdpage3.SelectedIndexChanged, AddressOf ddlgridView3_OnSelectedIndexChanged
                Else

                    Gridview3.DataSource = Nothing
                    Gridview3.DataBind()

                    pnldisplaypage3.Visible = False
                    divchange3.Visible = False
                End If
				
            Else

                If Not Session("mstmaintopic") Is Nothing Then
                    Dim dt As DataTable = TryCast(Session("mstmaintopic"), DataTable)
                    Gridview1.DataSource = dt
                    Gridview1.DataBind()
                    pnldisplaypage.Visible = True

                    ddlgrdpage.Items.Clear()
                    For i As Integer = 0 To Gridview1.PageCount - 1
                        ddlgrdpage.Items.Add(New ListItem(i + 1, i))
                    Next

                    ddlgrdpage.DataBind()
                    AddHandler ddlgrdpage.SelectedIndexChanged, AddressOf ddlgridView_OnSelectedIndexChanged
                End If

                If Not Session("mstmaintopic2") Is Nothing Then
                    Dim dt2 As DataTable = TryCast(Session("mstmaintopic2"), DataTable)
                    Gridview2.DataSource = dt2
                    Gridview2.DataBind()
                    pnldisplaypage2.Visible = True

                    ddlgrdpage.Items.Clear()
                    For i As Integer = 0 To Gridview1.PageCount - 1
                        ddlgrdpage.Items.Add(New ListItem(i + 1, i))
                    Next

                    ddlgrdpage.DataBind()
                    AddHandler ddlgrdpage.SelectedIndexChanged, AddressOf ddlgridView_OnSelectedIndexChanged
                End If

                If Not Session("mstmaintopic3") Is Nothing Then
                    Dim dt3 As DataTable = TryCast(Session("mstmaintopic3"), DataTable)
                    Gridview3.DataSource = dt3
                    Gridview3.DataBind()
                    pnldisplaypage3.Visible = True

                    ddlgrdpage.Items.Clear()
                    For i As Integer = 0 To Gridview1.PageCount - 1
                        ddlgrdpage.Items.Add(New ListItem(i + 1, i))
                    Next

                    ddlgrdpage.DataBind()
                    AddHandler ddlgrdpage.SelectedIndexChanged, AddressOf ddlgridView_OnSelectedIndexChanged
                End If

            End If
            AddHandler linkbtn.Click, AddressOf Redirect
            AddHandler linkbtn2.Click, AddressOf Redirect2
            AddHandler linkbtn3.Click, AddressOf Redirect3
        End If

    End Sub

    Protected Sub ButtonExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ButtonExecute.Click
        Try
            divchange.Visible = True

            Dim btnadd As Button = TryCast(pnl1.FindControl("ButtonAdd"), Button)
            Dim dt As New Data.DataTable
            Dim Sql As String = String.Empty

            If Not btnadd Is Nothing And btnadd.Visible = True Then
                Dim str As String = String.Empty

                If lstboxsearch.Items.Count <> 0 Then
                    For i As Integer = 0 To lstboxsearch.Items.Count - 1
                        str = str + lstboxsearch.Items(i).Text
                    Next
					
                    Sql = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND " & str
                Else
                    Sql = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5"
                End If
            Else
                If Not ddlsearch.SelectedItem.Value.Equals("-") Then
                    Sql = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND UPPER(" & ddlsearch.SelectedItem.Value & ") LIKE '%" & txtsearch.Text.ToUpper & "%' "
                Else
                    Sql = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5"
                End If
            End If

            If Not db_connSQLSel(Sql, dt) Then
                Exit Sub
            End If

            If Not Session("mstmaintopic") Is Nothing Then
                Session("mstmaintopic") = Nothing
                Session("mstmaintopic") = dt
            Else
                Session("mstmaintopic") = dt
            End If

            If Not dt.Rows.Count = 0 Then
                Gridview1.DataSource = dt
                Gridview1.DataBind()
                pnldisplaypage.Visible = True

                ddlgrdpage.Items.Clear()
                For i As Integer = 0 To Gridview1.PageCount - 1
                    ddlgrdpage.Items.Add(New ListItem(i + 1, i))
                Next
                ddlgrdpage.DataBind()
                AddHandler ddlgrdpage.SelectedIndexChanged, AddressOf ddlgridView_OnSelectedIndexChanged

            Else
                Gridview1.DataSource = Nothing
                Gridview1.DataBind()
                pnldisplaypage.Visible = False
                divchange.Visible = False
            End If

        Catch ex As Exception
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, ex.Message)
        End Try
    End Sub

    Protected Sub Redirect(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/acc/MstMainPan/FullMainScreen_Dtl.aspx?action=1")
    End Sub

    Protected Sub Redirect2(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/acc/MstMainPan/Upper2ndScreen_Dtl.aspx?action=1")
    End Sub

    Protected Sub Redirect3(ByVal sender As Object, ByVal e As System.EventArgs)
        Response.Redirect("~/acc/MstMainPan/Lower2ndScreen_Dtl.aspx?action=1")
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Session("display") = "Main"

        Response.Redirect("~/acc/entry/MainScreenVideo.aspx")
    End Sub




    Protected Sub btnFooter_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFooter.Click

        Try
			Dim TextPath as String = Server.MapPath("scrollingtext.txt")
			My.Computer.FileSystem.WriteAllText(TextPath, txtFooter.Text, False)
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, "Text Saved.")
        Catch ex As Exception
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, ex.Message)
        End Try

    End Sub
End Class
