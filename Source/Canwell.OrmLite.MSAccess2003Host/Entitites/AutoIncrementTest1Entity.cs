using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Host.Entitites
{
    [Alias("TestTable")]
    public class AutoIncrementTest1Entity
    {
        [PrimaryKey]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}
