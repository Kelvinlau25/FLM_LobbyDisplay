Public MustInherit Class UserControl
    Inherits System.Web.UI.UserControl

    Private _editmode As Boolean = False
    Public Property EditMode() As Boolean
        Get
            Return _editmode
        End Get
        Set(ByVal value As Boolean)
            _editmode = value
        End Set
    End Property

    Private _ValidationGroup As String
    Public Property ValidationGroup() As String
        Get
            Return _ValidationGroup
        End Get
        Set(ByVal value As String)
            _ValidationGroup = value
        End Set
    End Property

    Public MustOverride Sub Clear()
    Public MustOverride Function AddFuntion() As Boolean
    Public MustOverride Function DeleteFuntion() As Boolean
    Public MustOverride Function EditFuntion() As Boolean
    Public MustOverride Function PermissionFuntion() As Boolean
    Public MustOverride Sub View()
    Public MustOverride Sub AdminProceed()
    Public MustOverride Function getrowcount() As Integer
    Public MustOverride Sub DepartProceed()
    Public MustOverride Sub ActionTakenProceed()
    Public MustOverride Sub SendMailtoPICLevelOne()
    Public MustOverride Function SetPICStatus() As String
    Public MustOverride Sub SetPICStatusDate()
    Public MustOverride Sub SetPICButton()
    Public MustOverride Sub SetPICButtonForPIC()
    Public MustOverride Function validation() As Boolean
    Public MustOverride Function validationForSaveButton() As Boolean
    Public MustOverride Function ShowOrHide() As Boolean


End Class
