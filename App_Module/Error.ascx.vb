
Partial Class App_Module_ErrorReport
    Inherits System.Web.UI.UserControl

    Private _validationGroup As String = ""
    Public Property ValidationGroup() As String
        Get
            Return _validationGroup
        End Get
        Set(ByVal value As String)
            _validationGroup = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        vsSummary.ValidationGroup = Me.ValidationGroup
    End Sub
End Class
