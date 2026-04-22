Namespace [Object]
    Public Class Log

        Private _ID As Integer = 0
        Public Property ID() As Integer
            Get
                Return _ID
            End Get
            Set(ByVal value As Integer)
                _ID = value
            End Set
        End Property

        Private _keyField As String = String.Empty
        Public Property KeyField() As String
            Get
                Return _keyField
            End Get
            Set(ByVal value As String)
                _keyField = value
            End Set
        End Property

        Private _KeyDesc As String = String.Empty
        Public Property KeyDesc() As String
            Get
                Return _KeyDesc
            End Get
            Set(ByVal value As String)
                _KeyDesc = value
            End Set
        End Property

        Private _keyValue As String = String.Empty
        Public Property KeyValue() As String
            Get
                Return _keyValue
            End Get
            Set(ByVal value As String)
                _keyValue = value
            End Set
        End Property

        Private _fieldname As String = String.Empty
        Public Property FieldName() As String
            Get
                Return _fieldname
            End Get
            Set(ByVal value As String)
                _fieldname = value
            End Set
        End Property

        Private _b4Update As String = String.Empty
        Public Property B4Update() As String
            Get
                Return _b4Update
            End Get
            Set(ByVal value As String)
                _b4Update = value
            End Set
        End Property

        Private _afUpdate As String = String.Empty
        Public Property AFUpdate() As String
            Get
                Return _afUpdate
            End Get
            Set(ByVal value As String)
                _afUpdate = value
            End Set
        End Property

        Private _updatedby As String = String.Empty
        Public Property UpdateBy() As String
            Get
                Return _updatedby
            End Get
            Set(ByVal value As String)
                _updatedby = value
            End Set
        End Property

        Private _updateDate As String = String.Empty
        Public Property UpdatedDate() As String
            Get
                Return _updateDate
            End Get
            Set(ByVal value As String)
                _updateDate = value
            End Set
        End Property
    End Class
End Namespace

