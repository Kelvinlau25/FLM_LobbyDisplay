Public Class FieldSet
    Public Sub New(ByVal Field As String, ByVal Title As String, ByVal Type As EnumLib.DataType)
        Me.Field = Field
        Me.Title = Title
        Me.Type = Type
    End Sub

#Region "Properties"
    Private _Field As String = String.Empty
    Public Property Field() As String
        Get
            Return _Field
        End Get
        Set(ByVal value As String)
            _Field = value
        End Set
    End Property

    Private _title As String = String.Empty
    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property

    Public _type As EnumLib.DataType = EnumLib.DataType.String
    Public Property Type() As EnumLib.DataType
        Get
            Return _type
        End Get
        Set(ByVal value As EnumLib.DataType)
            _type = value
        End Set
    End Property
#End Region
End Class