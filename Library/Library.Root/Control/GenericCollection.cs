using System.Collections.Generic;

namespace Library.Root.Other
{
    /// <summary>
    /// Generic Collection
    /// ----------------------------------------------
    /// C.C.Yeon      25 April 2011    initial version
    /// </summary>
    public class GenericCollection<T>
    {
        public GenericCollection()
        {
            _data = new List<T>();
            _counter = 0;
        }

        private List<T> _data;
        public List<T> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private int _counter;
        public int TotalRow
        {
            get { return _counter; }
            set { _counter = value; }
        }
    }

    /// <summary>
    /// Generic Total Collection handler when the data was need total Calculation
    /// -------------------------------------------------------------------------
    /// C.C.Yeon      19 May 2011    initial version
    /// </summary>
    public class TotalCollection<T>
    {
        public TotalCollection()
        {
            _data = new List<T>();
            _counter = new List<T>();
        }

        private List<T> _data;
        public List<T> Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private List<T> _counter;
        public List<T> TotalData
        {
            get { return _counter; }
            set { _counter = value; }
        }
    }
}
