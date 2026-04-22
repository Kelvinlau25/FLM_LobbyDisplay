using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

public partial class acc_MstMain_FullMainScreen_Dtl : Control.Base
{
    private string str_MSSQL_Connstr = ConfigurationManager.ConnectionStrings["filmDisplay"].ConnectionString;

    public override void BindData()
    {
    }

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
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, ex.Message);
            result = false;
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
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, ex.Message);
        }
        return result;
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Session["gstrUserID"] == null)
        {
            Response.Redirect("~/SessionExpired.aspx");
        }
        else
        {
            DataTable dt = new DataTable();

            if (base.Action == EnumAction.Edit)
            {
                lblMainTitle.Text = "Master Maintenance Main Screen Video - Edit";

                fileupload.Visible = false;
                lblFileUpload.Visible = true;

                dt = new DataTable();
                string Sql = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND ID_MM_VIDEOS=" + Request.QueryString["ID"].ToString();

                if (!db_connSQLSel(Sql, dt))
                {
                    return;
                }

                if (dt.Rows.Count != 0)
                {
                    lblFileUpload.Text = dt.Rows[0]["ATTACH_FILE"].ToString();
                    txt_skstart.Text = dt.Rows[0]["SEEK_START"].ToString();
                    txt_skstop.Text = dt.Rows[0]["SEEK_END"].ToString();
                    string periodStart = dt.Rows[0]["PERIOD_START"].ToString();
                    string periodEnd = dt.Rows[0]["PERIOD_END"].ToString();
                    txt_tstart.Text = periodStart.Substring(0, periodStart.Length - 3);
                    txt_tstop.Text = periodEnd.Substring(0, periodEnd.Length - 3);
                    lblcreatedby.Text = dt.Rows[0]["CREATED_BY"].ToString();
                    lblcreateddate.Text = dt.Rows[0]["CREATED_DATE"].ToString();
                    lblcreatedloc.Text = dt.Rows[0]["CREATED_LOC"].ToString();
                    lblupdatedby.Text = dt.Rows[0]["UPDATED_BY"].ToString();
                    lblupdateddate.Text = dt.Rows[0]["UPDATED_DATE"].ToString();
                    lblupdatedloc.Text = dt.Rows[0]["UPDATED_LOC"].ToString();

                    lblerror.Visible = false;
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Attributes.Add("color", "red");
                }
            }
            else if (base.Action == EnumAction.Add)
            {
                lblMainTitle.Text = "Master Maintenance Main Screen Video - Add";
                fileupload.Visible = true;
                lblFileUpload.Visible = false;
                pnlAudit.Visible = false;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string user = Session["gstrUserID"].ToString();
        string loc = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();

        string tpth = "";
        try
        {
            string name = string.Empty;
            string _temp = "0";
            int seek_start = 0;
            int seek_end = 0;
            string start_time = "00:00:00";
            string end_time = "23:59:59";

            if (base.Action == EnumAction.Add)
            {
                if (fileupload.HasFile)
                {
                    string ext = Path.GetExtension(fileupload.FileName).ToLower();

                    name = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Path.GetFileName(fileupload.FileName).ToLower();

                    if (ext.Equals(".mp4"))
                    {
                        int fileSize = fileupload.PostedFile.ContentLength;

                        if (fileSize <= 4294967295L)
                        {
                            string paths = Server.MapPath("~/acc/PantryDisplay/mainscr/" + name);

                            string saved = (ResolveUrl(paths));
                            tpth = saved;
                            fileupload.SaveAs(saved);
                        }
                        else
                        {
                            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Video File Uploaded Must Not Exceeded 400MB");
                            return;
                        }
                    }
                    else
                    {
                        Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Upload Only .mp4 Video");
                        return;
                    }
                }
                else
                {
                    Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Please Select Video");
                    return;
                }
            }

            if (base.Action == EnumAction.Add)
            {
                var dt = new DataTable();
                string Sql = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND UPPER(Attach_File)='" + name + "'";

                if (!db_connSQLSel(Sql, dt))
                {
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    double startValue;
                    double endValue;

                    if (double.TryParse(txt_skstart.Text, out startValue))
                    {
                        seek_start = (int)startValue;
                    }
                    else
                    {
                        Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Seek Start Only Allow Number!");
                        return;
                    }

                    if (double.TryParse(txt_skstop.Text, out endValue))
                    {
                        seek_end = (int)endValue;
                    }
                    else
                    {
                        Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Seek End Only Allow Number!");
                        return;
                    }
                    start_time = (!(txt_tstart.Text == string.Empty)) ? txt_tstart.Text + ":00" : "00:00:00";
                    end_time = (!(txt_tstop.Text == string.Empty)) ? txt_tstop.Text + ":59" : "23:59:59";

                    Sql = "INSERT INTO MM_VIDEOS (ATTACH_FILE,SEEK_START,SEEK_END,PERIOD_START,PERIOD_END,SCR_ID,RECORD_TYP,CREATED_BY,CREATED_DATE,CREATED_LOC,UPDATED_BY,UPDATED_DATE,UPDATED_LOC) VALUES ('"
                        + name + "','" + seek_start + "','" + seek_end + "','" + start_time + "','" + end_time + "','4','1','" + user + "',GETDATE() ,'" + loc + "','" + user + "',GETDATE() ,'" + loc + "')";

                    if (!db_connSQLMaint(Sql))
                    {
                        Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Unable to Insert - Error");
                        return;
                    }

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert('Video Successfully Added.'); setTimeout(function(){window.location.href='MM_VerticalScreenFull.aspx'}, 500);", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('- Duplicate data ');", true);
                }
            }
            else if (base.Action == EnumAction.Edit)
            {
                var vid = Request.QueryString["ID"].ToString();
                var dt = new DataTable();
                string Sql = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND ID_MM_VIDEOS = '" + vid + "'";

                if (!db_connSQLSel(Sql, dt))
                {
                    return;
                }

                if (dt.Rows.Count != 0)
                {
                    double startValue;
                    double endValue;

                    if (double.TryParse(txt_skstart.Text, out startValue))
                    {
                        seek_start = (int)startValue;
                    }
                    else
                    {
                        Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Seek Start Only Allow Number!");
                        return;
                    }

                    if (double.TryParse(txt_skstop.Text, out endValue))
                    {
                        seek_end = (int)endValue;
                    }
                    else
                    {
                        Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Seek End Only Allow Number!");
                        return;
                    }
                    start_time = (!(txt_tstart.Text == string.Empty)) ? txt_tstart.Text + ":00" : "00:00:00";
                    end_time = (!(txt_tstop.Text == string.Empty)) ? txt_tstop.Text + ":59" : "23:59:59";

                    Sql = "UPDATE MM_VIDEOS SET SEEK_START = '" + seek_start + "',SEEK_END = '" + seek_end + "',PERIOD_START = '" + start_time + "',PERIOD_END = '" + end_time + "',RECORD_TYP = '3',UPDATED_BY = '" + user + "',UPDATED_DATE = GETDATE(),UPDATED_LOC = '" + loc + "' WHERE ID_MM_VIDEOS = '" + vid + "'";

                    if (!db_connSQLMaint(Sql))
                    {
                        Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, "Unable to Edit - Error");
                        return;
                    }

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert('Edits Successfully Updated.'); setTimeout(function(){window.location.href='MM_VerticalScreenFull.aspx'}, 500);", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('- Video Not Found ');", true);
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Access"))
            {
                Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, tpth + " Path Unable to Upload Error: Access Denied");
            }
            else
            {
                Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, ex.Message);
            }
        }
    }
}
