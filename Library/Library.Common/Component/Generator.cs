using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI;

namespace Library.common
{
    public class Generator : ICollection<FieldSet>
    {
        #region Properties
        private DataTable _data;
        /// <summary>
        /// Data Source
        /// </summary>
        public DataTable Data
        {
            get { return _data; }
            set { _data = value; }
        }

        private List<FieldSet> _setting = new List<FieldSet>();
        /// <summary>
        /// Field Setting Collection
        /// </summary>
        public List<FieldSet> Setting
        {
            get { return _setting; }
        }
        #endregion

        /// <summary>
        /// initial the data
        /// </summary>
        public Generator(DataTable data)
        {
            this._data = data;
            this._setting = new List<FieldSet>();
        }

        public void AddField(string Field, string Title, EnumLib.DataType Type)
        {
            this._setting.Add(new FieldSet(Field, Title, Type));
        }

        public void Add(FieldSet item)
        {
            this._setting.Add(item);
        }

        /// <summary>
        /// Generate the HTML
        /// </summary>
        public override string ToString()
        {
            using (StringWriter _sw = new StringWriter())
            {
                using (HtmlTextWriter _htw = new HtmlTextWriter(_sw))
                {
                    System.Web.UI.WebControls.DataGrid _dg = new System.Web.UI.WebControls.DataGrid();
                    int _counter = -1;

                    // Field setting
                    foreach (FieldSet Itm in this._setting)
                    {
                        _counter += 1;
                        this._data.Columns[Itm.Field].SetOrdinal(_counter);
                        this._data.Columns[Itm.Field].AllowDBNull = true;
                        this._data.Columns[Itm.Field].ColumnName = Itm.Title;
                    }

                    // Remove unwanted column
                    _counter += 1;
                    short _totalColumnCount = (short)(this.Data.Columns.Count - 1);
                    for (int temp = _counter; temp <= _totalColumnCount; temp++)
                    {
                        this.Data.Columns.RemoveAt(_counter);
                    }

                    _dg.DataSource = this.Data;
                    _dg.DataBind();
                    _dg.RenderControl(_htw);

                    return _sw.ToString();
                }
            }
        }

        public void Clear()
        {
            this._setting.Clear();
        }

        public bool Contains(FieldSet item)
        {
            return this._setting.Contains(item);
        }

        public void CopyTo(FieldSet[] array, int arrayIndex)
        {
            this._setting.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this._setting.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(FieldSet item)
        {
            return this._setting.Remove(item);
        }

        public int IndexOf(FieldSet item)
        {
            return this._setting.IndexOf(item);
        }

        public void Insert(int index, FieldSet item)
        {
            this._setting.Insert(index, item);
        }

        public FieldSet this[int index]
        {
            get { return this._setting[index]; }
            set { this._setting[index] = value; }
        }

        public void RemoveAt(int index)
        {
            this._setting.RemoveAt(index);
        }

        public IEnumerator<FieldSet> GetEnumerator()
        {
            return this._setting.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._setting.GetEnumerator();
        }
    }
}
