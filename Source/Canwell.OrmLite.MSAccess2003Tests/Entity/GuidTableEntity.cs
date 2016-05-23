using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Tests.Entity
{
    [Alias("GuidTable")]
    public class GuidTableEntity
    {
        public Guid GuidColumn { get; set; }
    }
}
