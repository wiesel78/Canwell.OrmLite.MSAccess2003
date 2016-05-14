using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace ServiceStack.OrmLite.MSAccess2003Host.Entitites
{
    [Alias("TestTypeTable")]
    public class TypeEntity
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public char CharColumn { get; set; }
        public string TextColumn { get; set; }
        public bool BooleanColumn { get; set; }
        public byte ByteColumn { get; set; }
        public short ShortColumn { get; set; }
        public int IntegerColumn { get; set; }
        public long LongIntegerColumn { get; set; }
        public double DoubleColumn { get; set; }
        public DateTime DateColumn { get; set; }
    }
}
