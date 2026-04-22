Public MustInherit Class Page
    Inherits System.Web.UI.Page

    Private _controlPanel As System.Web.UI.WebControls.PlaceHolder = Nothing
    Public _list As New Collections.Generic.List(Of String)

    Public Property ControlPanel() As System.Web.UI.WebControls.PlaceHolder
        Get
            Return _controlPanel
        End Get
        Set(ByVal value As System.Web.UI.WebControls.PlaceHolder)
            _controlPanel = value
        End Set
    End Property

    Public Sub Remove(ByVal Ctrl As System.Web.UI.Control)
        Ctrl.Visible = False
  
    End Sub
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
