using System;
using Canwell.OrmLite.MSAccess2003Host.Dtos;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Host.Entitites
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
