Imports System.Data
Imports System.IO
Imports System.Web.UI

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
        Using _sw As New StringWriter
            Using _htw As New HtmlTextWriter(_sw)
                Dim _dg As New System.Web.UI.WebControls.DataGrid
                Dim _counter As Integer = -1

                ' Field setting
                For Each Itm As FieldSet In Me._setting
                    _counter += 1
                    Me._data.Columns(Itm.Field).SetOrdinal(_counter)
                    Me._data.Columns(Itm.Field).AllowDBNull = True
                    Me._data.Columns(Itm.Field).ColumnName = Itm.Title
                Next

                ' Remove unwanted column
                _counter += 1
                Dim _totalColumnCount As Int16 = Me.Data.Columns.Count - 1
                For temp As Integer = _counter To _totalColumnCount
                    Me.Data.Columns.RemoveAt(_counter)
                Next

                _dg.DataSource = Me.Data
                _dg.DataBind()
                _dg.RenderControl(_htw)

                Return _sw.ToString
            End Using
        End Using
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



