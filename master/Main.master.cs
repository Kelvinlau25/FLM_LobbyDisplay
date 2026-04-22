using System;

public partial class master_Main : System.Web.UI.MasterPage
{
    private bool _pointer = false;

    protected void Page_Init(object sender, EventArgs e)
    {
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString[ACL.Control.URL.URLEMPLOYEEID] != null)
        {
            Session["gstrUserID"] = ACL.Security.Encryption.Decrypt(Request.QueryString[ACL.Control.URL.URLEMPLOYEEID]);
            _pointer = true;
        }

        if (Request.QueryString[ACL.Control.URL.URLCOMPANYID] != null)
        {
            Session["gstrUserCompCode"] = Request.QueryString[ACL.Control.URL.URLCOMPANYID];
            _pointer = true;
        }

        if (Session["gstrUserID"] == null)
        {
            if (Request.UserAgent.ToLower().Contains("ipad"))
            {
                Response.Redirect("~/SessionExpired.aspx?ID=1");
            }
            Response.Redirect("~/SessionExpired.aspx?ID=2");
        }
    }
}
