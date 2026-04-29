using System;
using System.Collections.Specialized;

namespace Library.Root.Control
{
    /// <summary>
    /// Log page base class (net8.0 migration — System.Web.UI.Page removed)
    /// QueryString supplied via constructor parameter.
    /// </summary>
    public abstract class LogBase
    {
        protected Library.Root.Other.GenericCollection<Library.Root.Object.Log> _list;
        private readonly NameValueCollection _queryString;

        protected LogBase(NameValueCollection queryString)
        {
            _queryString = queryString;
            this.BindKey();
            this.BindData();
        }

        private void BindKey()
        {
            foreach (string _query in _queryString)
            {
                if (!string.IsNullOrEmpty(_query))
                {
                    switch (_query)
                    {
                        case "id":
                            this._key = Uri.UnescapeDataString(_queryString["id"]);
                            break;
                        case "key":
                            this._setupKey = _queryString["key"];
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
            get { return this.LogPage + "?id=" + Uri.EscapeDataString(this.Key) + "&key=" + this.SetupKey + "&page=" + this.PageNo; }
        }
    }
}
