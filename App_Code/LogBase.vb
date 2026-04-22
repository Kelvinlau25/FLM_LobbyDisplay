Namespace Control
    Public MustInherit Class LogBase
        Inherits Library.Root.Control.LogBase

        Public ReadOnly Property LogTable() As String
            Get
                Return GetGlobalResourceObject("Log", MyBase.SetupKey)
            End Get
        End Property

        Public Overrides ReadOnly Property LogPage() As String
            Get
                Return GetGlobalResourceObject("ListPage", "History")
            End Get
        End Property

        Public Overrides ReadOnly Property LogTitle() As String
            Get
                Return GetGlobalResourceObject("Title", MyBase.SetupKey)
            End Get
        End Property

        Public ReadOnly Property SortDesc() As String
            Get
                Try
                    Return GetGlobalResourceObject("SortDesc", MyBase.SetupKey)
                Catch ex As Exception
                    Return String.Empty
                End Try
            End Get
        End Property

        Protected Overrides Sub BindData()

        End Sub

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)
        End Sub
    End Class
End Namespace
