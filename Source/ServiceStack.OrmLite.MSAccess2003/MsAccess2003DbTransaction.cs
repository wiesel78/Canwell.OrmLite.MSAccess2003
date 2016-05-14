using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace ServiceStack.OrmLite.MSAccess2003
{
    public class MsAccess2003DbTransaction : IDbTransaction
    {
        public OleDbTransaction Transaction { get; set; }

        public MsAccess2003DbTransaction(OleDbTransaction transaction)
        {
            Transaction = transaction;
        }

        public void Commit()
        {
            Transaction.Commit();
        }
        public IDbConnection Connection
        {
            get { return new MsAccess2003DbConnection(Transaction.Connection); }
        }

        public IsolationLevel IsolationLevel
        {
            get { return Transaction.IsolationLevel; }
        }

        public void Rollback()
        {
            Transaction.Rollback();
        }

        public void Dispose()
        {
            Transaction.Dispose();
        }
    }
}
