using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Tests.Entity
{
    [Alias("PrimaryGuid")]
    public class PrimaryGuidEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}
