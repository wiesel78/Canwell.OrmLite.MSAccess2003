using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite.MSAccess2003Host.Dtos;

namespace ServiceStack.OrmLite.MSAccess2003Host.Entitites
{
    [Alias("ExtendedTypes")]
    public class ExtendedTypesEntity
    {
        public Guid Id { get; set; }
        public byte[] BinaryColumn { get; set; }

        public string[] StringColumn { get; set; }

        public TestObject TestObjectColumn { get; set; }


    }
}
