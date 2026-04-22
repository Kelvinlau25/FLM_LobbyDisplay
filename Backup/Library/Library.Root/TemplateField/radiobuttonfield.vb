Imports System.Web.UI.WebControls

Public Class radiobuttonfield
    Implements System.Web.UI.ITemplate

    Public Const RadioButtonItemID As String = "rbitem"
    Private plittype As ListItemType

    Public Sub New(ByVal type As ListItemType)
        MyBase.New()
        Me.plittype = type
    End Sub

    Public Sub InstantiateIn(ByVal container As System.Web.UI.Control) Implements System.Web.UI.ITemplate.InstantiateIn
        Dim rbitem As RadioButton

        Select Case Me.plittype
            Case ListItemType.Header
                Return
            Case ListItemType.Footer
                Return
            Case listItemType.Item
                rbitem = New RadioButton()
                rbitem.ID = "rbitem"
                rbitem.AutoPostBack = True
                container.Controls.Add(rbitem)
        End Select
    End Sub
End Class

