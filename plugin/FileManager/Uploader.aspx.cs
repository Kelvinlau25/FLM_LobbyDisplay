using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using System.Configuration;
public partial class FileManager_Uploader : System.Web.UI.Page
{
    public String RootDirectory_Path = "";
    public String RootDirectory_Url = "";



    protected void Page_Load(object sender, EventArgs e)
    {
        // Customize
        RootDirectory_Path = ConfigurationManager.AppSettings["FILESERVER_PATH"].ToString();
        RootDirectory_Url = ConfigurationManager.AppSettings["FILESERVER_URL"].ToString();
        
        if (Session["FILESERVER_PATH"] != null && Session["FILESERVER_URL"] != null)
        {
            RootDirectory_Path = Session["FILESERVER_PATH"].ToString().Trim();
            RootDirectory_Url = Session["FILESERVER_URL"].ToString().Trim();
        }

        if (!FileServerTransfer.ConfirmDirectory(RootDirectory_Path))
        {
            Response.Clear();
            Response.Write("Access Denied");
            Response.End();
        }



        AuthenticateFileManager();
    }



    public void AuthenticateFileManager()
    {
        if (Request["sessionid"] == null)
        {
            Response.Clear();
            Response.Write("Access Denied");
            Response.End();
            return;
        }

        /* Edit this funcation to  AuthenticateFileManager*/
        string SessionID = Request["sessionid"].ToString();

        if (Request.Cookies[SessionID] != null)
        {

        }
        else
        {
            Response.Clear();
            Response.Write("Access Denied");
            Response.End();
        }

    }

    protected void bntUpload_Click(object sender, EventArgs e)
    {
        spanerror.InnerHtml = "";

        string FilePath = MapServerPhysicalPath(txtPath.Text.Replace("//", "/") + FileUpload1.FileName.ToString());
        FilePath = HttpUtility.UrlDecode(FilePath);
        if (System.IO.File.Exists(FilePath))
        {
            spanerror.InnerHtml = FileUpload1.FileName.ToString() + " File Exists ";
        }
        else
        {
            FileUpload1.SaveAs(FilePath);
        }


    }


    private String MapServerPhysicalPath(String filepath)
    {
        String FullPath = "";

        if (Request.PhysicalApplicationPath.LastIndexOf('\\') + 1 == Request.PhysicalApplicationPath.Length)
            FullPath = RootDirectory_Path;
            //FullPath = Request.PhysicalApplicationPath.Substring(0, Request.PhysicalApplicationPath.LastIndexOf('\\'));

        filepath = filepath.Replace(RootDirectory_Url, "");
        filepath = filepath.Replace("/", "\\");
        //filepath = filepath.Replace("\\IPCMS\\", "\\");

        if (filepath.IndexOf("\\") == 0)
            filepath = filepath.Substring(1);

        return FullPath + filepath;
    }


}