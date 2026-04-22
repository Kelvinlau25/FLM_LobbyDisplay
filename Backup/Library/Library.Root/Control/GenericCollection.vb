Imports System.Collections.Generic

Namespace Other
    ''' <summary>
    ''' Generic Collection
    ''' ----------------------------------------------
    ''' C.C.Yeon      25 April 2011    initial version
    ''' </summary>
    ''' <typeparam name="T">Object</typeparam>
    ''' <remarks></remarks>
    Public Class GenericCollection(Of T)

        Public Sub New()
            _data = New List(Of T)
            _counter = 0
        End Sub

        Private _data As List(Of T)
        Public Property Data() As List(Of T)
            Get
                Return _data
            End Get
            Set(ByVal value As List(Of T))
                _data = value
            End Set
        End Property

        Private _counter As Integer
        Public Property TotalRow() As Integer
            Get
                Return _counter
            End Get
            Set(ByVal value As Integer)
                _counter = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' Generic Total Collection handler when the data was need total Calculation
    ''' -------------------------------------------------------------------------
    ''' C.C.Yeon      19 May 2011    initial version
    ''' </summary>
    ''' <typeparam name="T">Object</typeparam>
    ''' <remarks></remarks>
    Public Class TotalCollection(Of T)
        Public Sub New()
            _data = New List(Of T)
            _counter = New List(Of T)
        End Sub

        Private _data As List(Of T)
        Public Property Data() As List(Of T)
            Get
                Return _data
            End Get
            Set(ByVal value As List(Of T))
                _data = value
            End Set
        End Property

        Private _counter As List(Of T)
        Public Property TotalData() As List(Of T)
            Get
                Return _counter
            End Get
            Set(ByVal value As List(Of T))
                _counter = value
            End Set
        End Property
    End Class
End Namespace

