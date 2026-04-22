using System;

public partial class App_Module_ErrorReport : System.Web.UI.UserControl
{
    private string _validationGroup = "";
    public string ValidationGroup
    {
        get { return _validationGroup; }
        set { _validationGroup = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        vsSummary.ValidationGroup = this.ValidationGroup;
    }
}
