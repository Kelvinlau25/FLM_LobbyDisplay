using System;
using Microsoft.VisualBasic;

public partial class acc_PopUp_ImgVideoPreview : System.Web.UI.Page
{
    public static string paths;

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Request.QueryString["id"].ToString().Length > 5)
        {
            string ext = Strings.Mid(Request.QueryString["id"].ToString(), Request.QueryString["id"].ToString().Length - 3);

            ext = Strings.Mid(ext.ToLower(), 1, ext.ToLower().Length - 1);

            string file = Strings.Mid(Request.QueryString["id"].ToString(), 2);

            file = Strings.Mid(file, 1, file.Length - 1);

            if (ext.Equals("mp4"))
            {
                string[] i = Request.Url.ToString().Split('/');
                paths = i[0] + "//" + i[2];

                if (Request.QueryString["scr"].ToString() == "1")
                {
                    if (Request.IsLocal)
                    {
                        paths = paths + "/LobbyDisplay/acc/LobbyDisplay/mainscr/" + file;
                    }
                    else
                    {
                        paths = paths + "/acc/PantryDisplay/mainscr/" + file;
                    }
                }
                else if (Request.QueryString["scr"].ToString() == "2")
                {
                    if (Request.IsLocal)
                    {
                        paths = paths + "/LobbyDisplay/acc/LobbyDisplay/secscrtop/" + file;
                    }
                    else
                    {
                        paths = paths + "/acc/PantryDisplay/secscrtop/" + file;
                    }
                }
                else
                {
                    if (Request.IsLocal)
                    {
                        paths = paths + "/LobbyDisplay/acc/LobbyDisplay/secscrbtm/" + file;
                    }
                    else
                    {
                        paths = paths + "/acc/PantryDisplay/secscrbtm/" + file;
                    }
                }
            }
        }
        else
        {
            return;
        }
    }
}
