Imports System.Reflection
Imports System.Resources
Imports System.Globalization
Imports System.IO
Imports System.Drawing.Drawing2D
Imports System.Drawing
Imports System.Drawing.Imaging

Namespace Control
    ''' <summary>
    ''' Retrieve the value from the resource page
    ''' -----------------------------------------
    ''' C.C.Yeon    25 April 2011   initial Version
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Resource
        Public Shared Function RetrieveValue(ByVal Resource As String, ByVal Field As String) As String
            Dim mng As New ResourceManager(Resource, Assembly.GetExecutingAssembly)
            RetrieveValue = mng.GetString(Field)
        End Function

        '    Public Shared Function Print_Barcode(ByVal barcode As String) As Integer
        '        If barcode.Length > 0 Then
        '            Dim myconnection As New Oracle.DataAccess.Client.OracleConnection(ConfigurationManager.ConnectionStrings("ORCL_GI").ConnectionString)
        '            Dim sqlquery As String = ""
        '            Dim myCommand As Oracle.DataAccess.Client.OracleCommand
        '            Dim dll As BarcodeLib.Barcode = New BarcodeLib.Barcode(barcode.ToString, BarcodeLib.TYPE.CODE39)
        '            Dim _barcode As Drawing.Image
        '            dll.Encode(BarcodeLib.TYPE.CODE39, barcode.ToString, 100)
        '            _barcode = dll.EncodedImage
        '            Dim image_data1 As Byte() = Nothing
        '            Using ms As MemoryStream = New MemoryStream
        '                _barcode.Save(ms, ImageFormat.Png)
        '                image_data1 = ms.ToArray
        '            End Using
        '            Using myconnection
        '                myconnection.Open()
        '                sqlquery = "DELETE From TEMP_BARCODE where TRIM(VALUE) = '" & barcode.Trim & "' or CREATED_DATE < to_date('" & DateTime.Now.AddMinutes(-5).ToString("yyyy-MM-dd HH:mm") & "', 'yy-MM-dd hh24:mi') "
        '                myCommand = New Oracle.DataAccess.Client.OracleCommand(sqlquery, myconnection)
        '                myCommand.CommandText = sqlquery
        '                myCommand.ExecuteNonQuery()
        '                myCommand.Dispose()
        '                sqlquery = "INSERT INTO TEMP_BARCODE(VALUE, BARCODE, CREATED_DATE, CREATED_USER) VALUES " _
        '                & "('" & barcode & "', :BlobParameter1, sysdate, 'TEST')"
        '                myCommand = New Oracle.DataAccess.Client.OracleCommand(sqlquery, myconnection)
        '                myCommand.CommandText = sqlquery
        '                myCommand.Parameters.Clear()
        '                myCommand.Parameters.Add("BlobParameter1", image_data1)
        '                myCommand.ExecuteNonQuery()
        '                myCommand.Dispose()
        '                myconnection.Close()
        '            End Using
        '            image_data1 = Nothing
        '        End If
        '    End Function

    End Class




End Namespace

