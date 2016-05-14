using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace ServiceStack.OrmLite.MSAccess2003
{
    public class MsAccess2003DbDataParameter : IDbDataParameter
    {
        public OleDbParameter Parameter { get; set; }

        public MsAccess2003DbDataParameter(OleDbParameter parameter)
        {
            Parameter = parameter;
        }

        public byte Precision
        {
            get { return Parameter.Precision; }
            set { Parameter.Precision = value; }
        }

        public byte Scale
        {
            get { return Parameter.Scale; }
            set { Parameter.Scale = value; }
        }

        public int Size
        {
            get { return Parameter.Size; }
            set { Parameter.Size = value; }
        }

        public DbType DbType
        {
            get { return Parameter.DbType; }
            set { Parameter.DbType = value; }
        }

        public ParameterDirection Direction
        {
            get { return Parameter.Direction; }
            set { Parameter.Direction = value; }
        }

        public bool IsNullable
        {
            get { return Parameter.IsNullable; }
        }

        public string ParameterName
        {
            get { return Parameter.ParameterName; }
            set { Parameter.ParameterName = value; }
        }

        public string SourceColumn
        {
            get { return Parameter.SourceColumn; }
            set { Parameter.SourceColumn = value; }
        }

        public DataRowVersion SourceVersion
        {
            get { return Parameter.SourceVersion; }
            set { Parameter.SourceVersion = value; }
        }

        public object Value
        {
            get { return Parameter.Value; }
            set { Parameter.Value = value; }
        }
    }
}
