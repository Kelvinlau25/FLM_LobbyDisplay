using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Library.Root.Control
{
    /// <summary>
    /// Handler All Page Common Function (net8.0 migration — System.Web.UI.Page removed)
    /// -------------------------------------------------------------------------------
    /// C.C.Yeon    25 April 2011   initial Version
    /// (migrated)  Removed Web Forms lifecycle, GridView, and System.Web dependencies.
    ///             IPageContext injected for Redirect/ResolveUrl/UrlEncode/UrlDecode.
    ///             QueryString supplied via constructor parameter.
    ///             ViewState replaced with in-memory Dictionary.
    /// </summary>
    public abstract class Base
    {
        public enum EnumAction
        {
            None = 0,
            Add = 1,
            Edit = 3,
            Delete = 5,
            View = 7,
            History = 9
        }

        // Injected dependencies
        private readonly IPageContext _pageContext;
        private readonly NameValueCollection _queryString;
        private readonly bool _isPostBack;
        private Dictionary<string, object> _viewState = new Dictionary<string, object>();

        protected Base(IPageContext pageContext, NameValueCollection queryString, bool isPostBack)
        {
            _pageContext = pageContext;
            _queryString = queryString;
            _isPostBack = isPostBack;

            if (this.FunctionControl)
            {
                this.BindAction();
            }

            this.BindSort();

            if (this.FunctionControl)
            {
                this.CheckURL();
            }

            if (this.FunctionControl)
            {
                this.BindKey();
            }
        }

        public void CheckURL()
        {
            if (!_isPostBack)
            {
                if (this.DefaultSort != string.Empty)
                {
                    if (SortField == string.Empty)
                    {
                        _pageContext.Redirect(GenerateList);
                    }
                }
                else
                {
                    if (this._Action == EnumAction.None)
                    {
                        _pageContext.Redirect(GetUrl(EnumAction.None));
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve the Key from ViewState or URL
        /// </summary>
        private void BindKey()
        {
            if (!_viewState.ContainsKey("Key") || _viewState["Key"] == null)
            {
                this._Key = _queryString["id"];
                _viewState["Key"] = this._Key;
            }
            else
            {
                this._Key = (string)_viewState["Key"];
            }
        }

        /// <summary>
        /// Retrieve the Sort Field and Sort Direction
        /// </summary>
        private void BindSort()
        {
            foreach (string _query in _queryString)
            {
                if (!string.IsNullOrEmpty(_query))
                {
                    switch (_query)
                    {
                        case "sort":
                            this._SortField = _queryString["sort"];
                            break;
                        case "dic":
                            this._sortDirection = _queryString["dic"];
                            break;
                        case "page":
                            if (int.TryParse(_queryString["page"], out int pageNum))
                            {
                                this._Pageno = pageNum;
                            }
                            else
                            {
                                this._Pageno = 1;
                            }
                            break;
                        case "fld":
                            this._SearchField = Uri.UnescapeDataString(_queryString["fld"]);
                            break;
                        case "vl":
                            string ASD = _queryString["vl"];
                            if (ASD.Contains("+"))
                            {
                                this._SearchValue = Uri.UnescapeDataString(_queryString["vl"]);
                            }
                            else
                            {
                                this._SearchValue = _queryString["vl"];
                            }
                            break;
                        case "type":
                            this._type = _queryString["type"];
                            break;
                        case "itm1":
                            this._item1 = _queryString["itm1"];
                            break;
                        case "itm2":
                            this._item2 = _queryString["itm2"];
                            break;
                        case "itm3":
                            this._item3 = _queryString["itm3"];
                            break;
                        case "itm4":
                            this._item4 = _queryString["itm4"];
                            break;
                        case "itm5":
                            this._item5 = _queryString["itm5"];
                            break;
                        case "itm6":
                            this._item6 = _queryString["itm6"];
                            break;
                        case "itm7":
                            this._item7 = _queryString["itm7"];
                            break;
                        case "itm8":
                            this._item8 = _queryString["itm8"];
                            break;
                        case "itm9":
                            this._item9 = _queryString["itm9"];
                            break;
                        case "itm10":
                            this._item10 = _queryString["itm10"];
                            break;
                        case "dlt":
                            this._ShowDeleted = bool.Parse(_queryString["dlt"]);
                            break;
                        case "id":
                            if (this.DetailListingFunction)
                            {
                                this._Key = _queryString["id"];
                            }
                            break;
                        case "k":
                            this._searchkeyword = _queryString["k"];
                            break;
                        case "t":
                            this._searchtype = _queryString["t"];
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Retrieve the Action From URL
        /// </summary>
        private void BindAction()
        {
            if (!string.IsNullOrEmpty(_queryString["action"]))
            {
                switch (_queryString["action"])
                {
                    case "1":
                        this._Action = EnumAction.Add;
                        break;
                    case "3":
                        this._Action = EnumAction.Edit;
                        break;
                    case "5":
                        this._Action = EnumAction.Delete;
                        break;
                    case "7":
                        this._Action = EnumAction.View;
                        break;
                    default:
                        this._Action = EnumAction.None;
                        break;
                }
            }
        }

        // 35
        private bool _DetailListingFunction = false;
        public bool DetailListingFunction
        {
            get { return _DetailListingFunction; }
            set { _DetailListingFunction = value; }
        }

        private string _type = string.Empty;
        public string Type
        {
            get { return _type; }
        }

        // Page Key
        private string _Key = string.Empty;
        public string Key
        {
            get { return _Key; }
        }

        // Action Such as Insert,Edit or Delete
        private EnumAction _Action = EnumAction.None;
        public EnumAction Action
        {
            get { return _Action; }
        }

        private string _DefaultSort = string.Empty;
        public string DefaultSort
        {
            get { return _DefaultSort; }
            set { _DefaultSort = value; }
        }

        private string _sortDirection = "1";
        public string SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }

        private string _SortField = string.Empty;
        public string SortField
        {
            get { return _SortField; }
            set { _SortField = value; }
        }

        private int _Pageno = 1;
        public int PageNo
        {
            get { return _Pageno; }
            set { _Pageno = value; }
        }

        private string _SearchField = string.Empty;
        public string SearchField
        {
            get { return Uri.UnescapeDataString(_SearchField); }
            set { _SearchField = Uri.EscapeDataString(value); }
        }

        private string _SearchValue = string.Empty;
        public string SearchValue
        {
            get { return Uri.UnescapeDataString(_SearchValue); }
            set
            {
                if (value.IndexOf("AND ") != -1 || value.IndexOf("OR ") != -1)
                {
                    if (value.Substring(0, 4).IndexOf("AND ") != -1)
                    {
                        value = value.Substring(3, value.Length - 3);
                    }
                    else if (value.Substring(0, 3).IndexOf("OR ") != -1)
                    {
                        value = value.Substring(2, value.Length - 2);
                    }
                }
                _SearchValue = Uri.EscapeDataString(value);
            }
        }

        private bool _ShowDeleted = false;
        public bool ShowDeleted
        {
            get { return _ShowDeleted; }
            set { _ShowDeleted = value; }
        }

        private string _item1 = string.Empty;
        public string Item1
        {
            get { return Uri.UnescapeDataString(_item1); }
            set { _item1 = value; }
        }

        private string _item2 = string.Empty;
        public string Item2
        {
            get { return _item2; }
            set { _item2 = value; }
        }

        private string _item3 = string.Empty;
        public string Item3
        {
            get { return _item3; }
            set { _item3 = value; }
        }

        private string _item4 = string.Empty;
        public string Item4
        {
            get { return _item4; }
            set { _item4 = value; }
        }

        private string _item5 = string.Empty;
        public string Item5
        {
            get { return _item5; }
            set { _item5 = value; }
        }

        private string _item6 = string.Empty;
        public string Item6
        {
            get { return _item6; }
            set { _item6 = value; }
        }

        private string _item7 = string.Empty;
        public string Item7
        {
            get { return _item7; }
            set { _item7 = value; }
        }

        private string _item8 = string.Empty;
        public string Item8
        {
            get { return _item8; }
            set { _item8 = value; }
        }

        private string _item9 = string.Empty;
        public string Item9
        {
            get { return _item9; }
            set { _item9 = value; }
        }

        private string _item10 = string.Empty;
        public string Item10
        {
            get { return _item10; }
            set { _item10 = value; }
        }

        private string _searchtype = "";
        public string SearchType
        {
            get { return _searchtype; }
            set { _searchtype = value; }
        }

        private string _searchkeyword = "";
        public string SearchKeyword
        {
            get { return _searchkeyword; }
            set { _searchkeyword = value; }
        }

        // 5
        public string ActionDesc
        {
            get
            {
                switch (_Action)
                {
                    case EnumAction.Add:
                        return "Add";
                    case EnumAction.Delete:
                        return "Delete";
                    case EnumAction.Edit:
                        return "Edit";
                    case EnumAction.View:
                        return "View";
                    case EnumAction.None:
                        return string.Empty;
                    default:
                        return string.Empty;
                }
            }
        }

        // 7
        public string GenerateList
        {
            get
            {
                return _pageContext.ResolveUrl(this.ListPage) + "?sort=" + (this.SortField == string.Empty ? DefaultSort : this.SortField) +
                    "&dic=" + (this.SortDirection == string.Empty ? "0" : this.SortDirection) + "&page=" + this._Pageno + "&fld=" + Uri.EscapeDataString(this._SearchField) + "&vl=" +
                    Uri.EscapeDataString(this._SearchValue) + "&type=" + this._type + "&itm1=" + Uri.EscapeDataString(this._item1) + "&itm2=" + this._item2 + "&itm3=" + this._item3 + "&itm4=" +
                    this._item4 + "&itm5=" + this._item5 + "&itm6=" + this._item6 + "&itm7=" + this._item7 + "&itm8=" + this._item8 + "&itm9=" + this._item9 +
                    "&itm10=" + this._item10 + "&dlt=" + this._ShowDeleted + (this._DetailListingFunction ? "&" + DetailListingAddtionalFunction() : string.Empty) +
                    ((_searchtype.Length > 0) ? "&t=" + _searchtype : "") + ((_searchkeyword.Length > 0) ? "&k=" + _searchkeyword : "");
            }
        }

        private string DetailListingAddtionalFunction()
        {
            string _temp = string.Empty;
            switch (Action)
            {
                case EnumAction.Add:
                    _temp = "action=" + (int)EnumAction.Add;
                    break;
                case EnumAction.Delete:
                    _temp = "?action=" + (int)EnumAction.Delete + "&id=" + (Key == "" ? this.Key : Key);
                    break;
                case EnumAction.Edit:
                    _temp = "action=" + (int)EnumAction.Edit + "&id=" + (Key == "" ? this.Key : Key);
                    break;
                case EnumAction.View:
                    _temp = "action=" + (int)EnumAction.View + "&id=" + (Key == "" ? this.Key : Key);
                    break;
                default:
                    _temp = string.Empty;
                    break;
            }
            return _temp;
        }

        private string AddReturnURLControl(string URL)
        {
            if (URL != string.Empty)
            {
                if (ReturnURLControl)
                {
                    return URL + (URL.IndexOf("?") != -1 ? "&" : "?") + "ReturnURL=" + Uri.EscapeDataString(_queryString.ToString());
                }
                else
                {
                    return URL;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        // 3
        public string GetUrl(EnumAction action, string key = "")
        {
            if (this.SetupKey == string.Empty)
            {
                return string.Empty;
            }

            string tempurl = string.Empty;

            switch (action)
            {
                case EnumAction.Add:
                    tempurl = _pageContext.ResolveUrl(this.DetailPage + "?action=" + (int)EnumAction.Add);
                    break;
                case EnumAction.Delete:
                    if (this.DeleteRedirectList)
                    {
                        tempurl = this.GenerateList + "&action=" + (int)EnumAction.Delete + "&id=" + (key == "" ? this.Key : key);
                    }
                    else
                    {
                        tempurl = _pageContext.ResolveUrl(this.DetailPage + "?action=" + (int)EnumAction.Delete + "&id=" + (key == "" ? this.Key : key));
                    }
                    break;
                case EnumAction.Edit:
                    tempurl = _pageContext.ResolveUrl(this.DetailPage + "?action=" + (int)EnumAction.Edit + "&id=" + (key == "" ? this.Key : key));
                    break;
                case EnumAction.View:
                    tempurl = _pageContext.ResolveUrl(this.DetailPage + "?action=" + (int)EnumAction.View + "&id=" + (key == "" ? this.Key : key));
                    break;
                case EnumAction.History:
                    tempurl = _pageContext.ResolveUrl(this.LogPage + "?id=" + (key == "" ? this.Key : key) + "&key=" + this.SetupKey + "&act=" + (int)this.Action + "&page=1");
                    break;
                default:
                    tempurl = _pageContext.ResolveUrl(this.ListPage);
                    break;
            }

            return AddReturnURLControl(tempurl);
        }

        // This Key must Same With The Resource Page
        private string _SetupKey = string.Empty;
        public string SetupKey
        {
            get { return _SetupKey; }
            set { _SetupKey = value; }
        }

        // 4
        public abstract string DisplayTitle { get; }

        // 8
        private bool _FunctionControl = true;
        public bool FunctionControl
        {
            get { return _FunctionControl; }
            set { _FunctionControl = value; }
        }

        // 9
        private bool _DeleteControl = true;
        public bool DeleteControl
        {
            get { return _DeleteControl; }
            set { _DeleteControl = value; }
        }

        private bool _customTitle = false;
        public bool CustomTitle
        {
            get { return _customTitle; }
            set { _customTitle = value; }
        }

        // 10
        public abstract string ListPage { get; }

        // 11
        public abstract string DetailPage { get; }

        // 12
        public abstract string LogPage { get; }

        // 16
        public abstract string PrintPage { get; }

        // 14
        public string GeneratePrintPage()
        {
            return _pageContext.ResolveUrl(PrintPage + "?type=" + this.SetupKey + "&itm1=" + this.Item1);
        }

        // 17
        public void AddItem(string id)
        {
            string[] idlist = this.Item1.Split(',');

            if (idlist.Contains(id))
            {
                return;
            }

            if (this._item1 == string.Empty)
            {
                this.Item1 = id;
            }
            else
            {
                this.Item1 += "," + id;
            }
        }

        // 18
        public void RemoveItem(string id)
        {
            string[] idlist = this.Item1.Split(',');

            if (idlist.Contains(id) == false)
            {
                return;
            }

            this.Item1 = string.Empty;

            foreach (string Str in idlist)
            {
                if (Str != id)
                {
                    if (this.Item1 == string.Empty)
                    {
                        this.Item1 = Str;
                    }
                    else
                    {
                        this.Item1 += "," + Str;
                    }
                }
            }
        }

        // 19
        public bool MatchID(string GridViewID)
        {
            bool result = false;

            string[] str = this.Item1.Split(',');
            if (str.Length > 0)
            {
                result = str.Contains(GridViewID);
            }

            return result;
        }

        // 30
        private bool _DeleteRedirectList = false;
        public bool DeleteRedirectList
        {
            get { return _DeleteRedirectList; }
            set { _DeleteRedirectList = value; }
        }

        // 31
        private bool _ReturnURLControl = false;
        public bool ReturnURLControl
        {
            get { return _ReturnURLControl; }
            set { _ReturnURLControl = value; }
        }

        public abstract void BindData();

        public string NormalTitle
        {
            get { return this.DisplayTitle; }
        }
    }
}
