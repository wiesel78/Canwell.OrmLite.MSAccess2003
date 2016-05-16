using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Tests.Entity
{
    [Alias("NumberTable")]
    public class NumberTableEntity
    {
        public byte ByteColumn { get; set; }
        public short ShortColumn { get; set; }
        public int IntegerColumn { get; set; }
        public long LongIntegerColumn { get; set; }
        public double DoubleColumn { get; set; }
    }
}
