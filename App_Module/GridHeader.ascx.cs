using System;
using System.Web.UI;

/// <summary>
/// Add User Control
/// 
/// Additional
/// ----------------------------------------
/// if the URL Doest not Containt the Sort Direction and Sort Field then will generate and redirect to default value
/// 
/// Remark : Based on previous version and modified the way of the binding
/// ----------------------------------------
/// C.C.Yeon    25 APril 2011  Modified 
/// C.C.Yeon    21 May 2012    Add Extraparameter 
/// </summary>
public partial class UserControl_GridHeader : System.Web.UI.UserControl
{
    private string _extraparameter = string.Empty;
    /// <summary>
    /// For Extra Parameter during User Click on the add item
    /// </summary>
    public string ExtraParameter
    {
        get { return _extraparameter; }
        set { _extraparameter = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.BindHyperLink();
        }

        Control.Base _page = (Control.Base)this.Page;
        hypAdd.Visible = _page.AddControl;
        ddlAction.Visible = _page.PrintControl;
    }

    protected void BindHyperLink()
    {
        ddlAction.Visible = false;
        Control.Base setting = (Control.Base)this.Page;
        string addurl = ResolveUrl(setting.GetUrl(Control.Base.EnumAction.Add));
        if (!string.IsNullOrEmpty(addurl))
        {
            if (this.ExtraParameter != string.Empty)
            {
                hypAdd.HRef = addurl + (addurl.IndexOf("?") != -1 ? "&" : "?") + "itm=" + this.ExtraParameter;
            }
            else
            {
                hypAdd.HRef = addurl;
            }
        }
        else
        {
            hypAdd.Visible = false;
        }
    }

    protected void ddlAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.ddlAction.SelectedValue == "PRINT")
        {
            Control.Base setting = (Control.Base)this.Page;
            ddlAction.SelectedIndex = 0;

            if (setting.Item1 == string.Empty)
            {
                raiseNoRecordSelectedMsg();
                return;
            }

            string strScript = "popwindow('" + setting.GeneratePrintPage() + "');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Print", strScript, true);
        }
    }

    public void raiseNoRecordSelectedMsg()
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), "NoRecordFound", "alert('No selected records to print');", true);
    }
}
