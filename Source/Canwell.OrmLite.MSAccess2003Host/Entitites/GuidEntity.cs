using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Host.Entitites
{
    [Alias("GuidTable")]
    public class GuidEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}
