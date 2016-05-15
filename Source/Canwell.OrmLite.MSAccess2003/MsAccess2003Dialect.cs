using ServiceStack.OrmLite;

namespace Canwell.OrmLite.MSAccess2003
{
    public static class MsAccess2003Dialect
    {
        public static IOrmLiteDialectProvider Provider { get { return MsAccess2003OrmLiteDialectProvider.Instance; } }
    }
}
