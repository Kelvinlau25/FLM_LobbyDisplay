using System;

public partial class UserControl_GridFooter : System.Web.UI.UserControl
{
    private Control.Base setting;
    private Control.LogBase logSetting;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.BindPage();
            this.setNavigator();
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        this.BindPage();
        this.setNavigator();
    }

    private bool _audit = false;
    public bool Audit
    {
        set { _audit = value; }
    }

    private int _total = 0;
    public int TotalRecords
    {
        get { return _total; }
        set
        {
            _total = value;
            this._totalPage = (int)Math.Ceiling((double)_total / Library.Root.Other.BusinessLogicBase.MaxQuantityPerPage);
        }
    }

    private int _totalPage = 0;
    public int TotalPage
    {
        get { return (int)Math.Ceiling((double)_total / Library.Root.Other.BusinessLogicBase.MaxQuantityPerPage); }
    }

    protected void ddlPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_audit)
        {
            logSetting = (Control.LogBase)this.Page;
            logSetting.PageNo = int.Parse(ddlPage.SelectedItem.Text);
            Response.Redirect(logSetting.GenerateList);
        }
        else
        {
            setting = (Control.Base)this.Page;
            setting.PageNo = int.Parse(ddlPage.SelectedItem.Text);
            Response.Redirect(setting.GenerateList);
        }
    }

    protected void btnNext_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        if (_audit)
        {
            logSetting = (Control.LogBase)this.Page;

            if (ddlPage.Items.Count > 0)
            {
                if (logSetting.PageNo >= int.Parse(ddlPage.Items[ddlPage.Items.Count - 1].Value))
                {
                    return;
                }
            }
            else
            {
                return;
            }

            logSetting.PageNo = logSetting.PageNo + 1;
            Response.Redirect(logSetting.GenerateList);
        }
        else
        {
            setting = (Control.Base)this.Page;

            if (ddlPage.Items.Count > 0)
            {
                if (setting.PageNo >= int.Parse(ddlPage.Items[ddlPage.Items.Count - 1].Value))
                {
                    return;
                }
            }
            else
            {
                return;
            }

            setting.PageNo = setting.PageNo + 1;
            Response.Redirect(setting.GenerateList);
        }
    }

    protected void btnPrevious_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        if (_audit)
        {
            logSetting = (Control.LogBase)this.Page;
            if (logSetting.PageNo <= 0)
            {
                return;
            }

            logSetting.PageNo = logSetting.PageNo - 1;
            Response.Redirect(logSetting.GenerateList);
        }
        else
        {
            setting = (Control.Base)this.Page;
            if (setting.PageNo <= 0)
            {
                return;
            }

            setting.PageNo = setting.PageNo - 1;
            Response.Redirect(setting.GenerateList);
        }
    }

    protected void btnLast_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        if (_audit)
        {
            logSetting = (Control.LogBase)this.Page;
            logSetting.PageNo = int.Parse(ddlPage.Items[ddlPage.Items.Count - 1].Value);
            Response.Redirect(logSetting.GenerateList);
        }
        else
        {
            setting = (Control.Base)this.Page;
            setting.PageNo = int.Parse(ddlPage.Items[ddlPage.Items.Count - 1].Value);
            Response.Redirect(setting.GenerateList);
        }
    }

    protected void btnFirst_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        if (_audit)
        {
            logSetting = (Control.LogBase)this.Page;
            logSetting.PageNo = 1;
            Response.Redirect(logSetting.GenerateList);
        }
        else
        {
            setting = (Control.Base)this.Page;
            setting.PageNo = 1;
            Response.Redirect(setting.GenerateList);
        }
    }

    private void BindPage()
    {
        if (_audit)
        {
            logSetting = (Control.LogBase)this.Page;
            if (logSetting.PageNo > this._totalPage)
            {
                logSetting.PageNo = this._totalPage;
            }
        }
        else
        {
            setting = (Control.Base)this.Page;
            if (setting.PageNo > this._totalPage)
            {
                setting.PageNo = this._totalPage;
            }
        }

        ddlPage.Items.Clear();
        for (int i = 1; i <= this._totalPage; i++)
        {
            ddlPage.Items.Add(i.ToString());
        }

        lblTotalRecord.Text = string.Format("Total of {0} records found", this.TotalRecords);
    }

    protected void setNavigator()
    {
        if (_audit)
        {
            logSetting = (Control.LogBase)this.Page;
        }
        else
        {
            setting = (Control.Base)this.Page;
        }


        if (this._total <= 1)
        {
            this.btnFirst.Visible = false;
            this.btnLast.Visible = false;
            this.btnNext.Visible = false;
            this.btnPrevious.Visible = false;
            return;
        }
        else
        {
            this.btnFirst.Visible = true;
            this.btnLast.Visible = true;
            this.btnNext.Visible = true;
            this.btnPrevious.Visible = true;
        }

        this.ddlPage.SelectedIndex = (_audit ? logSetting.PageNo - 1 : setting.PageNo - 1);
        if (ddlPage.SelectedIndex == 0)
        {
            this.btnFirst.Visible = false;
            this.btnPrevious.Visible = false;
        }
        else
        {
            this.btnFirst.Visible = true;
            this.btnPrevious.Visible = true;
        }

        int iPage = this.TotalPage;
        if (ddlPage.SelectedIndex >= iPage - 1)
        {
            this.btnLast.Visible = false;
            this.btnNext.Visible = false;
        }
        else
        {
            this.btnLast.Visible = true;
            this.btnNext.Visible = true;
        }
    }
}
