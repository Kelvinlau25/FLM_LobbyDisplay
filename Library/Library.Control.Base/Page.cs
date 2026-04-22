using System.Collections.Generic;

namespace Library.Control.Base
{
    public abstract class Page : System.Web.UI.Page
    {
        private System.Web.UI.WebControls.PlaceHolder _controlPanel = null;
        public List<string> _list = new List<string>();

        public System.Web.UI.WebControls.PlaceHolder ControlPanel
        {
            get { return _controlPanel; }
            set { _controlPanel = value; }
        }

        public void Remove(System.Web.UI.Control Ctrl)
        {
            Ctrl.Visible = false;
        }

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
