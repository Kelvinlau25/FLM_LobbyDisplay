namespace Library.Control.Base
{
    public abstract class UserControl : System.Web.UI.UserControl
    {
        private bool _editmode = false;
        public bool EditMode
        {
            get { return _editmode; }
            set { _editmode = value; }
        }

        private string _ValidationGroup;
        public string ValidationGroup
        {
            get { return _ValidationGroup; }
            set { _ValidationGroup = value; }
        }

        public abstract void Clear();
        public abstract bool AddFuntion();
        public abstract bool DeleteFuntion();
        public abstract bool EditFuntion();
        public abstract bool PermissionFuntion();
        public abstract void View();
        public abstract void AdminProceed();
        public abstract int getrowcount();
        public abstract void DepartProceed();
        public abstract void ActionTakenProceed();
        public abstract void SendMailtoPICLevelOne();
        public abstract string SetPICStatus();
        public abstract void SetPICStatusDate();
        public abstract void SetPICButton();
        public abstract void SetPICButtonForPIC();
        public abstract bool validation();
        public abstract bool validationForSaveButton();
        public abstract bool ShowOrHide();
    }
}
