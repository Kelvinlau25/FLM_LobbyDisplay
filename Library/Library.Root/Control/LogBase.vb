Namespace Control
    Public MustInherit Class LogBase
        Inherits System.Web.UI.Page

        Protected _list As Other.GenericCollection(Of [Object].Log)

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            Me.BindKey()
            MyBase.OnInit(e)
            Me.BindData()
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)
        End Sub

        Private Sub BindKey()
            For Each _query As String In Request.QueryString
                If Not String.IsNullOrEmpty(_query) Then
                    Select Case _query
                        Case "id"
                            Me._key = Server.UrlDecode(Request.QueryString("id"))
                        Case "key"
                            Me._setupKey = Request.QueryString("key")
                        Case "page"
                            If IsNumeric(Request.QueryString("page")) Then
                                Me._Pageno = Request.QueryString("page")
                            Else
                                Me._Pageno = 1
                            End If
                    End Select
                End If
            Next
        End Sub

        Protected MustOverride Sub BindData()

        Private _key As String = String.Empty
        Public ReadOnly Property Key() As String
            Get
                Return _key
            End Get
        End Property

        Private _Pageno As Integer = 0
        Public Property PageNo() As Integer
            Get
                Return _Pageno
            End Get
            Set(ByVal value As Integer)
                _Pageno = value
            End Set
        End Property

        Private _setupKey As String = String.Empty
        Public ReadOnly Property SetupKey() As String
            Get
                Return _setupKey
            End Get
        End Property

        Public ReadOnly Property KeyDesc() As String
            Get
                If _list.TotalRow > 0 Then
                    Return _list.Data.Item(0).KeyDesc
                Else
                    Return String.Empty
                End If
            End Get
        End Property

        Public MustOverride ReadOnly Property LogTitle() As String
        Public MustOverride ReadOnly Property LogPage() As String

        Public ReadOnly Property NormalTitle() As String
            Get
                Return Me.LogTitle
            End Get
        End Property

        Public ReadOnly Property DisplayTitle() As String
            Get
                Return Me.LogTitle & " Audit Trail"
            End Get
        End Property

        Public ReadOnly Property GenerateList() As String
            Get
                Return ResolveUrl(Me.LogPage & "?id=" & Server.UrlEncode(Me.Key) & "&key=" & Me.SetupKey & "&page=" & Me.PageNo)
            End Get
        End Property
    End Class
End Namespace

