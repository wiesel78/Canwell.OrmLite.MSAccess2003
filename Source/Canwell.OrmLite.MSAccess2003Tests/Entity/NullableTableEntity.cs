using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Tests.Entity
{
    [Alias("NullableTable")]
    public class NullableTableEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public int? NullableIntegerColumn { get; set; }
        public Guid? NullableGuidColumn { get; set; }
        public DateTime? NullableDateTimeColumn { get; set; }
    }
}
