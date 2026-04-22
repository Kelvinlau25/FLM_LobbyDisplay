Namespace [Object]
    ''' <summary>
    ''' Object of the List item ( Contain Text and Value )
    ''' --------------------------------------------------
    ''' C.C.Yeon     25 April 2011   Initial Version
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Binder
        Public Sub New()
            _Text = String.Empty
            _Value = String.Empty
        End Sub

        Public Sub New(ByVal Text As String, ByVal Value As String)
            _Text = Text
            _Value = Value
        End Sub

        Private _Text As String
        Public Property Text() As String
            Get
                Return _Text
            End Get
            Set(ByVal value As String)
                _Text = value
            End Set
        End Property

        Private _Value As String
        Public Property Value() As String
            Get
                Return _Value
            End Get
            Set(ByVal value As String)
                _Value = value
            End Set
        End Property

    End Class
End Namespace


