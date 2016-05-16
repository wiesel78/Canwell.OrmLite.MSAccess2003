using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Canwell.OrmLite.MSAccess2003Tests.Mock.Settings
{
    public static class ConnectionStringEnum
    {
        public const string WithPassword = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Mode=Share Deny None;Jet OLEDB:Database Password={1}";

        public const string WithoutPassword = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Mode=Share Deny None;Jet OLEDB:Database";
    }
}
