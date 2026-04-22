using System;
using System.Collections.Generic;

/// <summary>
/// Search User Control 
/// Handler Basic Search and Advanced Search 
/// 
/// Remark : Based on previous Version and modified the way of the binding
/// ----------------------------------------
/// C.C.Yeon    25 APril 2011  Modified 
/// </summary>
public partial class Search : System.Web.UI.UserControl
{
    private List<string> _Search;
    private List<string> query;

    public event Action AdditionalSearch;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.BindDDL();
        }

        Control.Base setting = (Control.Base)this.Page;

        if (Session["key"] == null)
        {
            Session["key"] = setting.SetupKey;
        }
        else
        {
            if ((string)Session["key"] != setting.SetupKey)
            {
                Session["key"] = setting.SetupKey;
                Session["Type"] = null;
                flush();
            }
        }

        if (Session["Type"] == null)
        {
            ChangeType("B");
        }
        else
        {
            ChangeType((string)Session["Type"]);
            _Search = (List<string>)Session["Search"];
            query = (List<string>)Session["Query"];
        }

        if (!IsPostBack)
        {
            if ((string)Session["Type"] == "B")
            {
                if (setting.SearchField.Trim() != string.Empty)
                {
                    txtSearch.Text = setting.SearchValue;
                    ddlSearch.SelectedValue = setting.SearchField;
                }
            }
            else
            {
                BindCreteria((List<string>)Session["Search"]);
            }
        }

        btnReset.Enabled = searchCriteria.Items.Count > 0;
        btnMinus.Enabled = searchCriteria.Items.Count > 0;
    }

    protected void BindDDL()
    {
        Control.Base setting = (Control.Base)this.Page;

        //here to disappear the delete check box
        this.chkDeleted.Visible = setting.DeleteControl;

        if (setting.DeleteControl)
        {
            this.chkDeleted.Visible = setting.ShowDeletedControl;
            this.chkDeleted.Checked = setting.ShowDeleted;
        }

        if (setting.SetupKey == string.Empty)
        {
            this.Visible = false;
        }
        else
        {
            Control.Binding.BindDropDownListResource(this.ddlSearch, setting.SetupKey, "-", "-");
            Control.Binding.BindDropDownListResource(this.ddlSearchUsing, setting.SetupKey, "-", "-");
        }
    }

    private void ChangeType(string Type)
    {
        Session["Type"] = Type;

        if (Type == "A")
        {
            lblSearch.Text = "Advance Search";
            pnlBasic.Visible = false;
            pnlAdvance.Visible = true;
        }
        else if (Type == "B")
        {
            lblSearch.Text = "Basic Search";
            pnlBasic.Visible = true;
            pnlAdvance.Visible = false;
        }
    }

    private void detector()
    {
        if (_Search.Count == 0)
        {
            ddlSearch2.Enabled = false;
        }
        else
        {
            ddlSearch2.Enabled = true;
        }
    }

    private void resetController()
    {
        ddlSearch.SelectedIndex = 0;
        ddlSearchUsing.SelectedIndex = 0;
        ddlOperator1.SelectedIndex = 0;
        ddlSearch2.SelectedIndex = 0;
        txtSearch.Text = string.Empty;
        txtSearchUsing.Text = string.Empty;
    }

    private void flush()
    {
        _Search = new List<string>();
        query = new List<string>();
        resetController();
        searchCriteria.Items.Clear();
        Session["Search"] = _Search;
        Session["Query"] = query;
    }

    /// <summary>
    /// Retrieve the delete value from the checkedbox
    /// </summary>
    public bool Deleted
    {
        get { return chkDeleted.Checked; }
    }

    public string SearchType
    {
        get { return (string)Session["Type"]; }
    }

    protected void lbAdvSearch_Click(object sender, EventArgs e)
    {
        flush();
        ChangeType("A");
    }

    protected void lbBasicSearch_Click(object sender, EventArgs e)
    {
        flush();
        ChangeType("B");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Control.Base setting = (Control.Base)this.Page;

        if (ddlSearch.SelectedIndex > 0)
        {
            setting.SearchField = ddlSearch.SelectedValue;
            setting.SearchValue = txtSearch.Text;
        }
        else
        {
            setting.SearchField = string.Empty;
            setting.SearchValue = string.Empty;
        }

        if (AdditionalSearch != null) AdditionalSearch();

        setting.PageNo = 1;
        Response.Redirect(setting.GenerateList);
    }

    protected void btnSubmit2_Click(object sender, EventArgs e)
    {
        Control.Base setting = (Control.Base)this.Page;
        setting.SearchField = string.Empty;
        setting.SearchValue = ConvertionString(query);

        if (AdditionalSearch != null) AdditionalSearch();

        setting.PageNo = 1;
        Response.Redirect(setting.GenerateList);
    }

    protected void btnPlus_Click(object sender, EventArgs e)
    {
        if (CheckSelection(ddlSearchUsing.SelectedValue, "Search Field") == false)
        {
            return;
        }

        if (CheckSelection(ddlOperator1.SelectedValue, "Operator") == false)
        {
            return;
        }

        if (_Search.Count > 0)
        {
            if (CheckSelection(ddlSearch2.SelectedValue, "Operator (And/Or)") == false)
            {
                return;
            }
        }

        if (EmptyCheck(txtSearchUsing.Text, "Search Value") == false)
        {
            return;
        }

        _Search.Add((_Search.Count > 0 ? ddlSearch2.SelectedItem.Text.Replace("-", "") + " " : string.Empty) + ddlSearchUsing.SelectedItem.Text + " " + ddlOperator1.SelectedItem.Text + " " + txtSearchUsing.Text);
        AddQuery((_Search.Count > 0 ? ddlSearch2.SelectedItem.Text.Replace("-", "") + " " : "Add "), ddlSearchUsing.SelectedValue, ddlOperator1.SelectedValue, txtSearchUsing.Text);

        Session["Search"] = _Search;
        Session["Query"] = query;

        BindCreteria(_Search);
        detector();
        resetController();
        ddlSearchUsing.Focus();
    }

    private void AddQuery(string Addtional, string SearchField, string Operator, string SearchValue)
    {
        if (Operator.Trim().ToUpper() == "LIKE")
        {
            SearchValue = "'%" + SearchValue + "%'";
        }
        else
        {
            SearchValue = "'" + SearchValue + "'";
        }

        query.Add(Addtional + " UPPER(" + SearchField + ") " + Operator + " UPPER(" + SearchValue + ")");
    }

    private bool CheckSelection(string Value, string field)
    {
        if (Value == "-")
        {
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, field + " was " + Resources.Message.InvalidSelect);
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool EmptyCheck(string Value, string Field)
    {
        if (Value.Trim() == string.Empty)
        {
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, string.Format(Resources.Message.FieldEmpty, Field));
            return false;
        }
        else
        {
            return true;
        }
    }

    private string ConvertionString(List<string> list)
    {
        var temp = new System.Text.StringBuilder();
        foreach (string Str in list)
        {
            temp.AppendLine(Str);
        }

        return temp.ToString();
    }

    private void BindCreteria(List<string> list)
    {
        btnReset.Enabled = list.Count > 0;
        btnMinus.Enabled = list.Count > 0;

        searchCriteria.DataSource = list;
        searchCriteria.DataBind();
    }

    protected void btnMinus_Click(object sender, EventArgs e)
    {
        if (_Search.Count == 0)
        {
            return;
        }

        if (searchCriteria.SelectedIndex == -1)
        {
            if (_Search.Count == 1)
            {
                _Search = new List<string>();
                query = new List<string>();
            }
            else
            {
                _Search.RemoveAt(_Search.Count - 1);
                query.RemoveAt(_Search.Count - 1);
            }
        }
        else
        {
            _Search.RemoveAt(searchCriteria.SelectedIndex);
            query.RemoveAt(searchCriteria.SelectedIndex);
        }

        if (_Search.Count > 0)
        {
            if (_Search[0].Substring(0, 3) == "AND")
            {
                _Search[0] = _Search[0].Substring(3, _Search[0].Length - 3);
            }
            else if (_Search[0].Substring(0, 2) == "OR")
            {
                _Search[0] = _Search[0].Substring(2, _Search[0].Length - 2);
            }
        }

        if (query.Count > 0)
        {
            if (query[0].Substring(0, 3) == "AND")
            {
                query[0] = query[0].Substring(3, query[0].Length - 3);
            }
            else if (query[0].Substring(0, 2) == "OR")
            {
                query[0] = query[0].Substring(2, query[0].Length - 2);
            }

            query[0] = "AND" + query[0];
        }

        Session["Search"] = _Search;
        Session["Query"] = query;

        BindCreteria(_Search);
        detector();
        resetController();
        ddlSearchUsing.Focus();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        _Search = new List<string>();
        query = new List<string>();
        Session["Search"] = _Search;
        Session["Query"] = query;

        BindCreteria(_Search);
        detector();
        resetController();
    }

    protected void chkDeleted_CheckedChanged(object sender, EventArgs e)
    {
        Control.Base setting = (Control.Base)this.Page;
        setting.ShowDeleted = chkDeleted.Checked;
        Response.Redirect(setting.GenerateList);
    }
}
