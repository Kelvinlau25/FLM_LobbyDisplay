
Namespace [Object]

    Public Class UserProfile

        Private _COMPANY As String
        Private _ORGANIZATION As String
        Private _USER_ID As String
        Private _USR_NAME As String
        Private _EMP_NAME As String
        Private _USR_EMAIL As String
        Private _DATE_JOIN As DateTime

        Public Sub New(ByVal company As String, ByVal organization As String, ByVal user_id As String, ByVal usr_name As String, ByVal emp_name As String, ByVal usr_email As String, ByVal date_join As DateTime)
            _COMPANY = company
            _ORGANIZATION = organization
            _USER_ID = user_id
            _USR_NAME = usr_name
            _EMP_NAME = emp_name
            _USR_EMAIL = usr_email
            _DATE_JOIN = date_join
        End Sub

        Public ReadOnly Property Company()
            Get
                Return _COMPANY
            End Get
        End Property

        Public ReadOnly Property Organization()
            Get
                Return _ORGANIZATION
            End Get
        End Property

        Public ReadOnly Property UserID()
            Get
                Return _USER_ID
            End Get
        End Property

        Public ReadOnly Property Username()
            Get
                Return _USR_NAME
            End Get
        End Property

        Public ReadOnly Property Name()
            Get
                Return _EMP_NAME
            End Get
        End Property

        Public ReadOnly Property Email()
            Get
                Return _USR_EMAIL
            End Get
        End Property

        Public ReadOnly Property DateJoin()
            Get
                Return _DATE_JOIN
            End Get
        End Property

    End Class


End Namespace