using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProcessPlayer.Data.Common
{
    public class DictionaryReader<T1, T2> : IDataReader
    {
        #region private variables

        private bool _isClosed;
        private IDictionary<int, string> _fieldOrdinal;
        private IDictionary<string, int> _fieldName;
        private IList<object[]> _items;
        private int _current = -1;

        #endregion

        #region private methods

        private T getValue<T>(int i)
        {
            var value = _items[_current][i];

            return value is T ? (T)value : default(T);
        }

        #endregion

        #region constructors

        public DictionaryReader(IDictionary dict, IList<string> names = null)
        {
            if (dict == null)
                throw new ArgumentNullException("dict");

            if (names == null)
                names = new string[] { };

            _items = dict.Cast<KeyValuePair<T1, T2>>().Select(kvp => new object[] { kvp.Key, kvp.Value }).ToArray();

            _fieldOrdinal = new Dictionary<int, string>();
            _fieldOrdinal.Add(0, names.ElementAtOrDefault(0) ?? "Key");
            _fieldOrdinal.Add(1, names.ElementAtOrDefault(1) ?? "Value");

            _fieldName = _fieldOrdinal.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
        }

        #endregion

        #region IDataReader Members

        public void Close()
        {
            _fieldName = null;
            _fieldOrdinal = null;
            _items = null;
        }

        public int Depth
        {
            get { return 0; }
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool IsClosed
        {
            get { return _isClosed; }
        }

        public bool NextResult()
        {
            if (_isClosed || (_isClosed = ++_current >= _items.Count))
                return false;

            return true;
        }

        public bool Read()
        {
            if (_isClosed || (_isClosed = ++_current >= _items.Count))
                return false;

            return true;
        }

        public int RecordsAffected
        {
            get { return _isClosed ? _current : 0; }
        }

        public void Dispose()
        {
            Close();
        }

        public int FieldCount
        {
            get { return 2; }
        }

        public bool GetBoolean(int i)
        {
            return getValue<bool>(i);
        }

        public byte GetByte(int i)
        {
            return getValue<byte>(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            return getValue<char>(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            switch (i)
            {
                case 0:
                    return typeof(T1).Name;
                case 1:
                    return typeof(T2).Name;
            }

            return null;
        }

        public DateTime GetDateTime(int i)
        {
            return getValue<DateTime>(i);
        }

        public decimal GetDecimal(int i)
        {
            return getValue<decimal>(i);
        }

        public double GetDouble(int i)
        {
            return getValue<double>(i);
        }

        public Type GetFieldType(int i)
        {
            switch (i)
            {
                case 0:
                    return typeof(T1);
                case 1:
                    return typeof(T2);
            }

            return null;
        }

        public float GetFloat(int i)
        {
            return getValue<float>(i);
        }

        public Guid GetGuid(int i)
        {
            return getValue<Guid>(i);
        }

        public short GetInt16(int i)
        {
            return getValue<short>(i);
        }

        public int GetInt32(int i)
        {
            return getValue<int>(i);
        }

        public long GetInt64(int i)
        {
            return getValue<long>(i);
        }

        public string GetName(int i)
        {
            return _fieldOrdinal[i];
        }

        public int GetOrdinal(string name)
        {
            return _fieldName.ContainsKey(name) ? _fieldName[name] : -1;
        }

        public string GetString(int i)
        {
            return getValue<string>(i);
        }

        public object GetValue(int i)
        {
            return getValue<object>(i);
        }

        public int GetValues(object[] values)
        {
            if (values.Length > 0)
                values[0] = _items[_current][0];

            if (values.Length > 1)
                values[1] = _items[_current][1];

            return 2;
        }

        public bool IsDBNull(int i)
        {
            return getValue<object>(i) == DBNull.Value;
        }

        public object this[string name]
        {
            get { return getValue<object>(_fieldName[name]); }
        }

        public object this[int i]
        {
            get { return getValue<object>(i); }
        }

        #endregion
    }
}
