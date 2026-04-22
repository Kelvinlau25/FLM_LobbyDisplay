Imports System.Web.UI.WebControls

Public Class historyfield
    Implements System.Web.UI.ITemplate

    Public Const LabelHeaderID As String = "lblhis"
    Public Const LiteralItemID As String = "ltrhisitem"
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
                lbdel.Text = "Audit Trail"
                lbdel.ID = "lblhis"
                lbdel.EnableViewState = False
                container.Controls.Add(lbdel)
            Case listItemType.Item
                ltritem = New Literal()
                ltritem.ID = "ltrhisitem"
                ltritem.EnableViewState = False
                container.Controls.Add(ltritem)
        End Select
    End Sub
End Class
