using System;

public partial class Acc_TVDisplay_Mst : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Cbutton.Visible = true;
        }
    }

    private void navigate(string ID)
    {
        string script = "";
        if (ID == "Disp1")
        {
            script = "window.open('pantry_mainDisplay.aspx', '_blank');";
        }
        else if (ID == "Disp2")
        {
            script = "window.open('pantry_2ndDisplay.aspx', '_blank');";
        }
        Page.ClientScript.RegisterStartupScript(this.GetType(), "alertscript", script, true);
    }

    #region "ButtonClick"
    protected void BtDisp1_Click(object sender, EventArgs e)
    {
        navigate("Disp1");
    }

    protected void BtnDisp2_Click(object sender, EventArgs e)
    {
        navigate("Disp2");
    }
    #endregion
}
