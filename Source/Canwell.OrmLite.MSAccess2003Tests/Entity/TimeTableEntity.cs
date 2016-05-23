using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Tests.Entity
{
    [Alias("TimeTable")]
    public class TimeTableEntity
    {
        public DateTime DateTimeColumn { get; set; }
        public TimeSpan TimeSpanColumn { get; set; }

    }
}
