using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace Canwell.OrmLite.MSAccess2003Tests.Entity
{
    [Alias("TextTable")]
    public class TextTableEntity
    {
        public string TextColumn { get; set; }
        public char CharColumn { get; set; }
    }
}
