using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Tests.Entity
{
    [Alias("AddColumnTable")]
    public class BasicColumnTableEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public string EMail { get; set; }
    }
}
