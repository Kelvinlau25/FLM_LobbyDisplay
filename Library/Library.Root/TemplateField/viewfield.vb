Imports System.Web.UI.WebControls

Public Class viewfield
    Implements System.Web.UI.ITemplate

    Public Const LabelHeaderID As String = "lblview"
    Public Const LiteralItemID As String = "ltrviewitem"
    Private plittype As ListItemType

    Public Sub New(ByVal type As ListItemType)
        MyBase.New()
        Me.plittype = type
    End Sub

    Public Sub InstantiateIn(ByVal container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn
        Dim ltritem As Literal
        Dim lbdel As Label

        Select Case Me.plittype
            Case listItemType.Header
                lbdel = New Label()
                lbdel.Text = "View"
                lbdel.ID = "lblview"
                lbdel.EnableViewState = False
                container.Controls.Add(lbdel)
            Case listItemType.Item
                ltritem = New Literal()
                ltritem.ID = "ltrviewitem"
                ltritem.EnableViewState = False
                container.Controls.Add(ltritem)
        End Select
    End Sub
End Class
