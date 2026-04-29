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
        RootDirectory_Path = ConfigurationManager.AppSettings["FILESERVER_ENBPATH"].ToString();
        RootDirectory_Url = ConfigurationManager.AppSettings["FILESERVER_ENBURL"].ToString();
        if (Session["FILESERVER_ENBPATH"] != null && Session["FILESERVER_ENBURL"] != null)
        {
            RootDirectory_Path = Session["FILESERVER_ENBPATH"].ToString().Trim();
            RootDirectory_Url = Session["FILESERVER_ENBURL"].ToString().Trim();
        }

        if (!FileServerTransfer.ConfirmDirectory(RootDirectory_Path))
        {
            Response.Clear();
            Response.Write("Access Denied");
            Response.End();
        }


        AuthenticateFileManager();
        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetNoStore();
    }



    // chnage this funcation if you want to create your Authenticate 
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
}