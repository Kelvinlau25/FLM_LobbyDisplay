Namespace Control
    Public Interface IPageContext
        Sub Redirect(ByVal url As String)
        Function ResolveUrl(ByVal relativeUrl As String) As String
        Function UrlDecode(ByVal value As String) As String
        Function UrlEncode(ByVal value As String) As String
    End Interface
End Namespace
