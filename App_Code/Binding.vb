Imports Library.Root.Object
Imports System.Collections.Generic

Namespace Control
    ''' <summary>
    ''' Component Binding 
    ''' ----------------------------------------------
    ''' C.C.Yeon    16 April 2012   initial version
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Binding
        Public Shared Sub BindDropDownListResource(ByVal DDL As System.Web.UI.WebControls.DropDownList, ByVal ResourceName As String, Optional ByVal Text As String = "", Optional ByVal Value As String = "")
            DDL.DataSource = Library.Root.Control.Convertion(Of Binder).Deserializer(HttpContext.GetGlobalResourceObject("SearchSource", ResourceName))
            DDL.DataTextField = "Text"
            DDL.DataValueField = "Value"
            DDL.DataBind()
            AddList(DDL, Text, Value)
        End Sub

        Public Shared Sub BindDropDownList(ByVal DDL As System.Web.UI.WebControls.DropDownList, ByVal list As List(Of Binder), Optional ByVal Text As String = "", Optional ByVal Value As String = "")
            If list.Count > 0 Then
                DDL.DataSource = list
                DDL.DataTextField = "Text"
                DDL.DataValueField = "Value"
                DDL.DataBind()
            End If
            AddList(DDL, Text, Value)
        End Sub

        Private Shared Sub AddList(ByVal ddl As System.Web.UI.WebControls.DropDownList, ByVal Text As String, ByVal Value As String)
            If Value <> String.Empty Then
                ddl.Items.Insert(0, New System.Web.UI.WebControls.ListItem(Text, Value))
            End If
        End Sub
    End Class
End Namespace

