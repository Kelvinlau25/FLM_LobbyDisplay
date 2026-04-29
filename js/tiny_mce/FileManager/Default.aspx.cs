using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Controls_ACMSTextBox_JavaScript_tiny_mce_plugins_FileManager_Default : System.Web.UI.Page
{
    public String RootDirectory_Path = "";
    public String RootDirectory_Url = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        // Customize
        RootDirectory_Path = Server.MapPath("~/image_upload/");//ConfigurationManager.AppSettings["FILESERVER_PATH_KL"].ToString();
        RootDirectory_Url = ConfigurationManager.AppSettings["FILESERVER_URL_KL"].ToString();
        //if (Session["FILESERVER_PATH_KL"] != null)
        //{
            //RootDirectory_Path = Session["FILESERVER_PATH_KL"].ToString().Trim();
        //}
        //else
        //{
        //    Response.Clear();
        //    Response.Write("Access Denied");
        //    Response.End();
        //}
        //if (Session["FILESERVER_URL_KL"] != null)
        //{
            //RootDirectory_Url = Session["FILESERVER_URL_KL"].ToString().Trim();
        //}
        //else
        //{
        //    Response.Clear();
        //    Response.Write("Access Denied");
        //    Response.End();
        //}
        FileServerTransfer.ConfirmDirectory(RootDirectory_Path);



        //AuthenticateFileManager();
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
    }



    // chnage this funcation if you want to create your Authenticate 
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
}