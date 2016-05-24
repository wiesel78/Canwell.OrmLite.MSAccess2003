using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Canwell.OrmLite.MSAccess2003Tests.Entity;

namespace Canwell.OrmLite.MSAccess2003Tests.Mock.Settings
{
    public static class TableList
    {
        public static readonly Type[] Types = new Type[]
        {
            typeof(NumberTableEntity),
            typeof(TimeTableEntity),
            typeof(TextTableEntity),
            typeof(NullableTableEntity),
            typeof(GuidTableEntity),
            typeof(ExtendedColumnTableEntity),
            typeof(BasicColumnTableEntity),
            typeof(AnnotationTableEntity)
        };
    }
}
