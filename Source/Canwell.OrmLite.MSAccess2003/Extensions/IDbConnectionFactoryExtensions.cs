using System.Data.OleDb;
using ServiceStack.OrmLite;

namespace Canwell.OrmLite.MSAccess2003.Extensions
{
    public static class IDbConnectionFactoryExtensions
    {
        public static OleDbConnection GetOleDbConnection(this IDbConnectionFactory db)
        {
            var ormConnectionFactory = db as OrmLiteConnectionFactory;
            if (ormConnectionFactory == null)
                return null;

            var ormConnection = ormConnectionFactory.CreateDbConnection() as OrmLiteConnection;
            if (ormConnection == null || ormConnection.DbConnection == null)
                return null;

            var accessConnection = ormConnection.DbConnection as MsAccess2003DbConnection;
            if (accessConnection == null)
                return null;

            return accessConnection.Connection;
        }
    }
}
