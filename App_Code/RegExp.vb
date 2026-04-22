Imports Microsoft.VisualBasic

Public Class RegExp

    Public Shared Function AlphaNumericValidation(ByVal IsMandatory As Boolean, ByVal MaxLength As Integer) As String
        Return "^[a-zA-Z0-9]{" & IIf(IsMandatory, "1", "0") & "," & MaxLength.ToString & "}$"
    End Function

    Public Shared Function EmailValidation(ByVal IsMandatory As Boolean, ByVal MaxLength As Integer) As String
        Return "^[a-zA-Z0-9 /.,\\\-_@]{" & IIf(IsMandatory, "1", "0") & "," & MaxLength.ToString & "}$"
    End Function

    Public Shared Function TextValidation(ByVal IsMandatory As Boolean, ByVal MaxLength As Integer) As String
        Return "^[a-zA-Z0-9 &%$#!*()+=\[\]:;<>?/.,\\\-_@']{" & IIf(IsMandatory, "1", "0") & "," & MaxLength.ToString & "}$"
    End Function

End Class
