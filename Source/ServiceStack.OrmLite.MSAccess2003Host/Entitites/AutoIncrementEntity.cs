using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace ServiceStack.OrmLite.MSAccess2003Host.Entitites
{
    [Alias("AutoIncrementTable")]
    public class AutoIncrementEntity
    {
        [AutoIncrement]
        [PrimaryKey]
        public int Id { get; set; }

        public string TextColumn { get; set; }
    }
}
