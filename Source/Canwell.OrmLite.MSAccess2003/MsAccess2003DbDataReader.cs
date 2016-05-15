using System;
using System.Data;
using System.Data.OleDb;

namespace Canwell.OrmLite.MSAccess2003
{
    public class MsAccess2003DbDataReader : IDataReader
    {
        private OleDbDataReader Reader { get; set; }

        public MsAccess2003DbDataReader(OleDbDataReader reader)
        {
            Reader = reader;
        }

        public void Close()
        {
            Reader.Close();
        }

        public int Depth
        {
            get { return Reader.Depth; }
        }

        public DataTable GetSchemaTable()
        {
            return Reader.GetSchemaTable();
        }

        public bool IsClosed
        {
            get { return Reader.IsClosed; }
        }

        public bool NextResult()
        {
            return Reader.NextResult();
        }

        public bool Read()
        {
            return Reader.Read();
        }

        public int RecordsAffected
        {
            get { return Reader.RecordsAffected; }
        }

        public void Dispose()
        {
            Reader.Dispose();
        }

        public int FieldCount
        {
            get { return Reader.FieldCount; }
        }

        public bool GetBoolean(int i)
        {
            return Reader.GetBoolean(i);
        }

        public byte GetByte(int i)
        {
            return Reader.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return Reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i)
        {
            return Reader.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return Reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i)
        {
            return new MsAccess2003DbDataReader(Reader.GetData(i));
        }

        public string GetDataTypeName(int i)
        {
            return Reader.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i)
        {
            return Reader.GetDateTime(i);
        }

        public decimal GetDecimal(int i)
        {
            return Reader.GetDecimal(i);
        }

        public double GetDouble(int i)
        {
            return Reader.GetDouble(i);
        }

        public Type GetFieldType(int i)
        {
            return Reader.GetFieldType(i);
        }

        public float GetFloat(int i)
        {
            return Reader.GetFloat(i);
        }

        public Guid GetGuid(int i)
        {
            return Reader.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            return Reader.GetInt16(i);
        }

        public int GetInt32(int i)
        {
            return Reader.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            return Reader.GetInt64(i);
        }

        public string GetName(int i)
        {
            return Reader.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            return Reader.GetOrdinal(name);
        }

        public string GetString(int i)
        {
            return Reader.GetString(i);
        }

        public object GetValue(int i)
        {
            return Reader.GetValue(i);
        }

        public int GetValues(object[] values)
        {
            return Reader.GetValues(values);
        }

        public bool IsDBNull(int i)
        {
            return Reader.IsDBNull(i);
        }

        public object this[string name]
        {
            get { return Reader[name]; }
        }

        public object this[int i]
        {
            get { return Reader[i]; }
        }
    }
}
