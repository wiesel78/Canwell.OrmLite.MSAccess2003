using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Tests.Entity
{
    [Alias("AddColumnTable")]
    public class ExtendedColumnTableEntity : BasicColumnTableEntity
    {
        public string Phone { get; set; }
    }
}
