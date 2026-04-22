using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

public partial class acc_LobbyDisplay_lobby_2ndDisplay : System.Web.UI.Page
{
    protected string video_lists = string.Empty;
    protected string video_lists2 = string.Empty;
    protected string seek_starts = string.Empty;
    protected string seek_ends = string.Empty;
    protected string seek_starts2 = string.Empty;
    protected string seek_ends2 = string.Empty;
    protected string period_starts = string.Empty;
    protected string period_ends = string.Empty;
    protected string period_starts2 = string.Empty;
    protected string period_ends2 = string.Empty;
    protected string scrollingText = string.Empty;

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
        catch (Exception)
        {
            // Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, ex.Message);
        }
        return result;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID=2";
        var dt = new DataTable();
        if (!db_connSQLSel(sql, dt))
        {
            return;
        }

        string sql2 = "SELECT * FROM MM_VIDEOS WHERE RECORD_TYP<>5 AND SCR_ID=3";
        var dt2 = new DataTable();
        if (!db_connSQLSel(sql2, dt2))
        {
            return;
        }

        string path1 = "\\acc\\LobbyDisplay\\secscrtop\\";
        string path2 = "\\acc\\LobbyDisplay\\secscrbtm\\";

        int videocount = 0;

        if (dt.Rows.Count != 0)
        {
            video_lists += "[";
            seek_starts += "[";
            seek_ends += "[";
            period_starts += "[";
            period_ends += "[";

            foreach (DataRow Row in dt.Rows)
            {
                if (videocount == 0)
                {
                    video_lists += "'" + path1 + (Row["ATTACH_FILE"]).ToString() + "'";
                    seek_starts += "'" + (Row["SEEK_START"]).ToString() + "'";
                    seek_ends += "'" + (Row["SEEK_END"]).ToString() + "'";
                    period_starts += "'" + (Row["PERIOD_START"]).ToString() + "'";
                    period_ends += "'" + (Row["PERIOD_END"]).ToString() + "'";
                }
                else
                {
                    video_lists += ",'" + path1 + (Row["ATTACH_FILE"]).ToString() + "'";
                    seek_starts += ",'" + (Row["SEEK_START"]).ToString() + "'";
                    seek_ends += ",'" + (Row["SEEK_END"]).ToString() + "'";
                    period_starts += ",'" + (Row["PERIOD_START"]).ToString() + "'";
                    period_ends += ",'" + (Row["PERIOD_END"]).ToString() + "'";
                }

                videocount = +1;
            }

            video_lists += "]";
            seek_starts += "]";
            seek_ends += "]";
            period_starts += "]";
            period_ends += "]";
        }

        video_lists = video_lists.Replace("\\", "/");

        if (dt2.Rows.Count != 0)
        {
            video_lists2 += "[";
            seek_starts2 += "[";
            seek_ends2 += "[";
            period_starts2 += "[";
            period_ends2 += "[";

            foreach (DataRow Row in dt2.Rows)
            {
                video_lists2 += "'" + path2 + (Row["ATTACH_FILE"]).ToString() + "',";
                seek_starts2 += "'" + (Row["SEEK_START"]).ToString() + "',";
                seek_ends2 += "'" + (Row["SEEK_END"]).ToString() + "',";
                period_starts2 += "'" + (Row["PERIOD_START"]).ToString() + "',";
                period_ends2 += "'" + (Row["PERIOD_END"]).ToString() + "',";
            }

            video_lists2 += "]";
            seek_starts2 += "]";
            seek_ends2 += "]";
            period_starts2 += "]";
            period_ends2 += "]";
        }

        video_lists2 = video_lists2.Replace("\\", "/");

        try
        {
            string TextPath = Server.MapPath("..\\MstMain\\scrollingtext.txt");
            scrollingText = File.ReadAllText(TextPath).ToString();
        }
        catch (Exception ex)
        {
            Library.Root.Control.MessageCenter.ShowAJAXMessageBox(this.Page, ex.Message);
        }

        Session["Checkpoint"] = "1";

        if (IsPostBack)
        {
            var video_lists_tr = video_lists.Replace("[", "");
            video_lists_tr = video_lists_tr.Replace("]", "");
            video_lists_tr = video_lists_tr.Substring(0, video_lists_tr.Length - 1);

            var seek_starts_tr = seek_starts.Replace("[", "");
            seek_starts_tr = seek_starts_tr.Replace("]", "");
            seek_starts_tr = seek_starts_tr.Substring(0, seek_starts_tr.Length - 1);

            var seek_ends_tr = seek_ends.Replace("[", "");
            seek_ends_tr = seek_ends_tr.Replace("]", "");
            seek_ends_tr = seek_ends_tr.Substring(0, seek_ends_tr.Length - 1);

            var period_starts_tr = period_starts.Replace("[", "");
            period_starts_tr = period_starts_tr.Replace("]", "");
            period_starts_tr = period_starts_tr.Substring(0, period_starts_tr.Length - 1);

            var period_ends_tr = period_ends.Replace("[", "");
            period_ends_tr = period_ends_tr.Replace("]", "");
            period_ends_tr = period_ends_tr.Substring(0, period_ends_tr.Length - 1);

            var video_lists_tr2 = video_lists2.Replace("[", "");
            video_lists_tr2 = video_lists_tr2.Replace("]", "");
            video_lists_tr2 = video_lists_tr2.Substring(0, video_lists_tr2.Length - 1);

            var seek_starts_tr2 = seek_starts2.Replace("[", "");
            seek_starts_tr2 = seek_starts_tr2.Replace("]", "");
            seek_starts_tr2 = seek_starts_tr2.Substring(0, seek_starts_tr2.Length - 1);

            var seek_ends_tr2 = seek_ends2.Replace("[", "");
            seek_ends_tr2 = seek_ends_tr2.Replace("]", "");
            seek_ends_tr2 = seek_ends_tr2.Substring(0, seek_ends_tr2.Length - 1);

            var period_starts_tr2 = period_starts2.Replace("[", "");
            period_starts_tr2 = period_starts_tr2.Replace("]", "");
            period_starts_tr2 = period_starts_tr2.Substring(0, period_starts_tr2.Length - 1);

            var period_ends_tr2 = period_ends2.Replace("[", "");
            period_ends_tr2 = period_ends_tr2.Replace("]", "");
            period_ends_tr2 = period_ends_tr2.Substring(0, period_ends_tr2.Length - 1);

            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Javascript", "loaded(\"" + video_lists_tr + "\", \"" + seek_starts_tr + "\", \"" + seek_ends_tr + "\", \"" + period_starts_tr + "\", \"" + period_ends_tr + "\", \"" + video_lists_tr2 + "\", \"" + seek_starts_tr2 + "\", \"" + seek_ends_tr2 + "\", \"" + period_starts_tr2 + "\", \"" + period_ends_tr2 + "\");onPageLoad();", true);
        }
        else
        {
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Javascript", "loaded2(); ", true);
        }
    }
}
