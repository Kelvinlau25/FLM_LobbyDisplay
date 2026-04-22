namespace Library.Root.Object
{
    public class Log
    {
        private int _ID = 0;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private string _keyField = string.Empty;
        public string KeyField
        {
            get { return _keyField; }
            set { _keyField = value; }
        }

        private string _KeyDesc = string.Empty;
        public string KeyDesc
        {
            get { return _KeyDesc; }
            set { _KeyDesc = value; }
        }

        private string _keyValue = string.Empty;
        public string KeyValue
        {
            get { return _keyValue; }
            set { _keyValue = value; }
        }

        private string _fieldname = string.Empty;
        public string FieldName
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }

        private string _b4Update = string.Empty;
        public string B4Update
        {
            get { return _b4Update; }
            set { _b4Update = value; }
        }

        private string _afUpdate = string.Empty;
        public string AFUpdate
        {
            get { return _afUpdate; }
            set { _afUpdate = value; }
        }

        private string _updatedby = string.Empty;
        public string UpdateBy
        {
            get { return _updatedby; }
            set { _updatedby = value; }
        }

        private string _updateDate = string.Empty;
        public string UpdatedDate
        {
            get { return _updateDate; }
            set { _updateDate = value; }
        }
    }
}
