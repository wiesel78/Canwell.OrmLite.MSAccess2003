using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canwell.OrmLite.MSAccess2003Tests.Entity;

namespace Canwell.OrmLite.MSAccess2003Tests.Mock.Settings
{
    public static class TableList
    {
        public static Type[] Types = new Type[]
        {
            typeof(NumberTableEntity),
            typeof(TextTableEntity)
        };
    }
}
