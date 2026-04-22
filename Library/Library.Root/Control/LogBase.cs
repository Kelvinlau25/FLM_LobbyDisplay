using Microsoft.VisualBasic;

namespace Library.Root.Control
{
    public abstract class LogBase : System.Web.UI.Page
    {
        protected Library.Root.Other.GenericCollection<Library.Root.Object.Log> _list;

        protected override void OnInit(System.EventArgs e)
        {
            this.BindKey();
            base.OnInit(e);
            this.BindData();
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
        }

        private void BindKey()
        {
            foreach (string _query in Request.QueryString)
            {
                if (!string.IsNullOrEmpty(_query))
                {
                    switch (_query)
                    {
                        case "id":
                            this._key = Server.UrlDecode(Request.QueryString["id"]);
                            break;
                        case "key":
                            this._setupKey = Request.QueryString["key"];
                            break;
                        case "page":
                            if (Information.IsNumeric(Request.QueryString["page"]))
                            {
                                this._Pageno = int.Parse(Request.QueryString["page"]);
                            }
                            else
                            {
                                this._Pageno = 1;
                            }
                            break;
                    }
                }
            }
        }

        protected abstract void BindData();

        private string _key = string.Empty;
        public string Key
        {
            get { return _key; }
        }

        private int _Pageno = 0;
        public int PageNo
        {
            get { return _Pageno; }
            set { _Pageno = value; }
        }

        private string _setupKey = string.Empty;
        public string SetupKey
        {
            get { return _setupKey; }
        }

        public string KeyDesc
        {
            get
            {
                if (_list.TotalRow > 0)
                {
                    return _list.Data[0].KeyDesc;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public abstract string LogTitle { get; }
        public abstract string LogPage { get; }

        public string NormalTitle
        {
            get { return this.LogTitle; }
        }

        public string DisplayTitle
        {
            get { return this.LogTitle + " Audit Trail"; }
        }

        public string GenerateList
        {
            get { return ResolveUrl(this.LogPage + "?id=" + Server.UrlEncode(this.Key) + "&key=" + this.SetupKey + "&page=" + this.PageNo); }
        }
    }
}
