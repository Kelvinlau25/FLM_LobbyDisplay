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
        RootDirectory_Path =  Server.MapPath("~/image_upload/");//ConfigurationManager.AppSettings["FILESERVER_PATH_KL"].ToString();
        RootDirectory_Url = ConfigurationManager.AppSettings["FILESERVER_URL_KL"].ToString();
        //if (Session["FILESERVER_PATH_KL"] != null)
        //{
        //    RootDirectory_Path = Session["FILESERVER_PATH_KL"].ToString().Trim();
        //}
        //if (Session["FILESERVER_URL_KL"] != null)
        //{
        //    RootDirectory_Url = Session["FILESERVER_URL_KL"].ToString().Trim();
        //}
        FileServerTransfer.ConfirmDirectory(RootDirectory_Path);



        //AuthenticateFileManager();
    }



    public void AuthenticateFileManager()
    {
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

        if (FileUpload1.HasFile)
        {
            spanerror.InnerHtml = "";

            string FilePath = Server.MapPath("~/image_upload/") + FileUpload1.FileName.ToString(); //MapServerPhysicalPath(txtPath.Text.Replace("//", "/") + FileUpload1.FileName.ToString());
            FilePath = HttpUtility.UrlDecode(FilePath);
            if (System.IO.File.Exists(FilePath))
            {
                spanerror.InnerHtml = FileUpload1.FileName.ToString() + " File Exists ";
            }
            else
            {
                FileUpload1.SaveAs(FilePath);
               // spanerror.InnerHtml = "http://10.200.1.90:528/image_upload/" + FileUpload1.FileName.ToString();
            }
        }
        else { spanerror.InnerHtml = "Please Upload File"; }

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