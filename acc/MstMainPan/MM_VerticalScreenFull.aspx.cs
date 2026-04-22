using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic;

public partial class acc_MstMain_MM_VerticalScreenFull : System.Web.UI.Page
{
    int clear = 0;
    private string str_MSSQL_Connstr = ConfigurationManager.ConnectionStrings["filmDisplay"].ConnectionString;

    public bool db_connSQLSel(string sql, DataTable dtDBTable)
    {
        bool result = true;
        try
        {
            string cnnstring = str_MSSQL_Connstr;
            using (var con = new SqlConnection(cnnstring))
            {
                using (var cmd = new SqlCommand(sql))
                {
                    using (var sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (var dt = new DataTable())
                        {
                            sda.Fill(dtDBTable);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Interaction.MsgBox(ex.Message);
        }
        return result;
    }

    public bool db_connSQLMaint(string sql)
    {
        bool result = true;
        try
        {
            string cnnstring = str_MSSQL_Connstr;
            var connection = new SqlConnection(cnnstring);
            connection.Open();
            var command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();
            connection.Close();
        }
        catch (Exception ex)
        {
            result = false;
            Interaction.MsgBox(ex.Message);
        }
        return result;
    }

    protected void ddlsearch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlsearch.SelectedItem.Text.Equals("-"))
        {
            txtsearch.Enabled = false;
            txtsearch.Text = "";
            ButtonAdd.Enabled = false;
        }
        else
        {
            txtsearch.Enabled = true;
            ButtonAdd.Enabled = true;
        }
    }

    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        string str;
        int dup = 0;

        if (ddlcondition.SelectedItem.Value.Equals("LIKE"))
        {
            str = ddloperate.SelectedItem.Value + " " + ddlsearch.SelectedItem.Value + " LIKE '%" + txtsearch.Text.ToUpper() + "%'" + " ";
            Interaction.MsgBox(str);
        }
        else
        {
            str = ddloperate.SelectedItem.Value.ToUpper() + " " + ddlsearch.SelectedItem.Value + " " + ddlcondition.SelectedItem.Value.ToUpper() + "'" + txtsearch.Text.ToUpper() + "'" + " ";
        }

        if (lstboxsearch.Items.Count >= 1)
        {
            for (int i = 0; i <= lstboxsearch.Items.Count - 1; i++)
            {
                if (str.Equals(lstboxsearch.Items[i].Text))
                {
                    dup = 1;
                }
            }

            if (dup == 0)
            {
                lstboxsearch.Items.Add(str);
            }

            ddloperate.Enabled = true;
        }
        else
        {
            ddloperate.Enabled = false;
            lstboxsearch.Items.Add(str);
        }
    }

    protected void ButtonClear_Click(object sender, EventArgs e)
    {
        try
        {
            if (clear == 0)
            {
                if (lstboxsearch.Items.Count == 1)
                {
                    ddloperate.Enabled = false;
                }

                for (int i = 0; i <= lstboxsearch.Items.Count - 1; i++)
                {
                    if (lstboxsearch.SelectedItem.Value.Equals(lstboxsearch.Items[i].Text))
                    {
                        lstboxsearch.Items.Remove(lstboxsearch.SelectedItem.Value);
                        clear = 1;
                        return;
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }

    protected void lnkbtnbasic_Click(object sender, EventArgs e)
    {
        ButtonAdd.Visible = false;
        ButtonClear.Visible = false;
        lstboxsearch.Visible = false;
        ddlcondition.Visible = false;
        ddloperate.Visible = false;
        lnkbtnadv.Visible = true;
        lnkbtnbasic.Visible = false;
    }

    protected void Advance_Click(object sender, EventArgs e)
    {
        ButtonAdd.Visible = true;
        if (ddlsearch.SelectedItem.Text.Equals("-"))
        {
            ButtonAdd.Enabled = false;
        }
        ButtonClear.Visible = true;
        lstboxsearch.Visible = true;
        ddlcondition.Visible = true;
        ddloperate.Visible = true;
        lnkbtnadv.Visible = false;
        lnkbtnbasic.Visible = true;
    }

    protected void ddlgridView_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Gridview1.PageIndex = int.Parse(ddlgrdpage.SelectedItem.Value);
        Gridview1.DataBind();
    }

    protected void ddlgridView2_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Gridview2.PageIndex = int.Parse(ddlgrdpage2.SelectedItem.Value);
        Gridview2.DataBind();
    }

    protected void ddlgridView3_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Gridview3.PageIndex = int.Parse(ddlgrdpage3.SelectedItem.Value);
        Gridview3.DataBind();
    }

    protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gridview1.PageIndex = e.NewPageIndex;
        Gridview1.DataBind();
        ddlgrdpage.SelectedIndex = e.NewPageIndex;
    }

    protected void gridView2_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gridview2.PageIndex = e.NewPageIndex;
        Gridview2.DataBind();
        ddlgrdpage2.SelectedIndex = e.NewPageIndex;
    }

    protected void gridView3_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Gridview3.PageIndex = e.NewPageIndex;
        Gridview3.DataBind();
        ddlgrdpage3.SelectedIndex = e.NewPageIndex;
    }

    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string item = e.Row.Cells[0].Text;

            foreach (System.Web.UI.WebControls.Image image in e.Row.Cells[0].Controls.OfType<System.Web.UI.WebControls.Image>())
            {
                image.Attributes["onclick"] = "if(!confirm('Do you want to delete " + item + "?')){ return false; };";
            }
        }
    }

    protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        var dt = new DataTable();
        TableCell id = (TableCell)Gridview1.Rows[index].Cells[1];
        TableCell scr_id = (TableCell)Gridview1.Rows[index].Cells[2];
        HyperLink desc = (HyperLink)Gridview1.Rows[index].Cells[3].FindControl("linkcode");
        string user = Session["gstrUserID"].ToString();
        string loc = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
        string Sql = "SELECT * FROM MM_VIDEOS where RECORD_TYP<>5 AND SCR_ID =" + Convert.ToInt32(scr_id.Text);

        if (!db_connSQLSel(Sql, dt))
        {
            return;
        }

        if (dt.Rows.Count < 2)
        {
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Unable to Delete - Video are being used in Lobby Screen");
            return;
        }
        else
        {
            Sql = "UPDATE MM_VIDEOS SET UPDATED_by='" + user + "', UPDATED_Loc='" + loc + "', UPDATED_DATE=GETDATE(),RECORD_TYP='5' WHERE RECORD_TYP<>'5' AND ID_MM_VIDEOS =" + Convert.ToInt32(id.Text);

            if (!db_connSQLMaint(Sql))
            {
                Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Unable to Delete - Error");
                return;
            }

            Response.Redirect(Request.RawUrl);
        }
    }

    protected void OnRowDeleting2(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        var dt2 = new DataTable();
        TableCell id = (TableCell)Gridview2.Rows[index].Cells[1];
        TableCell scr_id = (TableCell)Gridview2.Rows[index].Cells[2];
        HyperLink desc = (HyperLink)Gridview2.Rows[index].Cells[3].FindControl("linkcode");
        string user = Session["gstrUserID"].ToString();
        string loc = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
        string Sql = "SELECT * FROM MM_VIDEOS where RECORD_TYP<>5 AND SCR_ID =" + Convert.ToInt32(scr_id.Text);

        if (!db_connSQLSel(Sql, dt2))
        {
            return;
        }

        if (dt2.Rows.Count < 2)
        {
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Unable to Delete - Video are being used in Lobby Screen");
            return;
        }
        else
        {
            Sql = "UPDATE MM_VIDEOS SET UPDATED_by='" + user + "', UPDATED_Loc='" + loc + "', UPDATED_DATE=GETDATE(),RECORD_TYP='5' WHERE RECORD_TYP<>'5' AND ID_MM_VIDEOS =" + Convert.ToInt32(id.Text);

            if (!db_connSQLMaint(Sql))
            {
                Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Unable to Delete - Error");
                return;
            }

            Response.Redirect(Request.RawUrl);
        }
    }

    protected void OnRowDeleting3(object sender, GridViewDeleteEventArgs e)
    {
        int index = Convert.ToInt32(e.RowIndex);
        var dt3 = new DataTable();
        TableCell id = (TableCell)Gridview3.Rows[index].Cells[1];
        TableCell scr_id = (TableCell)Gridview3.Rows[index].Cells[2];
        HyperLink desc = (HyperLink)Gridview3.Rows[index].Cells[3].FindControl("linkcode");
        string user = Session["gstrUserID"].ToString();
        string loc = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
        string Sql = "SELECT * FROM MM_VIDEOS where RECORD_TYP<>5 AND SCR_ID =" + Convert.ToInt32(scr_id.Text);

        if (!db_connSQLSel(Sql, dt3))
        {
            return;
        }

        if (dt3.Rows.Count < 2)
        {
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Unable to Delete - Video are being used in Lobby Screen");
            return;
        }
        else
        {
            Sql = "UPDATE MM_VIDEOS SET UPDATED_by='" + user + "', UPDATED_Loc='" + loc + "', UPDATED_DATE=GETDATE(),RECORD_TYP='5' WHERE RECORD_TYP<>'5' AND ID_MM_VIDEOS =" + Convert.ToInt32(id.Text);

            if (!db_connSQLMaint(Sql))
            {
                Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Unable to Delete - Error");
                return;
            }

            Response.Redirect(Request.RawUrl);
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        pnl1.Visible = false;
        pnl2.Visible = false;
        pnl3.Visible = false;
        divchange.Visible = true;
        divchange2.Visible = true;
        divchange3.Visible = true;

        try
        {
            string TextPath = Server.MapPath("scrollingtext.txt");
            string scrollingText;
            scrollingText = File.ReadAllText(TextPath).ToString();
            txtFooter.Text = scrollingText;
        }
        catch (Exception ex)
        {
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, ex.Message);
        }

        if (Session["gstrUserID"] == null)
        {
            Response.Redirect("~/SessionExpired.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                var dt = new DataTable();
                string Sql = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID=4 order by ID_MM_VIDEOS desc";

                var dt2 = new DataTable();
                string Sql2 = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID=5 order by ID_MM_VIDEOS desc";

                var dt3 = new DataTable();
                string Sql3 = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID=6 order by ID_MM_VIDEOS desc";

                if (!db_connSQLSel(Sql, dt) | !db_connSQLSel(Sql2, dt2) | !db_connSQLSel(Sql3, dt3))
                {
                    Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Error");
                    return;
                }

                if (Session["mstmaintopic"] != null)
                {
                    Session["mstmaintopic"] = null;
                    Session["mstmaintopic"] = dt;
                }
                else
                {
                    Session["mstmaintopic"] = dt;
                }

                if (Session["mstmaintopic2"] != null)
                {
                    Session["mstmaintopic2"] = null;
                    Session["mstmaintopic2"] = dt2;
                }
                else
                {
                    Session["mstmaintopic2"] = dt2;
                }

                if (Session["mstmaintopic3"] != null)
                {
                    Session["mstmaintopic3"] = null;
                    Session["mstmaintopic3"] = dt3;
                }
                else
                {
                    Session["mstmaintopic3"] = dt3;
                }

                if (!(dt.Rows.Count == 0))
                {
                    Gridview1.DataSource = dt;
                    Gridview1.DataBind();

                    pnldisplaypage.Visible = true;

                    ddlgrdpage.Items.Clear();
                    for (int i = 0; i <= Gridview1.PageCount - 1; i++)
                    {
                        ddlgrdpage.Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
                    }
                    ddlgrdpage.DataBind();
                    ddlgrdpage.SelectedIndexChanged += ddlgridView_OnSelectedIndexChanged;
                }
                else
                {
                    Gridview1.DataSource = null;
                    Gridview1.DataBind();

                    pnldisplaypage.Visible = false;
                    divchange.Visible = false;
                }

                if (!(dt2.Rows.Count == 0))
                {
                    Gridview2.DataSource = dt2;
                    Gridview2.DataBind();

                    pnldisplaypage2.Visible = true;

                    ddlgrdpage2.Items.Clear();
                    for (int i = 0; i <= Gridview2.PageCount - 1; i++)
                    {
                        ddlgrdpage2.Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
                    }
                    ddlgrdpage2.DataBind();
                    ddlgrdpage2.SelectedIndexChanged += ddlgridView2_OnSelectedIndexChanged;
                }
                else
                {
                    Gridview2.DataSource = null;
                    Gridview2.DataBind();

                    pnldisplaypage2.Visible = false;
                    divchange2.Visible = false;
                }

                if (!(dt3.Rows.Count == 0))
                {
                    Gridview3.DataSource = dt3;
                    Gridview3.DataBind();

                    pnldisplaypage3.Visible = true;

                    ddlgrdpage3.Items.Clear();
                    for (int i = 0; i <= Gridview3.PageCount - 1; i++)
                    {
                        ddlgrdpage3.Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
                    }
                    ddlgrdpage3.DataBind();
                    ddlgrdpage3.SelectedIndexChanged += ddlgridView3_OnSelectedIndexChanged;
                }
                else
                {
                    Gridview3.DataSource = null;
                    Gridview3.DataBind();

                    pnldisplaypage3.Visible = false;
                    divchange3.Visible = false;
                }
            }
            else
            {
                if (Session["mstmaintopic"] != null)
                {
                    DataTable dt = Session["mstmaintopic"] as DataTable;
                    Gridview1.DataSource = dt;
                    Gridview1.DataBind();
                    pnldisplaypage.Visible = true;

                    ddlgrdpage.Items.Clear();
                    for (int i = 0; i <= Gridview1.PageCount - 1; i++)
                    {
                        ddlgrdpage.Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
                    }

                    ddlgrdpage.DataBind();
                    ddlgrdpage.SelectedIndexChanged += ddlgridView_OnSelectedIndexChanged;
                }

                if (Session["mstmaintopic2"] != null)
                {
                    DataTable dt2 = Session["mstmaintopic2"] as DataTable;
                    Gridview2.DataSource = dt2;
                    Gridview2.DataBind();
                    pnldisplaypage2.Visible = true;

                    ddlgrdpage.Items.Clear();
                    for (int i = 0; i <= Gridview1.PageCount - 1; i++)
                    {
                        ddlgrdpage.Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
                    }

                    ddlgrdpage.DataBind();
                    ddlgrdpage.SelectedIndexChanged += ddlgridView_OnSelectedIndexChanged;
                }

                if (Session["mstmaintopic3"] != null)
                {
                    DataTable dt3 = Session["mstmaintopic3"] as DataTable;
                    Gridview3.DataSource = dt3;
                    Gridview3.DataBind();
                    pnldisplaypage3.Visible = true;

                    ddlgrdpage.Items.Clear();
                    for (int i = 0; i <= Gridview1.PageCount - 1; i++)
                    {
                        ddlgrdpage.Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
                    }

                    ddlgrdpage.DataBind();
                    ddlgrdpage.SelectedIndexChanged += ddlgridView_OnSelectedIndexChanged;
                }
            }
            linkbtn.Click += Redirect;
            linkbtn2.Click += Redirect2;
            linkbtn3.Click += Redirect3;
        }
    }

    protected void ButtonExecute_Click(object sender, EventArgs e)
    {
        try
        {
            divchange.Visible = true;

            Button btnadd = pnl1.FindControl("ButtonAdd") as Button;
            var dt = new DataTable();
            string Sql = string.Empty;

            if (btnadd != null && btnadd.Visible == true)
            {
                string str = string.Empty;

                if (lstboxsearch.Items.Count != 0)
                {
                    for (int i = 0; i <= lstboxsearch.Items.Count - 1; i++)
                    {
                        str = str + lstboxsearch.Items[i].Text;
                    }
                    Sql = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND " + str;
                }
                else
                {
                    Sql = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5";
                }
            }
            else
            {
                if (!ddlsearch.SelectedItem.Value.Equals("-"))
                {
                    Sql = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND UPPER(" + ddlsearch.SelectedItem.Value + ") LIKE '%" + txtsearch.Text.ToUpper() + "%' ";
                }
                else
                {
                    Sql = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5";
                }
            }

            if (!db_connSQLSel(Sql, dt))
            {
                return;
            }

            if (Session["mstmaintopic"] != null)
            {
                Session["mstmaintopic"] = null;
                Session["mstmaintopic"] = dt;
            }
            else
            {
                Session["mstmaintopic"] = dt;
            }

            if (!(dt.Rows.Count == 0))
            {
                Gridview1.DataSource = dt;
                Gridview1.DataBind();
                pnldisplaypage.Visible = true;

                ddlgrdpage.Items.Clear();
                for (int i = 0; i <= Gridview1.PageCount - 1; i++)
                {
                    ddlgrdpage.Items.Add(new ListItem((i + 1).ToString(), i.ToString()));
                }
                ddlgrdpage.DataBind();
                ddlgrdpage.SelectedIndexChanged += ddlgridView_OnSelectedIndexChanged;
            }
            else
            {
                Gridview1.DataSource = null;
                Gridview1.DataBind();
                pnldisplaypage.Visible = false;
                divchange.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, ex.Message);
        }
    }

    protected void Redirect(object sender, EventArgs e)
    {
        Response.Redirect("~/acc/MstMainPan/FullMainScreen_Dtl.aspx?action=1");
    }

    protected void Redirect2(object sender, EventArgs e)
    {
        Response.Redirect("~/acc/MstMainPan/Upper2ndScreen_Dtl.aspx?action=1");
    }

    protected void Redirect3(object sender, EventArgs e)
    {
        Response.Redirect("~/acc/MstMainPan/Lower2ndScreen_Dtl.aspx?action=1");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        Session["display"] = "Main";
        Response.Redirect("~/acc/entry/MainScreenVideo.aspx");
    }

    protected void btnFooter_Click(object sender, EventArgs e)
    {
        try
        {
            string TextPath = Server.MapPath("scrollingtext.txt");
            File.WriteAllText(TextPath, txtFooter.Text);
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Text Saved.");
        }
        catch (Exception ex)
        {
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, ex.Message);
        }
    }
}
