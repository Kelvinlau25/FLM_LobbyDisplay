using System;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void btnSubmit_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        if (Page.IsValid)
        {
            string Dtl = "";
            if (!Request.Url.Query.ToString().Equals(""))
            {
                Dtl = Request.Url.Query;
            }
            if (Dtl.Equals(""))
            {
                Response.Redirect("~/Menu/Menu.aspx");
            }
            else
            {
                Response.Redirect("~/Menu/Menu.aspx" + Dtl);
            }
        }
    }

    protected void cusCustom_ServerValidate(object sender, ServerValidateEventArgs e)
    {
        try
        {
            systemCheck();

            var temp = new ACL.OracleClass.User(ConfigurationManager.ConnectionStrings["ORCL_ACL"].ConnectionString);
            ACL.Object.User userobj = new ACL.Object.User();

            if (ConfigurationManager.AppSettings["CrossCompany"] == "1")
            {
                userobj = temp.validateWithRetrieveUsernCompany(ddlcompany.SelectedValue, txtusername.Text.Trim(), txtpassword.Text.Trim(), Convert.ToInt32(Session["system"]));
            }
            else
            {
                userobj = temp.validateWithRetrieveUser(txtusername.Text.Trim(), txtpassword.Text.Trim(), Convert.ToInt32(Session["system"]));
            }

            if (Convert.ToInt32(userobj.UserID) > -1)
            {
                Session["gstrUserID"] = userobj.UserID;
                Session["gettemp"] = userobj.EmployeeName;
                Session["gstrUsername"] = txtusername.Text.Trim();
                Session["gstrPassword"] = txtpassword.Text.Trim();
                Session["LoginHis"] = DateTime.Now.ToString("dd MMMM yyyy");
                Session["gstrUserCompCode"] = userobj.UserCom;
                Session["com"] = userobj.UserCom;
                e.IsValid = true;
            }
            else
            {
                e.IsValid = false;
                ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('" + "" + "');", true);
            }
        }
        catch (Exception)
        {
            e.IsValid = false;
            ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('- Invalid username and password ');", true);
        }
    }

    private void systemCheck()
    {
        //Validate the system
        Session["system"] = 0;
        Session["system"] = ACL.OracleClass.Resource.RetrieveApplicationIDByName(ConfigurationManager.ConnectionStrings["ORCL_ACL"].ConnectionString, ConfigurationManager.AppSettings["SystemName"]);
        if (Session["system"] == null || (int)Session["system"] == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('Invalid System');", true);
            return;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // ACL.Control.Binding.BindCompany(ConfigurationManager.ConnectionStrings("ORCL_ACL").ConnectionString, ddlcompany)
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session.Abandon();
            Session.Clear();
            trcompanyselection.Visible = ConfigurationManager.AppSettings["CrossCompany"] == "1";
        }

        txtusername.Focus();
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        if (Request.QueryString["userid"] != null)
        {
            txtusername.Text = Request.QueryString["userid"].ToString();
        }
    }
}
