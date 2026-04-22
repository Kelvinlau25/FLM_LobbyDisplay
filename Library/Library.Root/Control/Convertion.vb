Imports System.Web.Script.Serialization

Namespace Control
    Public Class Convertion(Of T)
        Private Shared _ser As JavaScriptSerializer

        ''' <summary>
        ''' Convert List of T into String Format
        ''' </summary>
        ''' <param name="list"></param>
        Public Shared Function Serializer(ByVal list As List(Of T)) As String
            _ser = New JavaScriptSerializer
            Serializer = _ser.Serialize(list)
            _ser = Nothing
        End Function

        ''' <summary>
        ''' Convert string into List of T
        ''' </summary>
        ''' <param name="StringFormat"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Deserializer(ByVal StringFormat As String) As List(Of T)
            _ser = New JavaScriptSerializer
            Deserializer = _ser.Deserialize(Of List(Of T))(StringFormat)
            _ser = Nothing
        End Function
    End Class
End Namespace

