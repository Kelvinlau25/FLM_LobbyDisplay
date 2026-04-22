using System;

public partial class master_Main : System.Web.UI.MasterPage
{
    private bool _pointer = false;

    protected void Page_Init(object sender, EventArgs e)
    {
        Session["gstrUserID"] = "11";
        Session["gstrUserCompCode"] = "11";
        Session["gstrUserWorksNo"] = "11";
    }
}
