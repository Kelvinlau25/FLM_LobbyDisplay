Imports System.Collections.Specialized

Namespace Control
    ''' <summary>
    ''' Handler All Page Common Function (net8.0 migration — System.Web.UI.Page removed)
    ''' -------------------------------------------------------------------------------
    ''' C.C.Yeon    25 April 2011   initial Version
    ''' (migrated)  Removed Web Forms lifecycle, GridView, and System.Web dependencies.
    '''             IPageContext injected for Redirect/ResolveUrl/UrlEncode/UrlDecode.
    '''             QueryString supplied via constructor parameter.
    '''             ViewState replaced with in-memory Dictionary.
    ''' </summary>
    Public MustInherit Class Base

        Public Enum EnumAction
            None = 0
            Add = 1
            Edit = 3
            Delete = 5
            View = 7
            History = 9
        End Enum

        ' Injected dependencies
        Private ReadOnly _pageContext As IPageContext
        Private ReadOnly _queryString As NameValueCollection
        Private ReadOnly _isPostBack As Boolean
        Private _viewState As New Dictionary(Of String, Object)

        Protected Sub New(ByVal pageContext As IPageContext, ByVal queryString As NameValueCollection, ByVal isPostBack As Boolean)
            _pageContext = pageContext
            _queryString = queryString
            _isPostBack = isPostBack

            If Me.FunctionControl Then
                Me.BindAction()
            End If

            Me.BindSort()

            If Me.FunctionControl Then
                Me.CheckURL()
            End If

            If Me.FunctionControl Then
                Me.BindKey()
            End If
        End Sub

        Public Sub CheckURL()
            If Not _isPostBack Then
                If Me.DefaultSort <> String.Empty Then
                    If SortField = String.Empty Then
                        _pageContext.Redirect(GenerateList)
                    End If
                Else
                    If Me._Action = EnumAction.None Then
                        _pageContext.Redirect(GetUrl(EnumAction.None))
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Retrieve the Key from ViewState or URL
        ''' </summary>
        Private Sub BindKey()
            If _viewState("Key") Is Nothing Then
                Me._Key = _queryString("id")
                _viewState("Key") = Me._Key
            Else
                Me._Key = _viewState("Key")
            End If
        End Sub

        ''' <summary>
        ''' Retrieve the Sort Field and Sort Direction
        ''' </summary>
        Private Sub BindSort()
            For Each _query As String In _queryString
                If Not String.IsNullOrEmpty(_query) Then
                    Select Case _query
                        Case "sort"
                            Me._SortField = _queryString("sort")
                        Case "dic"
                            Me._sortDirection = _queryString("dic")
                        Case "page"
                            If IsNumeric(_queryString("page")) Then
                                Me._Pageno = _queryString("page")
                            Else
                                Me._Pageno = 1
                            End If
                        Case "fld"
                            Me._SearchField = Uri.UnescapeDataString(_queryString("fld"))
                        Case "vl"
                            Dim ASD As String = _queryString("vl")
                            If ASD.Contains("+") Then
                                Me._SearchValue = Uri.UnescapeDataString(_queryString("vl"))
                            Else
                                Me._SearchValue = _queryString("vl")
                            End If
                        Case "type"
                            Me._type = _queryString("type")
                        Case "itm1"
                            Me._item1 = _queryString("itm1")
                        Case "itm2"
                            Me._item2 = _queryString("itm2")
                        Case "itm3"
                            Me._item3 = _queryString("itm3")
                        Case "itm4"
                            Me._item4 = _queryString("itm4")
                        Case "itm5"
                            Me._item5 = _queryString("itm5")
                        Case "itm6"
                            Me._item6 = _queryString("itm6")
                        Case "itm7"
                            Me._item7 = _queryString("itm7")
                        Case "itm8"
                            Me._item8 = _queryString("itm8")
                        Case "itm9"
                            Me._item9 = _queryString("itm9")
                        Case "itm10"
                            Me._item10 = _queryString("itm10")
                        Case "dlt"
                            Me._ShowDeleted = _queryString("dlt")
                        Case "id"
                            If Me.DetailListingFunction Then
                                Me._Key = _queryString("id")
                            End If
                        Case "k"
                            Me._searchkeyword = _queryString("k")
                        Case "t"
                            Me._searchtype = _queryString("t")
                    End Select
                End If
            Next
        End Sub

        ''' <summary>
        ''' Retrieve the Action From URL
        ''' </summary>
        Private Sub BindAction()
            If Not String.IsNullOrEmpty(_queryString("action")) Then
                Select Case _queryString("action")
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
                Return Uri.UnescapeDataString(_SearchField)
            End Get
            Set(ByVal value As String)
                _SearchField = Uri.EscapeDataString(value)
            End Set
        End Property

        Private _SearchValue As String = String.Empty
        Public Property SearchValue() As String
            Get
                Return Uri.UnescapeDataString(_SearchValue)
            End Get
            Set(ByVal value As String)
                If value.IndexOf("AND ") <> -1 Or value.IndexOf("OR ") <> -1 Then
                    If value.Substring(0, 4).IndexOf("AND ") <> -1 Then
                        value = value.Substring(3, value.Length - 3)
                    ElseIf value.Substring(0, 3).IndexOf("OR ") <> -1 Then
                        value = value.Substring(2, value.Length - 2)
                    End If
                End If
                _SearchValue = Uri.EscapeDataString(value)
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
                Return Uri.UnescapeDataString(_item1)
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

        Private _searchtype As String = ""
        Public Property SearchType() As String
            Get
                Return _searchtype
            End Get
            Set(ByVal value As String)
                _searchtype = value
            End Set
        End Property

        Private _searchkeyword As String = ""
        Public Property SearchKeyword() As String
            Get
                Return _searchkeyword
            End Get
            Set(ByVal value As String)
                _searchkeyword = value
            End Set
        End Property

        '5)
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
                Return _pageContext.ResolveUrl(Me.ListPage) & "?sort=" & If(Me.SortField = String.Empty, DefaultSort, Me.SortField) & _
                "&dic=" & If(Me.SortDirection = String.Empty, "0", Me.SortDirection) & "&page=" & Me._Pageno & "&fld=" & Uri.EscapeDataString(Me._SearchField) & "&vl=" & _
                Uri.EscapeDataString(Me._SearchValue) & "&type=" & Me._type & "&itm1=" & Uri.EscapeDataString(Me._item1) & "&itm2=" & Me._item2 & "&itm3=" & Me._item3 & "&itm4=" & _
                Me._item4 & "&itm5=" & Me._item5 & "&itm6=" & Me._item6 & "&itm7=" & Me._item7 & "&itm8=" & Me._item8 & "&itm9=" & Me._item9 & _
                "&itm10=" & Me._item10 & "&dlt=" & Me._ShowDeleted & If(Me._DetailListingFunction, "&" & DetailListingAddtionalFunction(), String.Empty) & _
                IIf((_searchtype.Length > 0), "&t=" & _searchtype, "") & IIf((_searchkeyword.Length > 0), "&k=" & _searchkeyword, "")
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
                    Return URL & If(URL.IndexOf("?") <> -1, "&", "?") & "ReturnURL=" & Uri.EscapeDataString(_queryString.ToString())
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
                        tempurl = _pageContext.ResolveUrl(Me.DetailPage & "?action=" & CInt(EnumAction.Add))
                    Case EnumAction.Delete
                        If Me.DeleteRedirectList Then
                            tempurl = Me.GenerateList & "&action=" & CInt(EnumAction.Delete) & "&id=" & IIf(key = "", Me.Key, key)
                        Else
                            tempurl = _pageContext.ResolveUrl(Me.DetailPage & "?action=" & CInt(EnumAction.Delete) & "&id=" & IIf(key = "", Me.Key, key))
                        End If
                    Case EnumAction.Edit
                        tempurl = _pageContext.ResolveUrl(Me.DetailPage & "?action=" & CInt(EnumAction.Edit) & "&id=" & IIf(key = "", Me.Key, key))
                    Case EnumAction.View
                        tempurl = _pageContext.ResolveUrl(Me.DetailPage & "?action=" & CInt(EnumAction.View) & "&id=" & IIf(key = "", Me.Key, key))
                    Case EnumAction.History
                        tempurl = _pageContext.ResolveUrl(Me.LogPage & "?id=" & IIf(key = "", Me.Key, key) & "&key=" & Me.SetupKey & "&act=" & Me.Action & "&page=1")
                    Case Else
                        tempurl = _pageContext.ResolveUrl(Me.ListPage)
                End Select

                Return AddReturnURLControl(tempurl)
            End Get
        End Property

        ' This Key must Same With The Resource Page
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

        '14)
        Public Function GeneratePrintPage() As String
            GeneratePrintPage = _pageContext.ResolveUrl(PrintPage & "?type=" & Me.SetupKey & "&itm1=" & Me.Item1)
        End Function

        '17)
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

        Public MustOverride Sub BindData()

        Public ReadOnly Property NormalTitle() As String
            Get
                Return Me.DisplayTitle
            End Get
        End Property

    End Class
End Namespace
