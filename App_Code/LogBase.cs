using System;

namespace Control
{
    public abstract class LogBase : Library.Root.Control.LogBase
    {
        public string LogTable
        {
            get { return (string)GetGlobalResourceObject("Log", base.SetupKey); }
        }

        public override string LogPage
        {
            get { return (string)GetGlobalResourceObject("ListPage", "History"); }
        }

        public override string LogTitle
        {
            get { return (string)GetGlobalResourceObject("Title", base.SetupKey); }
        }

        public string SortDesc
        {
            get
            {
                try
                {
                    return (string)GetGlobalResourceObject("SortDesc", base.SetupKey);
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        protected override void BindData()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
    }
}
