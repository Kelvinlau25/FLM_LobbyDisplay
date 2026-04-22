Imports System.Data
Imports Oracle.ManagedDataAccess.Client

Public MustInherit Class Connection
    Implements IDisposable

    Protected _con As OracleConnection
    Protected _cmd As OracleCommand
    Protected _rdr As OracleDataReader
    Protected _tran As OracleTransaction

    Private _constr As String = String.Empty
    Public Property ConnectionString() As String
        Get
            Return _constr
        End Get
        Set(ByVal value As String)
            _constr = value
        End Set
    End Property

    Public Sub New(ByVal ConnectionStringName As String)
        Me.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings(ConnectionStringName).ToString

        If ConnectionString = String.Empty Then
            Throw New Exception("Invalid Connection String Name That Set At Web Config")
        End If

        Me._con = New OracleConnection(Me.ConnectionString)
        Me._con.Open()
        Me._cmd = _con.CreateCommand
        Me._tran = _con.BeginTransaction
    End Sub

    Public ReadOnly Property Status() As String
        Get
            If _con IsNot Nothing Then
                Return _con.State.ToString
            Else
                Return String.Empty
            End If
        End Get
    End Property

    ''' <summary>
    ''' Commit all the transaction
    ''' </summary>
    Public Sub Commit()
        _tran.Commit()
    End Sub

    ''' <summary>
    ''' Rollback all the transaction
    ''' </summary>
    Public Sub RollBack()
        _tran.Rollback()
    End Sub

    Private disposedValue As Boolean = False        ' To detect redundant calls
    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: free other state (managed objects).
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.

            If _rdr IsNot Nothing Then
                _rdr.Dispose()
            End If

            If _tran IsNot Nothing Then
                _tran.Dispose()
            End If

            If _cmd IsNot Nothing Then
                _cmd.Dispose()
            End If

            If _con IsNot Nothing Then
                If _con.State = ConnectionState.Open Then
                    _con.Close()
                End If

                _con.Dispose()
            End If
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
