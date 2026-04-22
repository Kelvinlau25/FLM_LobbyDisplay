Imports System.Web.UI.WebControls

Public Class checkboxfield
    Implements System.Web.UI.ITemplate

    Public Const CheckboxHeaderID As String = "chkall"
    Public Const CheckboxItemID As String = "ckitem"
    Private plittype As ListItemType

    Public Sub New(ByVal type As ListItemType)
        MyBase.New()
        Me.plittype = type
    End Sub

    Public Sub InstantiateIn(ByVal container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn
        Dim ckitem As CheckBox
        Dim ckall As CheckBox

        Select Case Me.plittype
            Case listItemType.Header
                ckall = New CheckBox()
                ckall.ID = "chkall"
                ckall.AutoPostBack = True
                container.Controls.Add(ckall)
            Case listItemType.Footer
                Return
            Case listItemType.Item
                ckitem = New CheckBox()
                ckitem.ID = "ckitem"
                ckitem.AutoPostBack = True
                container.Controls.Add(ckitem)
        End Select
    End Sub
End Class

