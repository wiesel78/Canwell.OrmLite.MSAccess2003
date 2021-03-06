﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using Canwell.OrmLite.MSAccess2003.Extensions;
using ServiceStack.Common;
using ServiceStack.OrmLite;

namespace Canwell.OrmLite.MSAccess2003
{
    public class MsAccess2003OrmLiteDialectProvider : OrmLiteDialectProviderBase<MsAccess2003OrmLiteDialectProvider>
    {
        public static MsAccess2003OrmLiteDialectProvider Instance = new MsAccess2003OrmLiteDialectProvider();

        public OleDbConnection OriginConnection { get; set; }

        public MsAccess2003OrmLiteDialectProvider()
        {
            base.StringLengthUnicodeColumnDefinitionFormat = "TEXT";
            base.StringLengthNonUnicodeColumnDefinitionFormat = "TEXT";
            base.StringLengthColumnDefinitionFormat = "TEXT";
            base.StringColumnDefinition = "TEXT";
            base.BoolColumnDefinition = "YESNO";
            base.BlobColumnDefinition = "TEXT";
            base.TimeColumnDefinition = "TEXT";
            base.DefaultValueFormat = " DEFAULT {0}";
            
            base.LongColumnDefinition = base.IntColumnDefinition;

            base.InitColumnTypeMap();
        }

        public override IDbConnection CreateConnection(string filePath, Dictionary<string, string> options)
        {
            var path = GetFilePath(filePath);
            var autoCreate = options == null || (!options.ContainsKey("AutoCreateDatabase") || bool.Parse(options["AutoCreateDatabase"]));


            // create mdb if filePath mdb is not exist
            if (!File.Exists(path) && autoCreate)
            {
                var cat = new ADOX.CatalogClass();
                cat.Create(filePath);
            }

            OriginConnection = new OleDbConnection(filePath);
            return new MsAccess2003DbConnection(OriginConnection);
        }

        /// <summary>
        /// Extract FilePath from connection string
        /// </summary>
        /// <param name="connectionString">connection string to mdb database</param>
        /// <returns>filepathstring to the potencially mdb</returns>
        public string GetFilePath(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return string.Empty;

            var options = connectionString.Split(';');

            if (options.Length == 0)
                return string.Empty;

            var pathOption = options.First(o => o.Contains("Data Source"));

            if (pathOption == null || !pathOption.Contains("="))
                return string.Empty;

            return pathOption.Split('=')[1];
        }

        public override SqlExpressionVisitor<T> ExpressionVisitor<T>()
        {
            return new MsAccess2003SqlExpressionVisitor<T>();
        }

        public override string GetQuotedTableName(string tableName)
        {
            return string.Format("`{0}`", NamingStrategy.GetTableName(tableName));
        }

        public override string GetQuotedColumnName(string columnName)
        {
            return string.Format("`{0}`", NamingStrategy.GetColumnName(columnName));
        }

        public override string GetQuotedName(string name)
        {
            return string.Format("\"{0}\"", name);
        }

        public override string GetQuotedValue(object value, Type fieldType)
        {
            if (fieldType == typeof(Guid))
            {
                if (value != null)
                    return string.Format("{0}{1}{2}", "#'", value.ToString(), "'#");
            }
            else if (fieldType == typeof(DateTime))
            {
                if (value != null)
                {
                    var d = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                    return string.Format("{0}{1}{0}", "#", d);
                }
            }

            return base.GetQuotedValue(value, fieldType);
        }

        public override bool DoesTableExist(IDbCommand db, string tableName)
        {
            // in IDbCommand findet sich eine Connection Eigenschaft, die in oledbconnection gecasted werden kann
            var oleDbConnection = db.GetOleDbConnection();
            if (oleDbConnection == null)
                return false;

            var exists = false;
            var wasOpened = true;

            // falls die 
            if (oleDbConnection.State != System.Data.ConnectionState.Open)
            {
                oleDbConnection.Open();
                wasOpened = false;
            }

            // Tabellenschema nach dem Tabellennamen durchsuchen
            exists = oleDbConnection.GetSchema("Tables", new string[4] { null, null, tableName, "TABLE" }).Rows.Count > 0;

            if (oleDbConnection.State != System.Data.ConnectionState.Closed && !wasOpened)
                oleDbConnection.Close();

            return exists;
        }


        public override string GetColumnDefinition(string fieldName, Type fieldType,
            bool isPrimaryKey, bool autoIncrement, bool isNullable,
            int? fieldLength, int? scale, string defaultValue)
        {
            string fieldDefinition;

            if (fieldType == typeof(string))
            {
                fieldDefinition = string.Format(StringLengthColumnDefinitionFormat, fieldLength.GetValueOrDefault(DefaultStringLength));
            }
            else
            {
                if (!DbTypeMap.ColumnTypeMap.TryGetValue(fieldType, out fieldDefinition))
                {
                    fieldDefinition = this.GetUndefinedColumnDefinition(fieldType, fieldLength);
                }
            }

            if (fieldDefinition == IntColumnDefinition && autoIncrement)
                fieldDefinition = "";

            var sql = new StringBuilder();
            sql.AppendFormat("{0} {1}", GetQuotedColumnName(fieldName), fieldDefinition);

            //if (autoIncrement)
            //    sql.Append(string.Format(" {0} ", "AUTOINCREMENT"));

            if (isPrimaryKey)
            {
                if (autoIncrement)
                {
                    sql.Append(GetAutoIncrementDefinition(fieldType));
                }
                //else
                //{



                sql.Append(" PRIMARY KEY");
                //}

            }
            else
            {
                if (isNullable)
                {
                    sql.Append(" NULL");
                }
                else
                {
                    sql.Append(" NOT NULL");
                }
            }

            if (!string.IsNullOrEmpty(defaultValue))
            {
                sql.AppendFormat(DefaultValueFormat, defaultValue);
            }

            return sql.ToString();
        }

        public override string ToAlterColumnStatement(Type modelType, FieldDefinition fieldDef)
        {
            var baseString = base.ToAlterColumnStatement(modelType,fieldDef);

            if (fieldDef.FieldType == typeof (int))
            {
                baseString = baseString.ReplaceAll("AUTOINCREMENT  PRIMARY KEY", "AUTOINCREMENT");
            }
            else if (fieldDef.FieldType == typeof(Guid))
            {
                baseString = baseString.ReplaceAll("GUID DEFAULT GenGUID()  PRIMARY KEY", " GUID DEFAULT GenGUID() ");
            }

            return baseString;
        }

        public string GetAutoIncrementDefinition(Type fieldType)
        {
            if (fieldType == typeof (Guid))
                return " DEFAULT GenGUID() ";
            else
                return string.Format(" {0} ", AutoIncrementDefinition);
                //return " COUNTER ";
                
        }

    }
}
