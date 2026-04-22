Imports Microsoft.VisualBasic
Imports System.Net
Imports System.IO

Public Class FileServerTransfer
    Private ftpRequest As FtpWebRequest = Nothing
    Private ftpStream As Stream = Nothing
    Private bufferSize As Integer = 2048


    '' ''byte[] byteBuffer = new byte[bufferSize];
    '' ''byte[] Data = fileArray.FileBytes;
    '' ''TransferFile(fileArray.PostedFile.FileName, Data));




    Public Function TransferFile(ByVal filename As String, ByVal fileContent As Byte()) As String
        Dim strMsg As String = String.Empty
        Try

            ' Create an FTP Request 

            Dim uploadUrl As [String] = [String].Format("{0}/{1}/{2}", "ftp://address", "foldername", filename)
            ftpRequest = DirectCast(FtpWebRequest.Create(New Uri(uploadUrl)), FtpWebRequest)
            ' Log in to the FTP Server with the User Name and Password Provided 

            ftpRequest.Credentials = New NetworkCredential("Username", "Password")
            ' When in doubt, use these options 

            ftpRequest.UseBinary = True
            ftpRequest.UsePassive = True
            ftpRequest.KeepAlive = True
            ' Specify the Type of FTP Request 

            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile

            ftpStream = ftpRequest.GetRequestStream()


            ' Upload the File by Sending the Buffered Data Until the Transfer is Complete 

            Try

                ftpRequest.ContentLength = fileContent.Length

                Dim requestStream As Stream = ftpRequest.GetRequestStream()
                requestStream.Write(fileContent, 0, fileContent.Length)
                requestStream.Close()

                Dim response As FtpWebResponse = DirectCast(ftpRequest.GetResponse(), FtpWebResponse)

                strMsg = "File Upload Status: " + response.ToString()
            Catch ex As Exception
                Console.WriteLine(ex.ToString())
            End Try
            ' Resource Cleanup 

            ftpStream.Close()
            ftpRequest = Nothing
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try
        Return strMsg
    End Function

    Public Shared Function Save(ByVal pUploadControl As FileUpload, ByVal pDestDirectory As String, ByVal pDestFileName As String, ByRef pFilePath As String) As Boolean
        Save = False

        '' Make Sure File Selection Exist
        If pUploadControl.FileName.Trim = "" Then
            Return False
        End If

        Dim cReplaceKey As String = ConfigurationManager.AppSettings("FILESERVER_KEY").ToString
        Dim cFilePath As String = ConfigurationManager.AppSettings("FILESERVER_PATH").ToString
        Dim cFileUrl As String = ConfigurationManager.AppSettings("FILESERVER_URL").ToString

        Dim fUploadFormat As String = pUploadControl.FileName.Substring(pUploadControl.FileName.LastIndexOf("."))

        '' Store Old File Directory
        Dim fOldFile As String = pFilePath

        '' New Path
        pFilePath = pDestDirectory & pDestFileName

        '' Check File Same Location
        If fOldFile <> pFilePath Then
            fOldFile = fOldFile.Replace(cReplaceKey, cFilePath).Replace("/", "\")
        Else
            fOldFile = ""
        End If

        Try
            '' Directory Exists Verification
            If Not Directory.Exists(pDestDirectory) Then
                Directory.CreateDirectory(pDestDirectory)
            End If

            '' File Exists Verification
            If File.Exists(pFilePath) Then
                File.Delete(pFilePath)
            End If

            '' Upload file to Path
            pUploadControl.SaveAs(pFilePath)

            '' Move existing to Archive
            FileServerTransfer.Archive(fOldFile)


            '' Path Map Replace
            If pFilePath.IndexOf(cFilePath) = 1 Then
                pFilePath = pFilePath.Replace(cFilePath, cReplaceKey)
            End If


            Save = True
        Catch ex As Exception
            pFilePath = ""
            Save = False
        End Try
    End Function

    Public Shared Function Save(ByVal UploadControl As FileUpload, ByVal DestDirectory As String, ByVal DestFileName As String) As Boolean
        Save = False

        Try
            '' Directory Exists Verification
            If Not Directory.Exists(HttpUtility.UrlDecode(DestDirectory)) Then
                Directory.CreateDirectory(HttpUtility.UrlDecode(DestDirectory))
            End If

            '' File Exists Verification
            If File.Exists(HttpUtility.UrlDecode(DestDirectory & DestFileName)) Then
                File.Delete(HttpUtility.UrlDecode(DestDirectory & DestFileName))
            End If

            '' Upload file to Path
            UploadControl.SaveAs(HttpUtility.UrlDecode(DestDirectory & DestFileName))

            Save = True
        Catch ex As Exception

            Save = False
        End Try
    End Function

    Public Shared Function Save(ByVal UploadControl As HttpPostedFile, ByVal DestDirectory As String, ByVal DestFileName As String) As Boolean
        Save = False

        Try
            '' Directory Exists Verification
            If Not Directory.Exists(HttpUtility.UrlDecode(DestDirectory)) Then
                Directory.CreateDirectory(HttpUtility.UrlDecode(DestDirectory))
            End If

            '' File Exists Verification
            If File.Exists(HttpUtility.UrlDecode(DestDirectory & DestFileName)) Then
                File.Delete(HttpUtility.UrlDecode(DestDirectory & DestFileName))
            End If

            '' Upload file to Path
            UploadControl.SaveAs(HttpUtility.UrlDecode(DestDirectory & DestFileName))

            Save = True
        Catch ex As Exception
            Save = False
        End Try
    End Function

    Public Shared Function Save2(ByVal UploadControl As HttpPostedFile, ByVal DestDirectory As String, ByVal DestFileName As String) As String
        Save2 = False

        Try
            '' Directory Exists Verification
            If Not Directory.Exists(HttpUtility.UrlDecode(DestDirectory)) Then
                Directory.CreateDirectory(HttpUtility.UrlDecode(DestDirectory))
            End If

            '' File Exists Verification
            If File.Exists(HttpUtility.UrlDecode(DestDirectory & DestFileName)) Then
                File.Delete(HttpUtility.UrlDecode(DestDirectory & DestFileName))
            End If

            '' Upload file to Path
            UploadControl.SaveAs(HttpUtility.UrlDecode(DestDirectory & DestFileName))

            Save2 = True
        Catch ex As Exception
            Save2 = ex.ToString
        End Try
    End Function

    Public Shared Function Remove(ByVal Location As String) As Boolean
        Remove = False

        Dim OriFile As String = HttpUtility.UrlDecode(Location).Replace("/", "\")
        If Not File.Exists(OriFile) Then
            Return False
        End If

        Try
            '' File Exists Verification
            If File.Exists(OriFile) Then
                File.Delete(OriFile)
            End If

            Remove = True
        Catch ex As Exception
            Remove = False
        End Try
    End Function

    Public Shared Function Archive(ByVal Location As String) As Boolean
        Archive = False

        Dim OriFile As String = HttpUtility.UrlDecode(Location).Replace("/", "\")
        If Not File.Exists(OriFile) Then
            Return False
        End If

        Try
            Dim OldDirectory As String = ""
            Dim NewDirectory As String = ""

            '' Network Path or Local Path
            If (OriFile.IndexOf("\\") = 0 Or OriFile.IndexOf(":\") <= 3) Then
                OldDirectory = OriFile.Substring(0, OriFile.LastIndexOf("\") + 1)
                NewDirectory = OldDirectory & "Archive\"
            Else '' Other Type Path
                Return False
            End If

            '' Directory Exists Verification
            If Not Directory.Exists(NewDirectory) Then
                Directory.CreateDirectory(NewDirectory)
            End If

            File.Move(OriFile, OriFile.Replace(OldDirectory, NewDirectory))

            Archive = True
        Catch ex As Exception
            Archive = False
        End Try
    End Function

    Public Shared Function ConfirmDirectory(ByVal Location As String) As Boolean
        ConfirmDirectory = False

        Dim OriFolder As String = HttpUtility.UrlDecode(Location).Replace("/", "\")
        If OriFolder.Length < 2 Then
            Return False
        End If

        Try
            '' Directory Exists Verification
            If Not Directory.Exists(OriFolder) Then
                Directory.CreateDirectory(OriFolder)
            End If

            ConfirmDirectory = True
        Catch ex As Exception
            ConfirmDirectory = False
        End Try
    End Function


End Class
