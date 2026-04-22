using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using ACL.MenuBar.Object;

public partial class Style2_Menu : System.Web.UI.Page
{
    protected LeftMenuItemList _list;
    private int _userid;
    private int _systemid;
    protected string _words;
    private bool _pointer = false;

    private string _SignOutURL = string.Empty;
    protected string SignOutURL
    {
        get { return _SignOutURL; }
    }

    private string _HomeURL = string.Empty;
    protected string HomeURL
    {
        get { return _HomeURL; }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (Request.QueryString["id"] != null)
        {
            frContent.Attributes["src"] = "../acc/Entry/" + Request.QueryString["page"].ToString() + Request.Url.Query;
        }
        else
        {
            frContent.Attributes["src"] = "";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this._SignOutURL = ResolveUrl("~/SessionExpired.aspx");
        this._HomeURL = ResolveUrl("~/Default.aspx");

        if (Request.QueryString[ACL.Control.URL.URLEMPLOYEEID] != null)
        {
            Session["gstrUserID"] = ACL.Security.Encryption.Decrypt(Request.QueryString[ACL.Control.URL.URLEMPLOYEEID]);
            _pointer = true;
        }

        if (Session["gstrUserID"] == null)
        {
            Response.Redirect("~/SessionExpired.aspx?ReturnURL=" + Server.UrlEncode(Request.RawUrl));
        }

        if (Session["system"] == null || (string)Session["system"] == "0")
        {
            systemCheck(Request.QueryString[ACL.Control.URL.URLSYSTEMNAME]);
        }

        ahrefhome.Visible = _pointer;

        if (_pointer)
        {
            this._SignOutURL = ConfigurationManager.AppSettings["MISSignout"];
            ahrefhome.HRef = ConfigurationManager.AppSettings["MISHome"];
            try
            {
                ACL.Object.User userobj = ACL.OracleClass.User.UserInfo(ConfigurationManager.ConnectionStrings["ORCL_ACL"].ConnectionString, Convert.ToInt32(Session["gstrUserID"]));
                Session["gstrUserID"] = userobj.UserID;
                Session["gettemp"] = userobj.EmployeeName;
                Session["gstrUsername"] = userobj.Username;
                Session["gstrPassword"] = userobj.Password;
                Session["LoginHis"] = DateTime.Now.ToString("dd MMMM yyyy");
                Session["gstrUserCompCode"] = userobj.UserCom;
                Session["com"] = userobj.UserCom;
            }
            catch (Exception)
            {
                Response.Redirect(ConfigurationManager.AppSettings["MISHome"]);
            }
        }

        _userid = Convert.ToInt32(Session["gstrUserID"]);
        _systemid = Convert.ToInt32(Session["system"]);

        if (_list == null)
        {
            _list = new LeftMenuItemList();
        }

        int counter = 0;
        var _acl = new ACL.OracleClass.Resource(ConfigurationManager.ConnectionStrings["ORCL_ACL"].ConnectionString);
        List<ACL.Object.Resource> _sourcelist = _acl.RetrieveResource(_userid, _systemid);
        StringBuilder _str;
        int _altcounter = 0;

        foreach (ACL.Object.Resource itm in ACL.Search.GetParent(_sourcelist, _systemid))
        {
            _list.AddItem(new LeftMenuItem("left_menu_" + counter, itm.ResouceDesc, false));
            _str = new StringBuilder();
            _altcounter = 0;

            foreach (ACL.Object.Resource node in ACL.Search.GetParent(_sourcelist, itm.ResourceID))
            {
                _altcounter += 1;
                _str.AppendFormat("<li class='{2}'><a {3} href='{0}'>{1}</a></li>", GenerateKeywords(node.ResourceURL, _userid.ToString(), (string)Session["gstrUserCom"], (string)Session["gettemp"], node.ResourceName), node.ResouceDesc, (_altcounter % 2 == 0 ? "alt" : "nor"), "target='page'");
            }

            liItems.Text += string.Format("<div class='bar_itms' id='{0}'><ul>{1}</ul></div>", "left_menu_" + counter, _str.ToString());
            counter += 1;
        }

        if (DateTime.Now.Hour < 12)
        {
            _words = "Good Morning";
        }
        else if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour <= 17)
        {
            _words = "Good Afternoon";
        }
        else
        {
            _words = "Good Evening";
        }
    }

    public string GenerateKeywords(string URL, string ID, string Company, string Name, string System)
    {
        return Server.HtmlEncode(ResolveUrl(URL));
    }

    private void systemCheck(string Systemname)
    {
        Session["system"] = 0;
        Session["system"] = ACL.OracleClass.Resource.RetrieveApplicationIDByName(ConfigurationManager.ConnectionStrings["ORCL_ACL"].ConnectionString, Systemname);

        if ((int)Session["system"] == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Alert", "alert('Invalid System');", true);
            return;
        }
    }
}
