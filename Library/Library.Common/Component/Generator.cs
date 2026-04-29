using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;

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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table>");
            // Header row
            sb.AppendLine("<tr>");
            foreach (FieldSet itm in this._setting)
            {
                sb.AppendLine($"<th>{WebUtility.HtmlEncode(itm.Title)}</th>");
            }
            sb.AppendLine("</tr>");
            // Data rows
            foreach (DataRow row in this._data.Rows)
            {
                sb.AppendLine("<tr>");
                foreach (FieldSet itm in this._setting)
                {
                    string cellValue = row[itm.Field] == DBNull.Value ? string.Empty : row[itm.Field].ToString();
                    sb.AppendLine($"<td>{WebUtility.HtmlEncode(cellValue)}</td>");
                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</table>");
            return sb.ToString();
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
