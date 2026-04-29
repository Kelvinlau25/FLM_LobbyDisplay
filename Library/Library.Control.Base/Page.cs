namespace Library.Control.Base
{
    // net8.0 migration: System.Web.UI.Page inheritance removed.
    // This is now a plain abstract base class for page-level properties.
    public abstract class Page
    {
        private short _Action;
        public short Action
        {
            get { return _Action; }
            set { _Action = value; }
        }

        private short _CurrentStep;
        public short CurrentStep
        {
            get { return _CurrentStep; }
            set { _CurrentStep = value; }
        }

        private string _Worksno;
        public string Worksno
        {
            get { return _Worksno; }
            set { _Worksno = value; }
        }

        private string _CelNo;
        public string CelNo
        {
            get { return _CelNo; }
            set { _CelNo = value; }
        }

        private string _CompCode;
        public string CompCode
        {
            get { return _CompCode; }
            set { _CompCode = value; }
        }

        private string _Reqno;
        public string Reqno
        {
            get { return _Reqno; }
            set { _Reqno = value; }
        }
    }
}
