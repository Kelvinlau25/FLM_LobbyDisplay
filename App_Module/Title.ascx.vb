
''' <summary>
''' Display Title 
''' ---------------------------------------
''' 21 May 2012    Yeon  Add Title properties
''' </summary>
''' <remarks></remarks>
Partial Class App_Module_Title
    Inherits System.Web.UI.UserControl

    Private _audit As Boolean = False
    Public WriteOnly Property Audit() As Boolean
        Set(ByVal value As Boolean)
            _audit = value
        End Set
    End Property

    Private _title As String = String.Empty
    ''' <summary>
    ''' this will be priority
    ''' </summary>
    Public Property Title() As String
        Get
            Return _title
        End Get
        Set(ByVal value As String)
            _title = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.Title = String.Empty Then
            If _audit = False Then
                Dim setting As Control.Base = CType(Me.Page, Control.Base)
                Me.lblFormTitle.Text = setting.DisplayTitle & If(setting.Action <> Control.Base.EnumAction.None, " - ", String.Empty) & setting.ActionDesc
            Else
                Dim setting As Control.LogBase = CType(Me.Page, Control.LogBase)
                Me.lblFormTitle.Text = setting.DisplayTitle
            End If
        Else
            If _audit = False Then
                Dim setting As Control.Base = CType(Me.Page, Control.Base)
                Me.lblFormTitle.Text = Me.Title & If(setting.Action <> Control.Base.EnumAction.None, " - ", String.Empty) & setting.ActionDesc
            Else
                Me.lblFormTitle.Text = Me.Title
            End If
        End If

        Me.Page.Title = lblFormTitle.Text
    End Sub
End Class
