using System;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class App_Module_Controller : System.Web.UI.UserControl
{
    public enum DisplayType
    {
        Full = 3,
        Half = 2,
        Name = 1,
        ID = 0
    }

    #region "Audit"
    private string _createdcompanycode = string.Empty;
    public string CreatedCompanyCode
    {
        get { return _createdcompanycode; }
        set { _createdcompanycode = value; }
    }

    private string _createdby = string.Empty;
    public string CreatedBy
    {
        get { return _createdby; }
        set { _createdby = value; }
    }

    private DateTime _createdDate = DateTime.Now;
    public DateTime CreatedDate
    {
        get { return _createdDate; }
        set { _createdDate = value; }
    }

    private string _createdLoc = string.Empty;
    public string CreatedLoc
    {
        get { return _createdLoc; }
        set { _createdLoc = value; }
    }

    private string _UpdatedCompanyCode = string.Empty;
    public string UpdatedCompanyCode
    {
        get { return _UpdatedCompanyCode; }
        set { _UpdatedCompanyCode = value; }
    }

    private string _UpdatedBy = string.Empty;
    public string UpdatedBy
    {
        get { return _UpdatedBy; }
        set { _UpdatedBy = value; }
    }

    private DateTime _UpdatedDate = DateTime.Now;
    public DateTime UpdatedDate
    {
        get { return _UpdatedDate; }
        set { _UpdatedDate = value; }
    }

    private string _UpdatedLoc = string.Empty;
    public string UpdatedLoc
    {
        get { return _UpdatedLoc; }
        set { _UpdatedLoc = value; }
    }
    #endregion

    private string _connectionstring = "ORCL_ACL";
    public string connectionstring
    {
        get { return _connectionstring; }
        set { _connectionstring = value; }
    }

    private string _datetimeformat = "dd / MMM / yyyy hh:mm:ss";
    public string DateTimeFormat
    {
        get { return _datetimeformat; }
        set { _datetimeformat = value; }
    }

    /// <summary>
    /// Full - Display Company Code + Employee Name + ID
    /// Half - Display Employee Name + ID
    /// Name - Display Employee Name
    /// ID - Display ID
    /// </summary>
    private DisplayType _AuditTrailDisplayType = DisplayType.ID;
    public DisplayType AuditTrailDisplayType
    {
        get { return _AuditTrailDisplayType; }
        set { _AuditTrailDisplayType = value; }
    }

    /// <summary>
    /// Control the ability the button
    /// </summary>
    private bool _editMode = true;
    public bool EditMode
    {
        get { return _editMode; }
        set { _editMode = value; }
    }

    private string _validationGroup;
    public string ValidationGroup
    {
        get { return _validationGroup; }
        set { _validationGroup = value; }
    }

    private bool _add = true;
    public bool Add
    {
        get { return _add; }
        set { _add = value; }
    }

    private bool _edit = true;
    public bool Edit
    {
        get { return _edit; }
        set { _edit = value; }
    }

    private bool _delete = true;
    public bool Delete
    {
        get { return _delete; }
        set { _delete = value; }
    }

    private bool _History = true;
    public bool History
    {
        get { return _History; }
        set { _History = value; }
    }

    private string _Listkey = string.Empty;
    public string ListKey
    {
        get { return _Listkey; }
        set { _Listkey = value; }
    }

    public event Action AddAction;
    public event Action EditAction;
    public event Action DeleteAction;
    public event Action AddResetAction;
    public event Action EditResetAction;
    public event Action ViewEditAction;
    public event Action ModifyMode;
    public event Action DisplayMode;
    public event Action CancelAction; // Add By Elven, Handler custom event

    protected void Page_Init(object sender, EventArgs e)
    {
        CancelAction += OnCancelAction;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Control.Base setting = (Control.Base)this.Page;

        hpLink.NavigateUrl = setting.GetUrl(Control.Base.EnumAction.History, this.ListKey);

        switch (setting.Action)
        {
            case Control.Base.EnumAction.Add:
                btnDelete.Visible = false;
                hpLink.Visible = false;
                pnconfirmation.Visible = false;
                pninfo.Visible = false;

                if (ModifyMode != null) ModifyMode();

                if (Add == false)
                {
                    Response.Redirect(setting.GetUrl(Control.Base.EnumAction.None));
                }
                break;
            case Control.Base.EnumAction.Delete:
                hpLink.Visible = false;
                btnReset.Visible = false;
                btnDelete.Visible = false;

                if (DisplayMode != null) DisplayMode();

                if (Delete == false)
                {
                    Response.Redirect(setting.GetUrl(Control.Base.EnumAction.None));
                }
                break;
            case Control.Base.EnumAction.Edit:
                hpLink.Visible = false;
                btnDelete.Visible = false;
                pnconfirmation.Visible = false;

                if (ModifyMode != null) ModifyMode();

                if (Edit == false)
                {
                    Response.Redirect(setting.GetUrl(Control.Base.EnumAction.None));
                }
                break;
            case Control.Base.EnumAction.View:
                btnSubmit.Text = "Edit";
                btnReset.Visible = false;
                btnSubmit.CausesValidation = false;
                btnDelete.Visible = Delete;
                pnconfirmation.Visible = false;

                if (DisplayMode != null) DisplayMode();
                break;
        }

        if (this.ValidationGroup != string.Empty)
        {
            btnSubmit.ValidationGroup = ValidationGroup;
            cvdeleteyes.ValidationGroup = ValidationGroup;
            cvdeleteyes.ErrorMessage = Resources.Message.Deletemessage;
        }

        if (pninfo.Visible)
        {
            ACL.Object.User _createdtemp = null;
            ACL.Object.User _updatedtemp = null;

            if (!IsPostBack)
            {
                if (AuditTrailDisplayType == DisplayType.Full || AuditTrailDisplayType == DisplayType.Half || AuditTrailDisplayType == DisplayType.Name)
                {
                    if (this.CreatedBy == string.Empty)
                    {
                        throw new Exception("Please set value into properties created by");
                    }

                    if (this.UpdatedBy == string.Empty)
                    {
                        throw new Exception("Please set value into properties updated by");
                    }
                }

                //Assigning Data
                string _createdtext = string.Empty;
                string _updatedtext = string.Empty;

                if (AuditTrailDisplayType != DisplayType.Name)
                {
                    _createdtext = GenerateText(_createdtext, " ID : " + this.CreatedBy);
                    _updatedtext = GenerateText(_updatedtext, " ID : " + this.UpdatedBy);
                }

                if (AuditTrailDisplayType == DisplayType.Full || AuditTrailDisplayType == DisplayType.Half || AuditTrailDisplayType == DisplayType.Name)
                {
                    if (_createdtemp != null)
                    {
                        if (_createdtemp.EmployeeName != string.Empty)
                        {
                            _createdtext = GenerateText(_createdtext, " Name : " + _createdtemp.EmployeeName);
                        }
                    }

                    if (_updatedtemp != null)
                    {
                        if (_updatedtemp.EmployeeName != string.Empty)
                        {
                            _updatedtext = GenerateText(_updatedtext, " Name : " + _updatedtemp.EmployeeName);
                        }
                    }
                }

                lblcreatedby.Text = _createdtext;
                lblcreateddate.Text = this.CreatedDate.ToString(this.DateTimeFormat);
                lblcreatedloc.Text = this.CreatedLoc;

                lblupdatedby.Text = _updatedtext;
                lblupdateddate.Text = this.UpdatedDate.ToString(this.DateTimeFormat);
                lblUpdatedloc.Text = this.UpdatedLoc;
            }
        }

        if (btnSubmit.Text != "Edit")
        {
            btnSubmit.OnClientClick = "if(!confirm('Confirm submit the form?')){ return false; } else{showLoading();};";
        }
    }

    private string GenerateText(string Value, string AddText)
    {
        if (Value.Length > 0)
        {
            return Value + " - " + AddText;
        }
        else
        {
            return AddText;
        }
    }

    // Add by Elven, handler existing event, if no override by page
    public virtual void OnCancelAction()
    {
        Control.Base setting = (Control.Base)this.Page;
        Response.Redirect(setting.GetUrl(Control.Base.EnumAction.None));
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (CancelAction != null) CancelAction();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (DeleteAction != null) DeleteAction();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Control.Base setting = (Control.Base)this.Page;
        switch (setting.Action)
        {
            case Control.Base.EnumAction.Delete:
                if (DeleteAction != null) DeleteAction();
                break;
            case Control.Base.EnumAction.Add:
                if (AddAction != null) AddAction();
                break;
            case Control.Base.EnumAction.Edit:
                if (EditAction != null) EditAction();
                break;
            case Control.Base.EnumAction.View:
                if (ViewEditAction != null) ViewEditAction();
                Response.Redirect(setting.GetUrl(Control.Base.EnumAction.Edit));
                break;
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Control.Base setting = (Control.Base)this.Page;
        switch (setting.Action)
        {
            case Control.Base.EnumAction.Add:
                if (AddResetAction != null) AddResetAction();
                break;
            case Control.Base.EnumAction.Edit:
                if (EditResetAction != null) EditResetAction();
                break;
        }
    }

    protected void cvdeleteyes_ServerValidate(object source, ServerValidateEventArgs args)
    {
        args.IsValid = rbyes.Checked;
    }
}
