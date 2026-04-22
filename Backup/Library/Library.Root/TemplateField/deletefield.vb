Imports System.Web.UI.WebControls

Public Class deletefield
    Implements System.Web.UI.ITemplate

    Public Const LabelHeaderID As String = "lbldel"
    Public Const LiteralItemID As String = "ltritem"
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
                lbdel.Text = "Del."
                lbdel.ID = "lbldel"
                lbdel.EnableViewState = False
                container.Controls.Add(lbdel)
            Case ListItemType.Item
                ltritem = New Literal()
                ltritem.ID = "ltritem"
                ltritem.EnableViewState = False
                container.Controls.Add(ltritem)
        End Select
    End Sub

End Class
