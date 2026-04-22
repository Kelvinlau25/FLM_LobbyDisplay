using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Library.Oraclecls
{
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

    public Connection(string ConnectionStringName)
    {
        this.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionStringName].ToString();

        if (ConnectionString == string.Empty)
        {
            throw new Exception("Invalid Connection String Name That Set At Web Config");
        }

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
                // TODO: free other state (managed objects).
            }

            // TODO: free your own state (unmanaged objects).
            // TODO: set large fields to null.

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
        this.disposedValue = true;
    }

    #region IDisposable Support
    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
        // Do not change this code.  Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
}
