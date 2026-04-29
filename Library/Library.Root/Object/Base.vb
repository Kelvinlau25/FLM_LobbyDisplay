Namespace [Object]
    ''' <summary>
    ''' Must Inheritance Common Object (net8.0 migration — System.Web.HttpContext removed)
    ''' Session/request values replaced with String.Empty; web layer handles user context.
    ''' -------------------------------------------
    ''' C.C.Yeon      25 April 2011  initial Version
    ''' </summary>
    Public MustInherit Class Base
        Public Sub New()
            _id = 0
            _createdby = String.Empty
            _createddate = Now
            _createdloc = String.Empty
            _Updatedby = String.Empty
            _updatedDate = Now
            _UpdatedLoc = String.Empty
        End Sub

        Private _id As Integer = 0
        Public Property ID() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Private _rectype As String = String.Empty
        Public Property Record_Type() As String
            Get
                Return _rectype
            End Get
            Set(ByVal value As String)
                _rectype = value
            End Set
        End Property

        Private _createdby As String = String.Empty
        Public Property CreatedBy() As String
            Get
                Return _createdby
            End Get
            Set(ByVal value As String)
                _createdby = value
            End Set
        End Property

        Private _createddate As DateTime = Now
        Public Property CreatedDate() As DateTime
            Get
                Return _createddate
            End Get
            Set(ByVal value As DateTime)
                _createddate = value
            End Set
        End Property

        Private _createdloc As String = String.Empty
        Public Property CreatedLoc() As String
            Get
                Return _createdloc
            End Get
            Set(ByVal value As String)
                _createdloc = value
            End Set
        End Property

        Private _Updatedby As String = String.Empty
        Public Property UpdatedBy() As String
            Get
                Return _Updatedby
            End Get
            Set(ByVal value As String)
                _Updatedby = value
            End Set
        End Property

        Private _updatedDate As DateTime = Now
        Public Property UpdatedDate() As DateTime
            Get
                Return _updatedDate
            End Get
            Set(ByVal value As DateTime)
                _updatedDate = value
            End Set
        End Property

        Private _UpdatedLoc As String = String.Empty
        Public Property UpdatedLoc() As String
            Get
                Return _UpdatedLoc
            End Get
            Set(ByVal value As String)
                _UpdatedLoc = value
            End Set
        End Property

    End Class
End Namespace
