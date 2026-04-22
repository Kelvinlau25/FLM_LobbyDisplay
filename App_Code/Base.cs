namespace Control
{
    /// <summary>
    /// Handler All Page Common Function 
    /// 1 ) Retrieve and Determine Key
    /// 2 ) Retrieve and Determine Action(Insert / Update / Delete)
    /// 3 ) Generate (Insert / Edit / Delete / List / View ) URL Based on the Setup Key
    /// 4 ) Generete Title Based on the Setup Key
    /// 5 ) Generate Action Desc 
    /// 6 ) Retrieve and Determine Sort Field , Sort Direction 
    /// 7 ) Generate the List View URL (Include Sort Field , Sort Value and Page No )
    /// 8 ) Function Control property , Default = true , if false all the generate list will not auto generate
    /// 9 ) Delete Control property , Default = true, if false then the show deleted Check box will be disappear.
    /// -------------------------------------------------------------------------------
    /// C.C.Yeon    25 April 2011   initial Version
    /// C.C.Yeon    12 May   2011   Add FunctionControl Property
    /// </summary>
    public abstract class Base : Library.Root.Control.Base
    {
        public abstract void BindData();

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            this.BindData();
        }

        /// <summary>
        /// Retrieve the Detail Path
        /// </summary>
        public override string DetailPage
        {
            get { return (string)GetGlobalResourceObject("DetailPage", this.SetupKey); }
        }

        /// <summary>
        /// Retrieve Title
        /// </summary>
        public override string DisplayTitle
        {
            get { return (string)GetGlobalResourceObject("Title", this.SetupKey); }
        }

        /// <summary>
        /// Retrieve List Page
        /// </summary>
        public override string ListPage
        {
            get { return (string)GetGlobalResourceObject("ListPage", this.SetupKey); }
        }

        /// <summary>
        /// Retrieve Log Page
        /// </summary>
        public override string LogPage
        {
            get { return (string)GetGlobalResourceObject("ListPage", "History"); }
        }

        /// <summary>
        /// Retrieve other path based on the key
        /// </summary>
        protected string RetrieveOthersDetail(string key)
        {
            return (string)GetGlobalResourceObject("DetailPage", key);
        }

        public string DateTimeFormat
        {
            get { return "dd / MMM / yyyy hh:mm:ss"; }
        }

        public int UserID
        {
            get { return Session["gstrUserID"] != null ? System.Convert.ToInt32(Session["gstrUserID"]) : 0; }
        }

        /// <summary>
        /// Retrieve Print Page Path
        /// </summary>
        public override string PrintPage
        {
            get { return (string)GetGlobalResourceObject("ListPage", "Print"); }
        }
    }
}
