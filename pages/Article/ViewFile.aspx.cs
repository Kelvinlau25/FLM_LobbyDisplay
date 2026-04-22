using System;
using System.Configuration;
using System.Web;

public partial class Pages_Article_PlayVideo : Control.BaseForm
{
    public bool _IsPreviousLink = false;
    public bool _IsValid = false;
    public string _V = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["p"] != null)
        {
            _IsPreviousLink = Request.QueryString["p"].Equals("1");
        }

        if (Request.QueryString["v"] != null)
        {
            _V = HttpUtility.UrlDecode(Request.QueryString["v"].ToString().Trim()).Replace(ConfigurationManager.AppSettings.Get("FILESERVER_KEY").ToString(), ConfigurationManager.AppSettings.Get("FILESERVER_URL").ToString());
            string ext = _V.Substring(_V.LastIndexOf("."));
            if (!ext.ToUpper().Equals(".PDF"))
            {
                _V = "";
            }

            if (_V.Length == 0)
            {
                litJava.Text = "alert('Invalid File Format, accept PDF only.'); window.close();";
            }
            else
            {
                _IsValid = true;
            }
        }
        else
        {
            litJava.Text = "alert('File Not Found.'); window.close();";
        }
    }
}
