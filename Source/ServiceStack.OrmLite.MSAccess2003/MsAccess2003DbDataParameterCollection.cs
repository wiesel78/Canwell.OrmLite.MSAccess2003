using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace ServiceStack.OrmLite.MSAccess2003
{
    public class MsAccess2003DbDataParameterCollection : IDataParameterCollection
    {
        private OleDbParameterCollection ParameterCollection { get; set; }

        public MsAccess2003DbDataParameterCollection(OleDbParameterCollection parameterCollection)
        {
            ParameterCollection = parameterCollection;
        }

        public bool Contains(string parameterName)
        {
            return ParameterCollection.Contains(parameterName);
        }

        public int IndexOf(string parameterName)
        {
            return ParameterCollection.IndexOf(parameterName);
        }

        public void RemoveAt(string parameterName)
        {
            ParameterCollection.RemoveAt(parameterName);
        }

        public object this[string parameterName]
        {
            get { return ParameterCollection[parameterName]; }
            set { ParameterCollection[parameterName] = (OleDbParameter)value; }
        }

        public int Add(object value)
        {
            return ParameterCollection.Add(value);
        }

        public void Clear()
        {
            ParameterCollection.Clear();
        }

        public bool Contains(object value)
        {
            return ParameterCollection.Contains(value);
        }

        public int IndexOf(object value)
        {
            return ParameterCollection.IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            ParameterCollection.Insert(index, value);
        }

        public bool IsFixedSize
        {
            get { return ParameterCollection.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return ParameterCollection.IsReadOnly; }
        }

        public void Remove(object value)
        {
            ParameterCollection.Remove(value);
        }

        public void RemoveAt(int index)
        {
            ParameterCollection.RemoveAt(index);
        }

        public object this[int index]
        {
            get { return ParameterCollection[index]; }
            set { ParameterCollection[index] = (OleDbParameter)value; }
        }

        public void CopyTo(Array array, int index)
        {
            ParameterCollection.CopyTo(array, index);
        }

        public int Count
        {
            get { return ParameterCollection.Count; }
        }

        public bool IsSynchronized
        {
            get { return ParameterCollection.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return ParameterCollection.SyncRoot; }
        }

        public System.Collections.IEnumerator GetEnumerator()
        {
            return ParameterCollection.GetEnumerator();
        }
    }
}
