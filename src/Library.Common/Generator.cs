using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace Library.common
{
    /// <summary>
    /// Field-set collection used to describe how columns of a <see cref="DataTable"/>
    /// should be projected (renamed, re-ordered, typed) before display.
    ///
    /// NOTE (.NET 8 migration): the original Web Forms version included a
    /// <c>ToString()</c> override that rendered an HTML table by binding the data
    /// to a <c>System.Web.UI.WebControls.DataGrid</c>. That overload has
    /// been removed because System.Web.UI is not available on .NET 8. Render the
    /// projected <see cref="Data"/> with a Razor view / tag helper / partial in the
    /// host application instead.
    /// </summary>
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
        /// Apply the configured field settings to <see cref="Data"/>: re-order the
        /// columns, rename them to the configured titles, and remove any columns
        /// that were not registered. The resulting <see cref="DataTable"/> can then
        /// be rendered by the caller (e.g. a Razor view).
        /// </summary>
        public DataTable Project()
        {
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

            return this._data;
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
