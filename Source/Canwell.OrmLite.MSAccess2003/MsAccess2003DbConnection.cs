using System.Data;
using System.Data.OleDb;

namespace Canwell.OrmLite.MSAccess2003
{
    public class MsAccess2003DbConnection : IDbConnection
    {
        public OleDbConnection Connection { get; set; }

        public MsAccess2003DbConnection(OleDbConnection connection)
        {
            Connection = connection;
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return new MsAccess2003DbTransaction(Connection.BeginTransaction(il));
        }

        public IDbTransaction BeginTransaction()
        {
            return new MsAccess2003DbTransaction(Connection.BeginTransaction());
        }

        public void ChangeDatabase(string databaseName)
        {
            Connection.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            Connection.Close();
        }

        public string ConnectionString
        {
            get { return Connection.ConnectionString; }
            set { Connection.ConnectionString = value; }
        }

        public int ConnectionTimeout
        {
            get { return Connection.ConnectionTimeout; }
        }

        public IDbCommand CreateCommand()
        {
            return new MsAccess2003DbCommand(Connection.CreateCommand());
        }

        public string Database
        {
            get { return Connection.Database; }
        }

        public void Open()
        {
            Connection.Open();
        }

        public ConnectionState State
        {
            get { return Connection.State; }
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
