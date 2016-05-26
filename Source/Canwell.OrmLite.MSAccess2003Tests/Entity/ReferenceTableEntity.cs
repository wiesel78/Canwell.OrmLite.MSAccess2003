using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Tests.Entity
{
    [Alias("ReferenceTable")]
    public class ReferenceTableEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        [References(typeof(OptionTableEntity))]
        public int? OptionsId { get; set; }
    }
}
