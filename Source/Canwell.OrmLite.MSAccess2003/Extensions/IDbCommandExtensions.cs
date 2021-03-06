﻿using System.Data;
using System.Data.OleDb;

namespace Canwell.OrmLite.MSAccess2003.Extensions
{
    public static class IDbCommandExtensions
    {
        public static OleDbConnection GetOleDbConnection(this IDbCommand command)
        {
            var accessCommand = command as MsAccess2003DbCommand;
            if (accessCommand == null)
                return null;

            var accessConnection = accessCommand.Connection as MsAccess2003DbConnection;
            if (accessConnection == null)
                return null;

            return accessConnection.Connection;
        }
    }
}
