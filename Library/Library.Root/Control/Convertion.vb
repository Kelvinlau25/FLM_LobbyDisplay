Imports System.Text.Json

Namespace Control
    ''' <summary>
    ''' JSON serialization helpers (net8.0 migration — JavaScriptSerializer replaced with System.Text.Json)
    ''' </summary>
    Public Class Convertion(Of T)

        ''' <summary>
        ''' Convert List of T into String Format
        ''' </summary>
        Public Shared Function Serializer(ByVal list As List(Of T)) As String
            Return JsonSerializer.Serialize(list)
        End Function

        ''' <summary>
        ''' Convert string into List of T
        ''' </summary>
        Public Shared Function Deserializer(ByVal StringFormat As String) As List(Of T)
            Return JsonSerializer.Deserialize(Of List(Of T))(StringFormat)
        End Function
    End Class
End Namespace
