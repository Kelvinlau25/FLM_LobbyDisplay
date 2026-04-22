using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Library.Oraclecls
{
    /// <summary>
    /// ADO.NET Oracle connection wrapper using Oracle.ManagedDataAccess.Core.
    /// Connection string is supplied directly by the caller (typically resolved
    /// from <c>IConfiguration</c> in the host application).
    /// </summary>
    public abstract class Connection : IDisposable
    {
        protected OracleConnection _con;
        protected OracleCommand _cmd;
        protected OracleDataReader _rdr;
        protected OracleTransaction _tran;

        private string _constr = string.Empty;
        public string ConnectionString
        {
            get { return _constr; }
            set { _constr = value; }
        }

        /// <summary>
        /// Create a connection from a fully-qualified Oracle connection string.
        /// </summary>
        /// <param name="connectionString">A complete Oracle connection string (e.g. resolved from configuration).</param>
        protected Connection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("Oracle connection string must not be null or empty.", nameof(connectionString));
            }

            this.ConnectionString = connectionString;

            this._con = new OracleConnection(this.ConnectionString);
            this._con.Open();
            this._cmd = _con.CreateCommand();
            this._tran = _con.BeginTransaction();
        }

        public string Status
        {
            get
            {
                if (_con != null)
                {
                    return _con.State.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Commit all the transaction
        /// </summary>
        public void Commit()
        {
            _tran.Commit();
        }

        /// <summary>
        /// Rollback all the transaction
        /// </summary>
        public void RollBack()
        {
            _tran.Rollback();
        }

        private bool disposedValue = false; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (_rdr != null)
                    {
                        _rdr.Dispose();
                    }

                    if (_tran != null)
                    {
                        _tran.Dispose();
                    }

                    if (_cmd != null)
                    {
                        _cmd.Dispose();
                    }

                    if (_con != null)
                    {
                        if (_con.State == ConnectionState.Open)
                        {
                            _con.Close();
                        }

                        _con.Dispose();
                    }
                }
            }
            this.disposedValue = true;
        }

        #region IDisposable Support
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
