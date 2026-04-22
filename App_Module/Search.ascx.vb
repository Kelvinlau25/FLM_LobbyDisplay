''' <summary>
''' Search User Control 
''' Handler Basic Search and Advanced Search 
''' 
''' Remark : Based on previous Version and modified the way of the binding
''' ----------------------------------------
''' C.C.Yeon    25 APril 2011  Modified 
''' </summary>
''' <remarks></remarks>

Partial Class Search
    Inherits System.Web.UI.UserControl

    Private Search As Collections.Generic.List(Of String)
    Private query As Collections.Generic.List(Of String)
    Public Event AdditionalSearch()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.BindDDL()
        End If

        Dim setting As Control.Base = CType(Me.Page, Control.Base)

        If Session("key") Is Nothing Then
            Session("key") = setting.SetupKey
        Else
            If Session("key") <> setting.SetupKey Then
                Session("key") = setting.SetupKey
                Session("Type") = Nothing
                flush()
            End If
        End If

        If Session("Type") Is Nothing Then
            ChangeType("B")
        Else
            ChangeType(Session("Type"))
            Search = Session("Search")
            query = Session("Query")
        End If

        If Not IsPostBack Then
            If Session("Type") = "B" Then
                If setting.SearchField.Trim <> String.Empty Then
                    txtSearch.Text = setting.SearchValue
                    ddlSearch.SelectedValue = setting.SearchField
                End If
            Else
                BindCreteria(Session("Search"))
            End If
        End If

        btnReset.Enabled = searchCriteria.Items.Count > 0
        btnMinus.Enabled = searchCriteria.Items.Count > 0
    End Sub

    Protected Sub BindDDL()
        'Retrieve the value of the properties from the page
        'Based on the key retrieve the value from the resource and bind its into search selection
        Dim setting As Control.Base = CType(Me.Page, Control.Base)

        'here to disappear the delete check box
        Me.chkDeleted.Visible = setting.DeleteControl

        If setting.DeleteControl Then
            Me.chkDeleted.Visible = setting.ShowDeletedControl
            Me.chkDeleted.Checked = setting.ShowDeleted
        End If

        If setting.SetupKey = String.Empty Then
            Me.Visible = False
        Else
            Control.Binding.BindDropDownListResource(Me.ddlSearch, setting.SetupKey, "-", "-")
            Control.Binding.BindDropDownListResource(Me.ddlSearchUsing, setting.SetupKey, "-", "-")
        End If
    End Sub

    Private Sub ChangeType(ByVal Type As String)
        Session("Type") = Type

        If Type = "A" Then
            lblSearch.Text = "Advance Search"
            pnlBasic.Visible = False
            pnlAdvance.Visible = True
        ElseIf Type = "B" Then
            lblSearch.Text = "Basic Search"
            pnlBasic.Visible = True
            pnlAdvance.Visible = False
        End If
    End Sub

    Private Sub detector()
        If Search.Count = 0 Then
            ddlSearch2.Enabled = False
        Else
            ddlSearch2.Enabled = True
        End If
    End Sub

    Private Sub resetController()
        ddlSearch.SelectedIndex = 0
        ddlSearchUsing.SelectedIndex = 0
        ddlOperator1.SelectedIndex = 0
        ddlSearch2.SelectedIndex = 0
        txtSearch.Text = String.Empty
        txtSearchUsing.Text = String.Empty
    End Sub

    Private Sub flush()
        Search = New Collections.Generic.List(Of String)
        query = New Collections.Generic.List(Of String)
        resetController()
        searchCriteria.Items.Clear()
        Session("Search") = Search
        Session("Query") = query
    End Sub

    ''' <summary>
    ''' Retrieve the delete value from the checkedbox
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Deleted() As Boolean
        Get
            Return chkDeleted.Checked
        End Get
    End Property

    Public ReadOnly Property SearchType() As String
        Get
            Return Session("Type")
        End Get
    End Property

    Protected Sub lbAdvSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAdvSearch.Click
        flush()
        ChangeType("A")
    End Sub

    Protected Sub lbBasicSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbBasicSearch.Click
        flush()
        ChangeType("B")
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Dim setting As Control.Base = CType(Me.Page, Control.Base)

        If ddlSearch.SelectedIndex > 0 Then
            setting.SearchField = ddlSearch.SelectedValue
            setting.SearchValue = txtSearch.Text
        Else
            setting.SearchField = String.Empty
            setting.SearchValue = String.Empty
        End If

        RaiseEvent AdditionalSearch()

        setting.PageNo = 1
        Response.Redirect(setting.GenerateList)
    End Sub

    Protected Sub btnSubmit2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit2.Click
        Dim setting As Control.Base = CType(Me.Page, Control.Base)
        setting.SearchField = String.Empty
        setting.SearchValue = ConvertionString(query)

        RaiseEvent AdditionalSearch()

        setting.PageNo = 1
        Response.Redirect(setting.GenerateList)
    End Sub

    Protected Sub btnPlus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPlus.Click
        If CheckSelection(ddlSearchUsing.SelectedValue, "Search Field") = False Then
            Return
        End If

        If CheckSelection(ddlOperator1.SelectedValue, "Operator") = False Then
            Return
        End If

        If Search.Count > 0 Then
            If CheckSelection(ddlSearch2.SelectedValue, "Operator (And/Or)") = False Then
                Return
            End If
        End If

        If EmptyCheck(txtSearchUsing.Text, "Search Value") = False Then
            Return
        End If

        Search.Add(If(Search.Count > 0, ddlSearch2.SelectedItem.Text.Replace("-", "") & " ", String.Empty) & ddlSearchUsing.SelectedItem.Text & " " & ddlOperator1.SelectedItem.Text & " " & txtSearchUsing.Text)
        AddQuery(If(Search.Count > 0, ddlSearch2.SelectedItem.Text.Replace("-", "") & " ", "Add "), ddlSearchUsing.SelectedValue, ddlOperator1.SelectedValue, txtSearchUsing.Text)

        Session("Search") = Search
        Session("Query") = query

        BindCreteria(Search)
        detector()
        resetController()
        ddlSearchUsing.Focus()
    End Sub

    Private Sub AddQuery(ByVal Addtional As String, ByVal SearchField As String, ByVal [Operator] As String, ByVal SearchValue As String)
        If [Operator].Trim.ToUpper = "LIKE" Then
            SearchValue = "'%" & SearchValue & "%'"
        Else
            SearchValue = "'" & SearchValue & "'"
        End If

        query.Add(Addtional & " UPPER(" & SearchField & ") " & [Operator] & " UPPER(" & SearchValue & ")")
    End Sub

    Private Function CheckSelection(ByVal Value As String, ByVal field As String) As Boolean
        If Value = "-" Then
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, field & " was " & Resources.Message.InvalidSelect)
            Return False
        Else
            Return True
        End If
    End Function

    Private Function EmptyCheck(ByVal Value As String, ByVal Field As String) As Boolean
        If Value.Trim = String.Empty Then
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(Me.Page, String.Format(Resources.Message.FieldEmpty, Field))
            Return False
        Else
            Return True
        End If
    End Function

    Private Function ConvertionString(ByVal list As Collections.Generic.List(Of String)) As String
        Dim temp As New Text.StringBuilder
        For Each Str As String In list
            temp.AppendLine(Str)
        Next

        Return temp.ToString
    End Function

    Private Sub BindCreteria(ByVal list As Collections.Generic.List(Of String))
        btnReset.Enabled = list.Count > 0
        btnMinus.Enabled = list.Count > 0

        searchCriteria.DataSource = list
        searchCriteria.DataBind()
    End Sub

    Protected Sub btnMinus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMinus.Click
        If Search.Count = 0 Then
            Exit Sub
        End If

        If searchCriteria.SelectedIndex = -1 Then
            If Search.Count = 1 Then
                Search = New Collections.Generic.List(Of String)
                query = New Collections.Generic.List(Of String)
            Else
                Search.RemoveAt(Search.Count - 1)
                query.RemoveAt(Search.Count - 1)
            End If
        Else
            Search.RemoveAt(searchCriteria.SelectedIndex)
            query.RemoveAt(searchCriteria.SelectedIndex)
        End If

        If Search.Count > 0 Then
            If Search.Item(0).Substring(0, 3) = "AND" Then
                Search.Item(0) = Search.Item(0).Substring(3, Search.Item(0).Length - 3)
            ElseIf Search.Item(0).Substring(0, 2) = "OR" Then
                Search.Item(0) = Search.Item(0).Substring(2, Search.Item(0).Length - 2)
            End If
        End If

        If query.Count > 0 Then
            If query.Item(0).Substring(0, 3) = "AND" Then
                query.Item(0) = query.Item(0).Substring(3, query.Item(0).Length - 3)
            ElseIf query.Item(0).Substring(0, 2) = "OR" Then
                query.Item(0) = query.Item(0).Substring(2, query.Item(0).Length - 2)
            End If

            query.Item(0) = "AND" & query.Item(0)
        End If

        Session("Search") = Search
        Session("Query") = query

        BindCreteria(Search)
        detector()
        resetController()
        ddlSearchUsing.Focus()
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Search = New Collections.Generic.List(Of String)
        query = New Collections.Generic.List(Of String)
        Session("Search") = Search
        Session("Query") = query

        BindCreteria(Search)
        detector()
        resetController()
    End Sub

    Protected Sub chkDeleted_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDeleted.CheckedChanged
        Dim setting As Control.Base = CType(Me.Page, Control.Base)
        setting.ShowDeleted = chkDeleted.Checked
        Response.Redirect(setting.GenerateList)
    End Sub

End Class
