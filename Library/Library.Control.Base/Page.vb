Public MustInherit Class Page
    ' net8.0 migration: System.Web.UI.Page inheritance removed.
    ' This is now a plain abstract base class for page-level properties.

    Private _Action As Int16
    Public Property Action() As Int16
        Get
            Return _Action
        End Get
        Set(ByVal value As Int16)
            _Action = value
        End Set
    End Property

    Private _CurrentStep As Int16
    Public Property CurrentStep() As Int16
        Get
            Return _CurrentStep
        End Get
        Set(ByVal value As Int16)
            _CurrentStep = value
        End Set
    End Property

    Private _Worksno As String
    Public Property Worksno() As String
        Get
            Return _Worksno
        End Get
        Set(ByVal value As String)
            _Worksno = value
        End Set
    End Property

    Private _CelNo As String
    Public Property CelNo() As String
        Get
            Return _CelNo
        End Get
        Set(ByVal value As String)
            _CelNo = value
        End Set
    End Property

    Private _CompCode As String
    Public Property CompCode() As String
        Get
            Return _CompCode
        End Get
        Set(ByVal value As String)
            _CompCode = value
        End Set
    End Property

    Private _Reqno As String
    Public Property Reqno() As String
        Get
            Return _Reqno
        End Get
        Set(ByVal value As String)
            _Reqno = value
        End Set
    End Property
End Class
