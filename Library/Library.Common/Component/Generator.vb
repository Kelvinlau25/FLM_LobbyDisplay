Imports System.Data
Imports System.IO
Imports System.Text

Public Class Generator
    Implements ICollection(Of FieldSet)

#Region "Properties"
    Private _data As DataTable
    ''' <summary>
    ''' Data Source
    ''' </summary>
    Public Property Data() As DataTable
        Get
            Return _data
        End Get
        Set(ByVal value As DataTable)
            _data = value
        End Set
    End Property

    Private _setting As New List(Of FieldSet)
    ''' <summary>
    ''' Field Setting Collection
    ''' </summary>
    Public ReadOnly Property Setting() As List(Of FieldSet)
        Get
            Return _setting
        End Get
    End Property
#End Region

    ''' <summary>
    ''' initial the data
    ''' </summary>
    Public Sub New(ByVal data As DataTable)
        Me._data = data
        Me._setting = New List(Of FieldSet)
    End Sub

    Public Sub AddField(ByVal Field As String, ByVal Title As String, ByVal Type As EnumLib.DataType)
        Me._setting.Add(New FieldSet(Field, Title, Type))
    End Sub

    Public Sub Add(ByVal item As FieldSet) Implements System.Collections.Generic.ICollection(Of FieldSet).Add
        Me._setting.Add(item)
    End Sub

    ''' <summary>
    ''' Generate the HTML
    ''' </summary>
    Public Overrides Function ToString() As String
        Dim sb As New System.Text.StringBuilder()
        sb.AppendLine("<table>")
        ' Header row
        sb.AppendLine("<tr>")
        For Each itm As FieldSet In Me._setting
            sb.AppendLine($"<th>{System.Web.HttpUtility.HtmlEncode(itm.Title)}</th>")
        Next
        sb.AppendLine("</tr>")
        ' Data rows
        For Each row As System.Data.DataRow In Me._data.Rows
            sb.AppendLine("<tr>")
            For Each itm As FieldSet In Me._setting
                Dim cellValue As String = If(row(itm.Field) Is DBNull.Value, String.Empty, row(itm.Field).ToString())
                sb.AppendLine($"<td>{System.Web.HttpUtility.HtmlEncode(cellValue)}</td>")
            Next
            sb.AppendLine("</tr>")
        Next
        sb.AppendLine("</table>")
        Return sb.ToString()
    End Function

    Public Sub Clear() Implements System.Collections.Generic.ICollection(Of FieldSet).Clear
        Me._setting.Clear()
    End Sub

    Public Function Contains(ByVal item As FieldSet) As Boolean Implements System.Collections.Generic.ICollection(Of FieldSet).Contains
        Return Me._setting.Contains(item)
    End Function

    Public Sub CopyTo(ByVal array() As FieldSet, ByVal arrayIndex As Integer) Implements System.Collections.Generic.ICollection(Of FieldSet).CopyTo
        Me._setting.CopyTo(array, arrayIndex)
    End Sub

    Public ReadOnly Property Count() As Integer Implements System.Collections.Generic.ICollection(Of FieldSet).Count
        Get
            Return Me._setting.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly() As Boolean Implements System.Collections.Generic.ICollection(Of FieldSet).IsReadOnly
        Get
            Return False
        End Get
    End Property

    Public Function Remove(ByVal item As FieldSet) As Boolean Implements System.Collections.Generic.ICollection(Of FieldSet).Remove
        Return Me._setting.Remove(item)
    End Function

    Public Function IndexOf(ByVal item As FieldSet) As Integer
        Return Me._setting.IndexOf(item)
    End Function

    Public Sub Insert(ByVal index As Integer, ByVal item As FieldSet)
        Me._setting.Insert(index, item)
    End Sub

    Default Public Property Item(ByVal index As Integer) As FieldSet
        Get
            Return Me._setting.Item(index)
        End Get
        Set(ByVal value As FieldSet)
            Me._setting.Item(index) = value
        End Set
    End Property

    Public Sub RemoveAt(ByVal index As Integer)
        Me._setting.RemoveAt(index)
    End Sub

    Public Function GetEnumerator() As System.Collections.Generic.IEnumerator(Of FieldSet) Implements System.Collections.Generic.IEnumerable(Of FieldSet).GetEnumerator
        Return Me._setting.GetEnumerator
    End Function

    Public Function GetEnumerator1() As System.Collections.IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
        Return Me._setting.GetEnumerator
    End Function
End Class
