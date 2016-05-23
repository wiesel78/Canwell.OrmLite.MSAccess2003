using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Tests.Entity
{
    [Alias("AnnotationTable")]
    public class AnnotationTableEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [Default(typeof(bool), "0")]
        public bool DefaultBooleanColumn { get; set; }

        [Index(Unique = true)]
        public string UniqueStringColumn { get; set; }
    }
}
