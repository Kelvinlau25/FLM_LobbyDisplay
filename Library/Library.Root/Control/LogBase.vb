Imports System.Collections.Specialized

Namespace Control
    ''' <summary>
    ''' Log page base class (net8.0 migration — System.Web.UI.Page removed)
    ''' QueryString supplied via constructor parameter.
    ''' </summary>
    Public MustInherit Class LogBase

        Protected _list As Other.GenericCollection(Of [Object].Log)
        Private ReadOnly _queryString As NameValueCollection

        Protected Sub New(ByVal queryString As NameValueCollection)
            _queryString = queryString
            Me.BindKey()
            Me.BindData()
        End Sub

        Private Sub BindKey()
            For Each _query As String In _queryString
                If Not String.IsNullOrEmpty(_query) Then
                    Select Case _query
                        Case "id"
                            Me._key = Uri.UnescapeDataString(_queryString("id"))
                        Case "key"
                            Me._setupKey = _queryString("key")
                        Case "page"
                            If IsNumeric(_queryString("page")) Then
                                Me._Pageno = _queryString("page")
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
                Return Me.LogPage & "?id=" & Uri.EscapeDataString(Me.Key) & "&key=" & Me.SetupKey & "&page=" & Me.PageNo
            End Get
        End Property

    End Class
End Namespace
