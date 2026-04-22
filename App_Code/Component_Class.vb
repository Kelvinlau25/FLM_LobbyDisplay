Imports Microsoft.VisualBasic
Imports System.Globalization

Public Class Component_Class
    ''' <summary>
    ''' Rectify_Date
    ''' </summary>
    ''' <param name="value">pass in date/datetime</param>
    ''' <param name="format">optional to convert date format by default 'dd/MM/yyyy'</param>
    ''' <param name="spliter_format">pass in string to split day month year</param>
    ''' <param name="type">optional by default date only if pass in other value than '' will get datetime</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Rectify_Date(ByVal value As String, Optional ByVal format As String = "dd/MM/yyyy", Optional ByVal spliter_format As String = "/", Optional ByVal type As String = "") As String
        Dim d As String = value

        Try
            If type.Equals("") Then
                If HttpContext.Current.Request.IsLocal Then
                    d = Mid(value, 1, 2) & spliter_format & Mid(value, 4, 2) & spliter_format & Mid(value, 7, 4)
                Else
                    d = Mid(value, 4, 2) & spliter_format & Mid(value, 1, 2) & spliter_format & Mid(value, 7, 4)
                End If
            Else
                If HttpContext.Current.Request.IsLocal Then
                    d = Mid(value, 1, 2) & spliter_format & Mid(value, 4, 2) & spliter_format & Mid(value, 7)
                Else
                    d = Mid(value, 4, 2) & spliter_format & Mid(value, 1, 2) & spliter_format & Mid(value, 7)
                End If
            End If

            If format <> "dd/MM/yyyy" Then
                d = CDate(d).ToString(format)
            End If

        Catch ex As Exception
            d = value
        End Try

        Return d
    End Function

    Public Shared Function fn_convertMth(ByVal mth As String) As String
        Dim convertMth As String = Nothing
        If mth = "JAN" Then
            convertMth = "01"
        ElseIf mth = "FEB" Then
            convertMth = "02"
        ElseIf mth = "MAR" Then
            convertMth = "03"
        ElseIf mth = "APR" Then
            convertMth = "04"
        ElseIf mth = "MAY" Then
            convertMth = "05"
        ElseIf mth = "JUN" Then
            convertMth = "06"
        ElseIf mth = "JUL" Then
            convertMth = "07"
        ElseIf mth = "AUG" Then
            convertMth = "08"
        ElseIf mth = "SEP" Then
            convertMth = "09"
        ElseIf mth = "OCT" Then
            convertMth = "10"
        ElseIf mth = "NOV" Then
            convertMth = "11"
        ElseIf mth = "DEC" Then
            convertMth = "12"
        End If

        fn_convertMth = convertMth
    End Function

    'Public Shared Function ConvertMonth_Date(ByVal value As String, Optional ByVal month As String = "MMM", Optional ByVal Spliter As String = "-") As String
    '    Dim d As String = value

    '    Try
    '        If month.Equals("MMM") Then
    '            d = Mid(value, 1, 2) & Spliter & CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(Mid(value, 4, 2)) & Spliter & Mid(value, 7)
    '        ElseIf month.Equals("MMMM") Then
    '            d = Mid(value, 1, 2) & Spliter & CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Mid(value, 4, 2)) & Spliter & Mid(value, 7)
    '        ElseIf month.Equals("MM") Then
    '            d = Mid(value, 1, 2) & Spliter & Mid(value, 4, 2) & Spliter & Mid(value, 7, 4)
    '        End If
    '    Catch ex As Exception
    '        d = value
    '    End Try

    '    Return d
    'End Function

End Class
