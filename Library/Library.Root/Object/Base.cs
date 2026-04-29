using System;

namespace Library.Root.Object
{
    /// <summary>
    /// Must Inheritance Common Object (net8.0 migration — System.Web.HttpContext removed)
    /// Session/request values replaced with String.Empty; web layer handles user context.
    /// -------------------------------------------
    /// C.C.Yeon      25 April 2011  initial Version
    /// </summary>
    public abstract class Base
    {
        public Base()
        {
            _id = 0;
            _createdby = string.Empty;
            _createddate = DateTime.Now;
            _createdloc = string.Empty;
            _Updatedby = string.Empty;
            _updatedDate = DateTime.Now;
            _UpdatedLoc = string.Empty;
        }

        private int _id = 0;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _rectype = string.Empty;
        public string Record_Type
        {
            get { return _rectype; }
            set { _rectype = value; }
        }

        private string _createdby = string.Empty;
        public string CreatedBy
        {
            get { return _createdby; }
            set { _createdby = value; }
        }

        private DateTime _createddate = DateTime.Now;
        public DateTime CreatedDate
        {
            get { return _createddate; }
            set { _createddate = value; }
        }

        private string _createdloc = string.Empty;
        public string CreatedLoc
        {
            get { return _createdloc; }
            set { _createdloc = value; }
        }

        private string _Updatedby = string.Empty;
        public string UpdatedBy
        {
            get { return _Updatedby; }
            set { _Updatedby = value; }
        }

        private DateTime _updatedDate = DateTime.Now;
        public DateTime UpdatedDate
        {
            get { return _updatedDate; }
            set { _updatedDate = value; }
        }

        private string _UpdatedLoc = string.Empty;
        public string UpdatedLoc
        {
            get { return _UpdatedLoc; }
            set { _UpdatedLoc = value; }
        }
    }
}
