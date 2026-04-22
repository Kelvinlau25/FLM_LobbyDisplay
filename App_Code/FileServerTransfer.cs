using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.UI.WebControls;

public class FileServerTransfer
{
    private FtpWebRequest ftpRequest = null;
    private Stream ftpStream = null;
    private int bufferSize = 2048;

    public string TransferFile(string filename, byte[] fileContent)
    {
        string strMsg = string.Empty;
        try
        {
            // Create an FTP Request 
            string uploadUrl = string.Format("{0}/{1}/{2}", "ftp://address", "foldername", filename);
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(uploadUrl));
            // Log in to the FTP Server with the User Name and Password Provided 
            ftpRequest.Credentials = new NetworkCredential("Username", "Password");
            // When in doubt, use these options 
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            // Specify the Type of FTP Request 
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

            ftpStream = ftpRequest.GetRequestStream();

            // Upload the File by Sending the Buffered Data Until the Transfer is Complete 
            try
            {
                ftpRequest.ContentLength = fileContent.Length;

                Stream requestStream = ftpRequest.GetRequestStream();
                requestStream.Write(fileContent, 0, fileContent.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();

                strMsg = "File Upload Status: " + response.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            // Resource Cleanup 
            ftpStream.Close();
            ftpRequest = null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
        return strMsg;
    }

    public static bool Save(FileUpload pUploadControl, string pDestDirectory, string pDestFileName, ref string pFilePath)
    {
        bool result = false;

        //// Make Sure File Selection Exist
        if (pUploadControl.FileName.Trim() == "")
        {
            return false;
        }

        string cReplaceKey = ConfigurationManager.AppSettings["FILESERVER_KEY"].ToString();
        string cFilePath = ConfigurationManager.AppSettings["FILESERVER_PATH"].ToString();
        string cFileUrl = ConfigurationManager.AppSettings["FILESERVER_URL"].ToString();

        string fUploadFormat = pUploadControl.FileName.Substring(pUploadControl.FileName.LastIndexOf("."));

        //// Store Old File Directory
        string fOldFile = pFilePath;

        //// New Path
        pFilePath = pDestDirectory + pDestFileName;

        //// Check File Same Location
        if (fOldFile != pFilePath)
        {
            fOldFile = fOldFile.Replace(cReplaceKey, cFilePath).Replace("/", "\\");
        }
        else
        {
            fOldFile = "";
        }

        try
        {
            //// Directory Exists Verification
            if (!Directory.Exists(pDestDirectory))
            {
                Directory.CreateDirectory(pDestDirectory);
            }

            //// File Exists Verification
            if (File.Exists(pFilePath))
            {
                File.Delete(pFilePath);
            }

            //// Upload file to Path
            pUploadControl.SaveAs(pFilePath);

            //// Move existing to Archive
            FileServerTransfer.Archive(fOldFile);

            //// Path Map Replace
            if (pFilePath.IndexOf(cFilePath) == 1)
            {
                pFilePath = pFilePath.Replace(cFilePath, cReplaceKey);
            }

            result = true;
        }
        catch (Exception)
        {
            pFilePath = "";
            result = false;
        }
        return result;
    }

    public static bool Save(FileUpload UploadControl, string DestDirectory, string DestFileName)
    {
        bool result = false;

        try
        {
            //// Directory Exists Verification
            if (!Directory.Exists(HttpUtility.UrlDecode(DestDirectory)))
            {
                Directory.CreateDirectory(HttpUtility.UrlDecode(DestDirectory));
            }

            //// File Exists Verification
            if (File.Exists(HttpUtility.UrlDecode(DestDirectory + DestFileName)))
            {
                File.Delete(HttpUtility.UrlDecode(DestDirectory + DestFileName));
            }

            //// Upload file to Path
            UploadControl.SaveAs(HttpUtility.UrlDecode(DestDirectory + DestFileName));

            result = true;
        }
        catch (Exception)
        {
            result = false;
        }
        return result;
    }

    public static bool Save(HttpPostedFile UploadControl, string DestDirectory, string DestFileName)
    {
        bool result = false;

        try
        {
            //// Directory Exists Verification
            if (!Directory.Exists(HttpUtility.UrlDecode(DestDirectory)))
            {
                Directory.CreateDirectory(HttpUtility.UrlDecode(DestDirectory));
            }

            //// File Exists Verification
            if (File.Exists(HttpUtility.UrlDecode(DestDirectory + DestFileName)))
            {
                File.Delete(HttpUtility.UrlDecode(DestDirectory + DestFileName));
            }

            //// Upload file to Path
            UploadControl.SaveAs(HttpUtility.UrlDecode(DestDirectory + DestFileName));

            result = true;
        }
        catch (Exception)
        {
            result = false;
        }
        return result;
    }

    public static string Save2(HttpPostedFile UploadControl, string DestDirectory, string DestFileName)
    {
        string result = "False";

        try
        {
            //// Directory Exists Verification
            if (!Directory.Exists(HttpUtility.UrlDecode(DestDirectory)))
            {
                Directory.CreateDirectory(HttpUtility.UrlDecode(DestDirectory));
            }

            //// File Exists Verification
            if (File.Exists(HttpUtility.UrlDecode(DestDirectory + DestFileName)))
            {
                File.Delete(HttpUtility.UrlDecode(DestDirectory + DestFileName));
            }

            //// Upload file to Path
            UploadControl.SaveAs(HttpUtility.UrlDecode(DestDirectory + DestFileName));

            result = "True";
        }
        catch (Exception ex)
        {
            result = ex.ToString();
        }
        return result;
    }

    public static bool Remove(string Location)
    {
        bool result = false;

        string OriFile = HttpUtility.UrlDecode(Location).Replace("/", "\\");
        if (!File.Exists(OriFile))
        {
            return false;
        }

        try
        {
            //// File Exists Verification
            if (File.Exists(OriFile))
            {
                File.Delete(OriFile);
            }

            result = true;
        }
        catch (Exception)
        {
            result = false;
        }
        return result;
    }

    public static bool Archive(string Location)
    {
        bool result = false;

        string OriFile = HttpUtility.UrlDecode(Location).Replace("/", "\\");
        if (!File.Exists(OriFile))
        {
            return false;
        }

        try
        {
            string OldDirectory = "";
            string NewDirectory = "";

            //// Network Path or Local Path
            if (OriFile.IndexOf("\\\\") == 0 || OriFile.IndexOf(":\\") <= 3)
            {
                OldDirectory = OriFile.Substring(0, OriFile.LastIndexOf("\\") + 1);
                NewDirectory = OldDirectory + "Archive\\";
            }
            else //// Other Type Path
            {
                return false;
            }

            //// Directory Exists Verification
            if (!Directory.Exists(NewDirectory))
            {
                Directory.CreateDirectory(NewDirectory);
            }

            File.Move(OriFile, OriFile.Replace(OldDirectory, NewDirectory));

            result = true;
        }
        catch (Exception)
        {
            result = false;
        }
        return result;
    }

    public static bool ConfirmDirectory(string Location)
    {
        bool result = false;

        string OriFolder = HttpUtility.UrlDecode(Location).Replace("/", "\\");
        if (OriFolder.Length < 2)
        {
            return false;
        }

        try
        {
            //// Directory Exists Verification
            if (!Directory.Exists(OriFolder))
            {
                Directory.CreateDirectory(OriFolder);
            }

            result = true;
        }
        catch (Exception)
        {
            result = false;
        }
        return result;
    }
}
