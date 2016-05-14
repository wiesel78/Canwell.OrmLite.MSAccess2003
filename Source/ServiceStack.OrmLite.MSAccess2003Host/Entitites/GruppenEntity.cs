﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.DataAnnotations;

namespace ServiceStack.OrmLite.MSAccess2003Host.Entitites
{
    [Alias("TBL_Gruppen")]
    public class GruppenEntity
    {
        [PrimaryKey]
        public int Gruppe_id { get; set; }
        public string Gruppebezeichnung { get; set; }
        public bool fl_IsAktiv { get; set; }
        public int TaktStep { get; set; }
        public int HourHeight { get; set; }
        public int? HeaderFontSize { get; set; }
    }
}
