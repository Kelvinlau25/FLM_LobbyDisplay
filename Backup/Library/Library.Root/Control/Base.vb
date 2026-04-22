Imports System.Web.UI.WebControls

Namespace Control
    ''' <summary>
    ''' Handler All Page Common Function 
    ''' 1 ) Retrieve and Determine Key
    ''' 2 ) Retrieve and Determine Action(Insert / Update / Delete)
    ''' 3 ) Generate (Insert / Edit / Delete / List / View ) URL Based on the Setup Key
    ''' 4 ) Generete Title Based on the Setup Key
    ''' 5 ) Generate Action Desc 
    ''' 6 ) Retrieve and Determine Sort Field , Sort Direction 
    ''' 7 ) Generate the List View URL (Include Sort Field , Sort Value and Page No )
    ''' 8 ) Function Control property , Default = true , if false all the generate list will not auto generate, this is to igone the error wish the page doest not have setting in the resource page
    ''' 9 ) Delete Control property , Default = true, if false then the show deleted Check box will be disappear.
    ''' 10) List Page Path (Read Only)
    ''' 11) Detail Page Path (Read Only)
    ''' 12) Log Page Path (Read Only)
    ''' 13) Add Show Delete Property 
    ''' 14) Print Page Parameter Generator
    ''' 15) MustOverride Databind Function
    ''' 16) PrintPage Property (Read Only)
    ''' 17) Add item into item1 ID (URL Parameter)
    ''' 18) Remove item From item1 (URL Parameter)
    ''' 19) Check the Id was In teh list 
    ''' 20) Add Control Property
    ''' 21) Advance Control Property
    ''' 22) Delete Image Path Property
    ''' 23) Grid View Property 
    ''' 24) Grid View First Column Checkbox Property
    ''' 25) Grid View Mouse Out script
    ''' 26) Grid View Mouse Over Script
    ''' 27) Print Control Property
    ''' 28) View History Control Proprty
    ''' 29) Show Delete Control Property , Default = true, if false then the show deleted Check box will be disappear.(note if delete control = false, this will be automatically false)
    ''' 30) Delete Redirect the same page Control : Default : False
    ''' 31) URL Additional Add ReturnURL paramater control. if true then then url at the back will attach the currrent url.
    '''     note : this is use to back to the previus page.
    ''' 32) Record Type Column Property - This will handler the list for the delete record type
    ''' 33) Deleted Text for Hidden the Delete Control
    ''' 34) Deleted Column Compare, if match the action will be follow the properties setting.
    ''' 35) List for Detail Page(Detail Page was containt List Page), if true the parameter will carry to the detail page and combine with the listing page
    ''' 36) Delete confirmation box in the listing page
    ''' 37) Delete class name
    ''' 
    ''' Remark : The default sort property will determine wherether the url is check or not
    ''' check ( if the url failed retrieve the sort field then will generete and redirect its
    ''' -------------------------------------------------------------------------------
    ''' C.C.Yeon    25 April 2011   initial Version
    ''' C.C.Yeon    12 May   2011   Add FucntionControl Property
    ''' C.C.Yeon    08 July  2011   Add Print Page Parameter Generater Function
    ''' C.C.Yeon    29 April 2012   Add DetailListingFunction was handler the detail was contains listing function
    ''' C.C.Yeon    07 May   2012   Add Control for the Delete confirmation  
    ''' C.C.Yeon    24 June  2012   Add Delete Class Name
    ''' Elven Lee   05 June  2013   Property [Record Type Column], 32, handle additional control field added in GridViewRow to ease developer.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class Base
        Inherits System.Web.UI.Page

        Public Enum EnumAction
            None = 0
            Add = 1
            Edit = 3
            Delete = 5
            View = 7
            History = 9
        End Enum

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            If (Me.FunctionControl) Then
                Me.BindAction()
            End If

            Me.BindSort()
            MyBase.OnInit(e)

            If (Me.FunctionControl) Then
                Me.CheckURL()
            End If
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            If Me.FunctionControl Then
                Me.BindKey()
            End If

            If Me.CustomTitle = False Then
                Me.Title = Me.DisplayTitle
            End If

            MyBase.OnLoad(e)

            If Me.GridView IsNot Nothing Then
                If Me.DeleteControl Then
                    Dim _field As TemplateField = New TemplateField
                    _field.ItemTemplate = New deletefield(ListItemType.Item)
                    _field.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                    _field.ControlStyle.Width = Unit.op_Implicit(30)
                    _field.ItemStyle.Width = Unit.op_Implicit(30)
                    _field.HeaderStyle.Width = Unit.op_Implicit(30)
                    _field.ItemStyle.CssClass = "Delete"
                    Me.GridView.Columns.Insert(0, _field)
                End If

                If Me.GridViewCheckColumn Then
                    Dim _field As TemplateField = New TemplateField()
                    _field.ItemTemplate = New checkboxfield(ListItemType.Item)
                    _field.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                    _field.HeaderTemplate = New checkboxfield(ListItemType.Header)
                    _field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                    _field.ControlStyle.Width = Unit.op_Implicit(30)
                    _field.ItemStyle.Width = Unit.op_Implicit(30)
                    _field.HeaderStyle.Width = Unit.op_Implicit(30)
                    Me.GridView.Columns.Insert(0, _field)
                End If

                If Me.GridViewRadioColumn Then
                    Dim _field As TemplateField = New TemplateField()
                    _field.ItemTemplate = New radiobuttonfield(ListItemType.Item)
                    _field.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                    '_field.HeaderTemplate = New radiobuttonfield(ListItemType.Header)
                    '_field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                    _field.ControlStyle.Width = Unit.op_Implicit(30)
                    _field.ItemStyle.Width = Unit.op_Implicit(30)
                    _field.HeaderStyle.Width = Unit.op_Implicit(30)
                    Me.GridView.Columns.Insert(0, _field)
                End If


                If Me.ViewHistoryControl Then
                    Dim _field As TemplateField = New TemplateField()
                    _field.ItemTemplate = New historyfield(ListItemType.Item)
                    _field.ItemStyle.HorizontalAlign = HorizontalAlign.Center
                    _field.HeaderTemplate = New historyfield(ListItemType.Header)
                    _field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center
                    _field.ControlStyle.Width = Unit.op_Implicit(70)
                    _field.ItemStyle.Width = Unit.op_Implicit(70)
                    _field.HeaderStyle.Width = Unit.op_Implicit(70)
                    Me.GridView.Columns.Add(_field)
                End If

                AddHandler Me.GridView.Sorting, AddressOf Me.Sorting
                AddHandler Me.GridView.RowCreated, AddressOf Me.gridview_rowcreated
                AddHandler Me.GridView.RowDataBound, AddressOf Me.gridview_RowDataBound
            End If
        End Sub

        Private Sub Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
            Me.SortField = e.SortExpression.ToString
            Me.SortDirection = (Me.SortDirection + 1) Mod 2
            Response.Redirect(Me.GenerateList)
        End Sub

        Protected Sub CheckURL()
            If Not IsPostBack Then
                If Me.DefaultSort <> String.Empty Then
                    If SortField = String.Empty Then
                        Response.Redirect(GenerateList)
                    End If
                Else
                    If Me._Action = EnumAction.None Then
                        Response.Redirect(GetUrl(EnumAction.None))
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Retrieve the Key from View State or URL
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindKey()
            If ViewState("Key") Is Nothing Then
                Me._Key = Request.QueryString("id")
                ViewState("Key") = Me._Key
            Else
                Me._Key = ViewState("Key")
            End If
        End Sub

        ''' <summary>
        ''' Retrieve the Sort Field and Sort Direction
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindSort()
            For Each _query As String In Request.QueryString
                If Not String.IsNullOrEmpty(_query) Then
                    Select Case _query
                        Case "sort"
                            Me._SortField = Request.QueryString("sort")
                        Case "dic"
                            Me._sortDirection = Request.QueryString("dic")
                        Case "page"
                            If IsNumeric(Request.QueryString("page")) Then
                                Me._Pageno = Request.QueryString("page")
                            Else
                                Me._Pageno = 1
                            End If
                        Case "fld"
                            Me._SearchField = Server.UrlDecode(Request.QueryString("fld"))
                        Case "vl"
                            Dim ASD As String = Request.QueryString("vl")
                            If ASD.Contains("+") Then
                                Me._SearchValue = Server.UrlDecode(Request.QueryString("vl"))
                            Else
                                Me._SearchValue = Request.QueryString("vl")
                            End If
                            'Me._SearchValue = Server.UrlDecode(Request.QueryString("vl"))
                        Case "type"
                            Me._type = Request.QueryString("type")
                        Case "itm1"
                            Me._item1 = Request.QueryString("itm1")
                        Case "itm2"
                            Me._item2 = Request.QueryString("itm2")
                        Case "itm3"
                            Me._item3 = Request.QueryString("itm3")
                        Case "itm4"
                            Me._item4 = Request.QueryString("itm4")
                        Case "itm5"
                            Me._item5 = Request.QueryString("itm5")
                        Case "itm6"
                            Me._item6 = Request.QueryString("itm6")
                        Case "itm7"
                            Me._item7 = Request.QueryString("itm7")
                        Case "itm8"
                            Me._item8 = Request.QueryString("itm8")
                        Case "itm9"
                            Me._item9 = Request.QueryString("itm9")
                        Case "itm10"
                            Me._item10 = Request.QueryString("itm10")
                        Case "dlt"
                            Me._ShowDeleted = Request.QueryString("dlt")
                        Case "id"
                            If Me.DetailListingFunction Then
                                Me._Key = Request.QueryString("id")
                            End If

                            '' Add By Elven (19/Apr/2013)
                        Case "k"
                            Me._searchkeyword = Request.QueryString("k")
                        Case "t"
                            Me._searchtype = Request.QueryString("t")
                    End Select
                End If
            Next
        End Sub

        ''' <summary>
        ''' Retrieve the Action From URL
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindAction()
            If Not String.IsNullOrEmpty(Request.QueryString("action")) Then
                Select Case Request.QueryString("action")
                    Case 1
                        Me._Action = EnumAction.Add
                    Case 3
                        Me._Action = EnumAction.Edit
                    Case 5
                        Me._Action = EnumAction.Delete
                    Case 7
                        Me._Action = EnumAction.View
                    Case Else
                        Me._Action = EnumAction.None
                End Select
            End If
        End Sub

        '37 
        Private _DeleteClassName As String = String.Empty
        Public Property DeleteClassName() As String
            Get
                Return _DeleteClassName
            End Get
            Set(ByVal value As String)
                _DeleteClassName = value
            End Set
        End Property

        '36 
        Private _DeleteConfirmationBox As Boolean = False
        Public Property DeleteConfirmationBox() As Boolean
            Get
                Return _DeleteConfirmationBox
            End Get
            Set(ByVal value As Boolean)
                _DeleteConfirmationBox = value
            End Set
        End Property

        '35
        Private _DetailListingFunction As Boolean = False
        Public Property DetailListingFunction() As Boolean
            Get
                Return _DetailListingFunction
            End Get
            Set(ByVal value As Boolean)
                _DetailListingFunction = value
            End Set
        End Property

        '34
        Private _DeletedVisibleControl As Boolean = False
        Public Property DeletedVisibleControl() As Boolean
            Get
                Return _DeletedVisibleControl
            End Get
            Set(ByVal value As Boolean)
                _DeletedVisibleControl = value
            End Set
        End Property

        '33 
        Private _DetetedText As String = "Deleted"
        Public Property DeletedText() As String
            Get
                Return _DetetedText
            End Get
            Set(ByVal value As String)
                _DetetedText = value
            End Set
        End Property

        '32
        Private _recordTypeColumn As Integer = -1
        Public Property RecordTypeColumn() As Integer
            Get
                'Return _recordTypeColumn
                Return IIf(_recordTypeColumn > -1, _recordTypeColumn + IIf(Me.GridViewCheckColumn, 1, 0) + IIf(Me.GridViewRadioColumn, 1, 0) + IIf(Me.DeleteControl, 1, 0), -1)
            End Get
            Set(ByVal value As Integer)
                _recordTypeColumn = value
            End Set
        End Property

        '31) 
        Private _ReturnURLControl As Boolean = False
        Public Property ReturnURLControl() As Boolean
            Get
                Return _ReturnURLControl
            End Get
            Set(ByVal value As Boolean)
                _ReturnURLControl = value
            End Set
        End Property

        '30)
        Private _DeleteRedirectList As Boolean = False
        Public Property DeleteRedirectList() As Boolean
            Get
                Return _DeleteRedirectList
            End Get
            Set(ByVal value As Boolean)
                _DeleteRedirectList = value
            End Set
        End Property

        '29)
        Private _ShowDeletedControl As Boolean = True
        Public Property ShowDeletedControl() As Boolean
            Get
                Return Me._ShowDeletedControl
            End Get
            Set(ByVal value As Boolean)
                Me._ShowDeletedControl = value
            End Set
        End Property

        '28)
        Private _ViewHistoryControl As Boolean = True
        Public Property ViewHistoryControl() As Boolean
            Get
                Return Me._ViewHistoryControl
            End Get
            Set(ByVal value As Boolean)
                Me._ViewHistoryControl = value
            End Set
        End Property

        '27)
        Private _PrintControl As Boolean = True
        Public Property PrintControl() As Boolean
            Get
                Return Me._PrintControl
            End Get
            Set(ByVal value As Boolean)
                Me._PrintControl = value
            End Set
        End Property

        '26) 
        Private _gvMouseOver As String = "Highlight(this)"
        Public Property GridViewRowMouseOver() As String
            Get
                Return Me._gvMouseOver
            End Get
            Set(ByVal value As String)
                Me._gvMouseOver = value
            End Set
        End Property

        '25)
        Private _gvMouseOUT As String = "UnHighlight(this)"
        Public Property GridViewRowMouseOut() As String
            Get
                Return Me._gvMouseOUT
            End Get
            Set(ByVal value As String)
                Me._gvMouseOUT = value
            End Set
        End Property

        '24) 
        Private _checkcolumn As Boolean = True
        Public Property GridViewCheckColumn() As Boolean
            Get
                Return Me._checkcolumn
            End Get
            Set(ByVal value As Boolean)
                Me._checkcolumn = value
            End Set
        End Property

        Private _radiocolumn As Boolean = True
        Public Property GridViewRadioColumn() As Boolean
            Get
                Return Me._radiocolumn
            End Get
            Set(ByVal value As Boolean)
                Me._radiocolumn = value
            End Set
        End Property

        '23)
        Private _gridview As System.Web.UI.WebControls.GridView = Nothing
        Public Property GridView() As System.Web.UI.WebControls.GridView
            Get
                Return Me._gridview
            End Get
            Set(ByVal value As System.Web.UI.WebControls.GridView)
                Me._gridview = value
            End Set
        End Property


        '22)
        Private _deleteImagePath As String = "~/Image/delete1.gif"
        Public Property DeleteImagePath() As String
            Get
                Return Me._deleteImagePath
            End Get
            Set(ByVal value As String)
                Me._deleteImagePath = value
            End Set
        End Property

        '21)
        Private _AdvanceControl As Boolean = True
        Public Property AdvancedControl() As Boolean
            Get
                Return Me._AdvanceControl
            End Get
            Set(ByVal value As Boolean)
                Me._AdvanceControl = value
            End Set
        End Property


        '20)
        Private _AddControl As Boolean = True
        Public Property AddControl() As Boolean
            Get
                Return Me._AddControl
            End Get
            Set(ByVal value As Boolean)
                Me._AddControl = value
            End Set
        End Property

        Private _type As String = String.Empty
        Public ReadOnly Property Type() As String
            Get
                Return _type
            End Get
        End Property

        'Page Key
        Private _Key As String = String.Empty
        Public ReadOnly Property Key() As String
            Get
                Return _Key
            End Get
        End Property

        'Action Such as Insert,Edit or Delete
        Private _Action As EnumAction = EnumAction.None
        Public ReadOnly Property Action() As EnumAction
            Get
                Return _Action
            End Get
        End Property

        Private _DefaultSort As String = String.Empty
        Public Property DefaultSort() As String
            Get
                Return _DefaultSort
            End Get
            Set(ByVal value As String)
                _DefaultSort = value
            End Set
        End Property

        Private _sortDirection As String = "1"
        Public Property SortDirection() As String
            Get
                Return _sortDirection
            End Get
            Set(ByVal value As String)
                _sortDirection = value
            End Set
        End Property

        Private _SortField As String
        Public Property SortField() As String
            Get
                Return _SortField
            End Get
            Set(ByVal value As String)
                _SortField = value
            End Set
        End Property

        Private _Pageno As Integer = 1
        Public Property PageNo() As Integer
            Get
                Return _Pageno
            End Get
            Set(ByVal value As Integer)
                _Pageno = value
            End Set
        End Property

        Private _SearchField As String = String.Empty
        Public Property SearchField() As String
            Get
                Return Server.UrlDecode(_SearchField)
            End Get
            Set(ByVal value As String)
                _SearchField = Server.UrlEncode(value)
            End Set
        End Property

        Private _SearchValue As String = String.Empty
        Public Property SearchValue() As String
            Get
                Return Server.UrlDecode(_SearchValue)
            End Get
            Set(ByVal value As String)
                If value.IndexOf("AND ") <> -1 Or value.IndexOf("OR ") <> -1 Then
                    'Elimited the And or OR during the first words
                    If value.Substring(0, 4).IndexOf("AND ") <> -1 Then
                        value = value.Substring(3, value.Length - 3)
                    ElseIf value.Substring(0, 3).IndexOf("OR ") <> -1 Then
                        value = value.Substring(2, value.Length - 2)
                    End If
                End If
                _SearchValue = Server.UrlEncode(value)
            End Set
        End Property

        Private _ShowDeleted As Boolean = False
        Public Property ShowDeleted() As Boolean
            Get
                Return _ShowDeleted
            End Get
            Set(ByVal value As Boolean)
                _ShowDeleted = value
            End Set
        End Property

        Private _item1 As String = String.Empty
        Public Property Item1() As String
            Get
                Return Server.UrlDecode(_item1)
            End Get
            Set(ByVal value As String)
                _item1 = value
            End Set
        End Property

        Private _item2 As String = String.Empty
        Public Property Item2() As String
            Get
                Return _item2
            End Get
            Set(ByVal value As String)
                _item2 = value
            End Set
        End Property

        Private _item3 As String = String.Empty
        Public Property Item3() As String
            Get
                Return _item3
            End Get
            Set(ByVal value As String)
                _item3 = value
            End Set
        End Property

        Private _item4 As String = String.Empty
        Public Property Item4() As String
            Get
                Return _item4
            End Get
            Set(ByVal value As String)
                _item4 = value
            End Set
        End Property

        Private _item5 As String = String.Empty
        Public Property Item5() As String
            Get
                Return _item5
            End Get
            Set(ByVal value As String)
                _item5 = value
            End Set
        End Property

        Private _item6 As String = String.Empty
        Public Property Item6() As String
            Get
                Return _item6
            End Get
            Set(ByVal value As String)
                _item6 = value
            End Set
        End Property

        Private _item7 As String = String.Empty
        Public Property Item7() As String
            Get
                Return _item7
            End Get
            Set(ByVal value As String)
                _item7 = value
            End Set
        End Property

        Private _item8 As String = String.Empty
        Public Property Item8() As String
            Get
                Return _item8
            End Get
            Set(ByVal value As String)
                _item8 = value
            End Set
        End Property

        Private _item9 As String = String.Empty
        Public Property Item9() As String
            Get
                Return _item9
            End Get
            Set(ByVal value As String)
                _item9 = value
            End Set
        End Property

        Private _item10 As String = String.Empty
        Public Property Item10() As String
            Get
                Return _item10
            End Get
            Set(ByVal value As String)
                _item10 = value
            End Set
        End Property

        '' Add By Elven (19/Apr/2013)
        Private _searchtype As String = ""
        Public Property SearchType() As String
            Get
                Return _searchtype
            End Get
            Set(ByVal value As String)
                _searchtype = value
            End Set
        End Property

        '' Add By Elven (19/Apr/2013)
        Private _searchkeyword As String = ""
        Public Property SearchKeyword() As String
            Get
                Return _searchkeyword
            End Get
            Set(ByVal value As String)
                _searchkeyword = value
            End Set
        End Property

        '5 )
        Public ReadOnly Property ActionDesc() As String
            Get
                Select Case _Action
                    Case EnumAction.Add
                        Return "Add"
                    Case EnumAction.Delete
                        Return "Delete"
                    Case EnumAction.Edit
                        Return "Edit"
                    Case EnumAction.View
                        Return "View"
                    Case EnumAction.None
                        Return String.Empty
                    Case Else
                        Return String.Empty
                End Select
            End Get
        End Property

        ' 7)
        Public ReadOnly Property GenerateList() As String
            Get
                Return ResolveUrl(Me.ListPage) & "?sort=" & If(Me.SortField = String.Empty, DefaultSort, Me.SortField) & _
                "&dic=" & If(Me.SortDirection = String.Empty, "0", Me.SortDirection) & "&page=" & Me._Pageno & "&fld=" & Server.UrlEncode(Me._SearchField) & "&vl=" & _
                Server.UrlEncode(Me._SearchValue) & "&type=" & Me._type & "&itm1=" & Server.UrlEncode(Me._item1) & "&itm2=" & Me._item2 & "&itm3=" & Me._item3 & "&itm4=" & _
                Me._item4 & "&itm5=" & Me._item5 & "&itm6=" & Me._item6 & "&itm7=" & Me._item7 & "&itm8=" & Me._item8 & "&itm9=" & Me._item9 & _
                "&itm10=" & Me._item10 & "&dlt=" & Me._ShowDeleted & If(Me._DetailListingFunction, "&" & DetailListingAddtionalFunction(), String.Empty) & _
                IIf((_searchtype.Length > 0), "&t=" & _searchtype, "") & IIf((_searchkeyword.Length > 0), "&k=" & _searchkeyword, "") '' Add By Elven (19/Apr/2013)
            End Get
        End Property

        Private Function DetailListingAddtionalFunction() As String
            Dim _temp As String = String.Empty
            Select Case Action
                Case EnumAction.Add
                    _temp = "action=" & CInt(EnumAction.Add)
                Case EnumAction.Delete
                    _temp = "?action=" & CInt(EnumAction.Delete) & "&id=" & IIf(Key = "", Me.Key, Key)
                Case EnumAction.Edit
                    _temp = "action=" & CInt(EnumAction.Edit) & "&id=" & IIf(Key = "", Me.Key, Key)
                Case EnumAction.View
                    _temp = "action=" & CInt(EnumAction.View) & "&id=" & IIf(Key = "", Me.Key, Key)
                Case Else
                    _temp = String.Empty
            End Select

            Return _temp
        End Function

        Private Function AddReturnURLControl(ByVal URL As String) As String
            If URL <> String.Empty Then
                If ReturnURLControl Then
                    Return URL & If(URL.IndexOf("?") <> -1, "&", "?") & "ReturnURL=" & Server.UrlEncode(System.Web.HttpContext.Current.Request.RawUrl)
                Else
                    Return URL
                End If
            Else
                Return String.Empty
            End If
        End Function

        ' 3) 
        Public ReadOnly Property GetUrl(ByVal action As EnumAction, Optional ByVal key As String = "") As String
            Get
                If Me.SetupKey = String.Empty Then
                    Return String.Empty
                End If

                Dim tempurl As String = String.Empty

                Select Case action
                    Case EnumAction.Add
                        tempurl = ResolveUrl(Me.DetailPage & "?action=" & CInt(EnumAction.Add))
                    Case EnumAction.Delete
                        If Me.DeleteRedirectList Then
                            tempurl = Me.GenerateList & "&action=" & CInt(EnumAction.Delete) & "&id=" & IIf(key = "", Me.Key, key)
                        Else
                            tempurl = ResolveUrl(Me.DetailPage & "?action=" & CInt(EnumAction.Delete) & "&id=" & IIf(key = "", Me.Key, key))
                        End If
                    Case EnumAction.Edit
                        tempurl = ResolveUrl(Me.DetailPage & "?action=" & CInt(EnumAction.Edit) & "&id=" & IIf(key = "", Me.Key, key))
                    Case EnumAction.View
                        tempurl = ResolveUrl(Me.DetailPage & "?action=" & CInt(EnumAction.View) & "&id=" & IIf(key = "", Me.Key, key))
                    Case EnumAction.History
                        tempurl = ResolveUrl(Me.LogPage & "?id=" & IIf(key = "", Me.Key, key) & "&key=" & Me.SetupKey & "&act=" & Me.Action & "&page=1")
                    Case Else
                        tempurl = ResolveUrl(Me.ListPage)
                End Select

                Return AddReturnURLControl(tempurl)
            End Get
        End Property

        ' This Key must Same With The Resource Page
        ' Based On this key, the value will retrieved
        Private _SetupKey As String = String.Empty
        Public Property SetupKey() As String
            Get
                Return _SetupKey
            End Get
            Set(ByVal value As String)
                _SetupKey = value
            End Set
        End Property

        ' 4)
        Public MustOverride ReadOnly Property DisplayTitle() As String

        ' 8)
        Private _FunctionControl As Boolean = True
        Public Property FunctionControl() As Boolean
            Get
                Return _FunctionControl
            End Get
            Set(ByVal value As Boolean)
                _FunctionControl = value
            End Set
        End Property

        ' 9) 
        Private _DeleteControl As Boolean = True
        Public Property DeleteControl() As Boolean
            Get
                Return _DeleteControl
            End Get
            Set(ByVal value As Boolean)
                _DeleteControl = value
            End Set
        End Property

        Private _customTitle As Boolean = False
        Public Property CustomTitle() As Boolean
            Get
                Return _customTitle
            End Get
            Set(ByVal value As Boolean)
                _customTitle = value
            End Set
        End Property


        '10)
        Public MustOverride ReadOnly Property ListPage() As String

        '11)
        Public MustOverride ReadOnly Property DetailPage() As String

        '12)
        Public MustOverride ReadOnly Property LogPage() As String

        '16)
        Public MustOverride ReadOnly Property PrintPage() As String

        '14 ) 
        Public Function GeneratePrintPage() As String
            GeneratePrintPage = ResolveUrl(PrintPage & "?type=" & Me.SetupKey & "&itm1=" & Me.Item1)
        End Function

        '17 )
        Public Sub AddItem(ByVal id As String)
            Dim idlist() As String = Me.Item1.Split(",")

            If idlist.Contains(id) Then
                Exit Sub
            End If

            If Me._item1 = String.Empty Then
                Me.Item1 = id
            Else
                Me.Item1 &= "," & id
            End If
        End Sub

        '18
        Public Sub RemoveItem(ByVal id As String)
            Dim idlist() As String = Me.Item1.Split(",")

            If idlist.Contains(id) = False Then
                Exit Sub
            End If

            Me.Item1 = String.Empty

            For Each Str As String In idlist
                If Str <> id Then
                    If Me.Item1 = String.Empty Then
                        Me.Item1 = Str
                    Else
                        Me.Item1 &= "," & Str
                    End If
                End If
            Next
        End Sub

        '19 
        Public Function MatchID(ByVal GridViewID As String) As String
            MatchID = False

            Dim str() As String = Me.Item1.Split(",")
            If str.Length > 0 Then
                MatchID = str.Contains(GridViewID)
            End If
        End Function

        Protected Sub grdResult_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
            Me.SortField = e.SortExpression.ToString()
            Me.SortDirection = (CInt(Me.SortDirection) + 1) Mod 2
            MyBase.Response.Redirect(Me.GenerateList)
        End Sub

        Protected Sub grdResult_DoSorting(ByVal ColumnName As String, ByVal Sort As System.Web.UI.WebControls.SortDirection)
            Me.SortField = ColumnName
            Me.SortDirection = Sort
            MyBase.Response.Redirect(Me.GenerateList)
        End Sub

        Protected Sub gridview_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
            If e.Row.RowType = DataControlRowType.DataRow Then
                If Me.DeleteControl Then
                    If Me.RecordTypeColumn > -1 Then
                        If e.Row.Cells(Me.RecordTypeColumn).Text.ToUpper = Me.DeletedText.ToUpper Then
                            DirectCast(e.Row.FindControl("ltritem"), System.Web.UI.WebControls.Literal).Visible = Me._DeletedVisibleControl
                        Else
                            DirectCast(e.Row.FindControl("ltritem"), System.Web.UI.WebControls.Literal).Visible = Not Me._DeletedVisibleControl
                        End If
                    End If
                End If
            End If
        End Sub

        Private Function addClassName() As String
            If Me.DeleteClassName <> String.Empty Then
                Return "class='" & Me.DeleteClassName & "'"
            Else
                Return String.Empty
            End If
        End Function

        Private Sub gridview_rowcreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
            If (e.Row.RowType = Web.UI.WebControls.DataControlRowType.DataRow AndAlso Me.GridView IsNot Nothing) Then

                If Me.GridViewRowMouseOver <> String.Empty Then
                    e.Row.Attributes.Add("onMouseOver", Me.GridViewRowMouseOver)
                End If

                If Me.GridViewRowMouseOut <> String.Empty Then
                    e.Row.Attributes.Add("onMouseOut", Me.GridViewRowMouseOut)
                End If

                If Me.GridViewCheckColumn Then
                    Dim _item As System.Web.UI.WebControls.CheckBox = DirectCast(e.Row.FindControl("ckitem"), System.Web.UI.WebControls.CheckBox)
                    _item.Checked = Me.MatchID(Me.GridView.DataKeys(e.Row.RowIndex)(0))
                    AddHandler _item.CheckedChanged, AddressOf OnCheckChanged
                End If
                If Me.GridViewRadioColumn Then
                    Dim _item As System.Web.UI.WebControls.RadioButton = DirectCast(e.Row.FindControl("rbitem"), System.Web.UI.WebControls.RadioButton)
                    _item.Checked = Me.MatchID(Me.GridView.DataKeys(e.Row.RowIndex)(0))
                    AddHandler _item.CheckedChanged, AddressOf OnCheckChanged2
                End If

                If Me.DeleteControl Then
                    Dim _url As String = String.Empty
                    DirectCast(e.Row.FindControl("ltritem"), System.Web.UI.WebControls.Literal).Text = _
                    String.Format("<a target='_self' {3} href='{0}' {2}><img src='{1}' alt='Delete' /></a>", _
                                  Me.ResolveUrl(Me.GetUrl(EnumAction.Delete, Me.GridView.DataKeys(e.Row.RowIndex)(0))), _
                                  Me.ResolveUrl(Me.DeleteImagePath), _
                                  If(Me.DeleteConfirmationBox, "onclick=""return confirm('Are you sure you want to delete?')""", ""), addClassName())
                End If

                If Me.ViewHistoryControl Then
                    DirectCast(e.Row.FindControl("ltrhisitem"), System.Web.UI.WebControls.Literal).Text = String.Format("<a target='_blank' href='{0}'>View</a>", Me.GetUrl(EnumAction.History, Me.GridView.DataKeys(e.Row.RowIndex)(0)))
                End If
            End If

            If (e.Row.RowType = Web.UI.WebControls.DataControlRowType.Header AndAlso Me.GridView IsNot Nothing AndAlso Me.GridViewCheckColumn) Then
                If Me.GridViewCheckColumn Then
                    Dim _item As System.Web.UI.WebControls.CheckBox = DirectCast(e.Row.FindControl("chkall"), System.Web.UI.WebControls.CheckBox)

                    If Me.Item2 <> String.Empty Then
                        _item.Checked = Me.Item2
                    End If

                    AddHandler _item.CheckedChanged, AddressOf OnCheckAll
                End If
            End If
        End Sub

        Public Sub OnCheckAll(ByVal sender As Object, ByVal e As EventArgs)
            Dim checkbox As System.Web.UI.WebControls.CheckBox = DirectCast(sender, System.Web.UI.WebControls.CheckBox)

            Me.Item2 = checkbox.Checked

            For Each gvr As System.Web.UI.WebControls.GridViewRow In Me.GridView.Rows
                If checkbox.Checked Then
                    Me.AddItem(Me.GridView.DataKeys(gvr.RowIndex)(0))
                Else
                    Me.RemoveItem(Me.GridView.DataKeys(gvr.RowIndex)(0))
                End If
            Next

            MyBase.Response.Redirect(Me.GenerateList)
        End Sub

        Public Sub OnCheckChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim checkbox As System.Web.UI.WebControls.CheckBox = DirectCast(sender, System.Web.UI.WebControls.CheckBox)
            Dim contain As System.Web.UI.WebControls.GridViewRow = DirectCast(checkbox.NamingContainer, System.Web.UI.WebControls.GridViewRow)

            Me.Item2 = False
            If checkbox.Checked Then
                Me.AddItem(Me.GridView.DataKeys(contain.RowIndex)(0))
            Else
                Me.RemoveItem(Me.GridView.DataKeys(contain.RowIndex)(0))
            End If

            Response.Redirect(Me.GenerateList())
        End Sub
        Public Sub OnCheckChanged2(ByVal sender As Object, ByVal e As EventArgs)
            Dim radiobutton As System.Web.UI.WebControls.RadioButton = DirectCast(sender, System.Web.UI.WebControls.RadioButton)
            Dim contain As System.Web.UI.WebControls.GridViewRow = DirectCast(radiobutton.NamingContainer, System.Web.UI.WebControls.GridViewRow)

            Me.Item1 = ""
            Me.Item2 = False
            If radiobutton.Checked Then
                Me.AddItem(Me.GridView.DataKeys(contain.RowIndex)(0))
            End If

            Response.Redirect(Me.GenerateList())
        End Sub
    End Class
End Namespace

