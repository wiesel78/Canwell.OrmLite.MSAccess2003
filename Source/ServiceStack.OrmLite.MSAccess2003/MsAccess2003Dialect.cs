using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceStack.OrmLite.MSAccess2003
{
    public static class MsAccess2003Dialect
    {
        public static IOrmLiteDialectProvider Provider { get { return MsAccess2003OrmLiteDialectProvider.Instance; } }
    }
}
