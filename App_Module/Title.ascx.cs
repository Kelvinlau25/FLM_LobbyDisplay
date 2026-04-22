using System;

/// <summary>
/// Display Title 
/// ---------------------------------------
/// 21 May 2012    Yeon  Add Title properties
/// </summary>
public partial class App_Module_Title : System.Web.UI.UserControl
{
    private bool _audit = false;
    public bool Audit
    {
        set { _audit = value; }
    }

    private string _title = string.Empty;
    /// <summary>
    /// this will be priority
    /// </summary>
    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Title == string.Empty)
        {
            if (_audit == false)
            {
                Control.Base setting = (Control.Base)this.Page;
                this.lblFormTitle.Text = setting.DisplayTitle + (setting.Action != Control.Base.EnumAction.None ? " - " : string.Empty) + setting.ActionDesc;
            }
            else
            {
                Control.LogBase setting = (Control.LogBase)this.Page;
                this.lblFormTitle.Text = setting.DisplayTitle;
            }
        }
        else
        {
            if (_audit == false)
            {
                Control.Base setting = (Control.Base)this.Page;
                this.lblFormTitle.Text = this.Title + (setting.Action != Control.Base.EnumAction.None ? " - " : string.Empty) + setting.ActionDesc;
            }
            else
            {
                this.lblFormTitle.Text = this.Title;
            }
        }

        this.Page.Title = lblFormTitle.Text;
    }
}
