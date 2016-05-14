using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace ServiceStack.OrmLite.MSAccess2003
{
    public class MsAccess2003DbCommand : IDbCommand
    {
        private OleDbCommand Command { get; set; }

        public MsAccess2003DbCommand(OleDbCommand command)
        {
            Command = command;
        }

        public void Cancel()
        {
            Command.Cancel();
        }

        public string CommandText
        {
            get { return Command.CommandText; }
            set { Command.CommandText = value; }
        }

        public int CommandTimeout
        {
            get { return Command.CommandTimeout; }
            set { Command.CommandTimeout = value; }
        }

        public CommandType CommandType
        {
            get { return Command.CommandType; }
            set { Command.CommandType = value; }
        }

        public IDbConnection Connection
        {
            get
            {
                return new MsAccess2003DbConnection(Command.Connection);
            }
            set
            {
                Command.Connection = new OleDbConnection(value.ConnectionString);
            }
        }

        public IDbDataParameter CreateParameter()
        {
            return new MsAccess2003DbDataParameter(Command.CreateParameter());
        }

        public int ExecuteNonQuery()
        {
            return Command.ExecuteNonQuery();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return new MsAccess2003DbDataReader(Command.ExecuteReader(behavior));
        }

        public IDataReader ExecuteReader()
        {
            return new MsAccess2003DbDataReader(Command.ExecuteReader());
        }

        public object ExecuteScalar()
        {
            return Command.ExecuteScalar();
        }

        public IDataParameterCollection Parameters
        {
            get { return new MsAccess2003DbDataParameterCollection(Command.Parameters); }
        }

        public void Prepare()
        {
            Command.Prepare();
        }

        public IDbTransaction Transaction
        {
            get
            {
                return new MsAccess2003DbTransaction(Command.Transaction);
            }
            set
            {
                Command.Transaction = (OleDbTransaction)value;
            }
        }

        public UpdateRowSource UpdatedRowSource
        {
            get { return Command.UpdatedRowSource; }
            set { Command.UpdatedRowSource = value; }
        }

        public void Dispose()
        {
            Command.Dispose();
        }
    }
}
