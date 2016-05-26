using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;
using ServiceStack.DesignPatterns.Model;

namespace Canwell.OrmLite.MSAccess2003Tests.Entity
{
    [Alias("OptionTable")]
    public class OptionTableEntity : IHasIntId
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
