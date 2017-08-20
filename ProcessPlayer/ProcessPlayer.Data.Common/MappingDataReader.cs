using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProcessPlayer.Data.Common
{
    public class MappingDataReader : IDataReader
    {
        #region private variables

        private IDataReader _reader;
        private IDictionary<int, string> _fieldOrdinal;
        private IDictionary<string, int> _fieldName;

        #endregion

        #region private methods

        private T getValue<T>(int i)
        {
            var field = _fieldOrdinal[i];
            var index = _fieldName[field];
            var value = _reader.GetValue(index);

            return value is T ? (T)value : default(T);
        }

        #endregion

        #region constructors

        public MappingDataReader(IDataReader reader, IDictionary<string, string> fieldMapping, MappingMode mappingMode)
        {
            if (fieldMapping == null)
                throw new ArgumentNullException("fieldMapping");

            if (reader == null)
                throw new ArgumentNullException("reader");

            IDictionary<string, string> dict;
            var i = 0;

            switch (mappingMode)
            {
                case MappingMode.ExcludeUnmapped:
                    dict =
                        (from m in fieldMapping
                         join n in reader.GetFieldNames() on m.Key equals n into ts
                         from t in ts.DefaultIfEmpty().Where(t => t != null)
                         select m).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                    break;
                case MappingMode.Join:
                    dict =
                        (from n in reader.GetFieldNames()
                         join m in fieldMapping on n equals m.Key into ts
                         from t in ts.DefaultIfEmpty()
                         select new { name = n, value = string.IsNullOrEmpty(t.Value) ? n : t.Value }).ToDictionary(o => o.name, o => o.value);
                    break;
                case MappingMode.JoinCaseInsensitive:
                    dict =
                        (from n in reader.GetFieldNames()
                         join m in fieldMapping on n.ToLower() equals m.Key.ToLower() into ts
                         from t in ts.DefaultIfEmpty()
                         select new { name = n, value = string.IsNullOrEmpty(t.Value) ? n : t.Value }).ToDictionary(o => o.name, o => o.value);
                    break;
                default:
                    dict = reader.GetFieldNames().ToDictionary(n => n, n => n);

                    foreach (var kvp in fieldMapping.Where(kvp => dict.ContainsKey(kvp.Key)))
                        dict[kvp.Key] = kvp.Value;
                    break;
            }

            _fieldName = dict.ToDictionary(kvp => kvp.Value, kvp => reader.GetOrdinal(kvp.Key));
            _fieldOrdinal = dict.ToDictionary(kvp => i++, kvp => kvp.Value);
            _reader = reader;
        }

        #endregion

        #region IDataReader Members

        public void Close()
        {
            _fieldName = null;
            _fieldOrdinal = null;
            _reader.Close();
        }

        public int Depth
        {
            get { return _reader.Depth; }
        }

        public DataTable GetSchemaTable()
        {
            return _reader.GetSchemaTable();
        }

        public bool IsClosed
        {
            get { return _reader.IsClosed; }
        }

        public bool NextResult()
        {
            return _reader.IsClosed ? false : _reader.NextResult();
        }

        public bool Read()
        {
            return _reader.Read();
        }

        public int RecordsAffected
        {
            get { return _reader.RecordsAffected; ; }
        }

        public void Dispose()
        {
            _reader.Dispose();
        }

        public int FieldCount
        {
            get { return _fieldName.Count; }
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
            return _reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i)
        {
            return getValue<char>(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return _reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i)
        {
            return _reader.GetData(i);
        }

        public string GetDataTypeName(int i)
        {
            var field = _fieldOrdinal[i];
            var index = _fieldName[field];

            return _reader.GetDataTypeName(index);
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
            var field = _fieldOrdinal[i];
            var index = _fieldName[field];

            return _reader.GetFieldType(index);
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
            var fullValues = new object[_reader.FieldCount];
            var i = 0;

            _reader.GetValues(fullValues);

            foreach (var kvp in _fieldName)
                values[i++] = fullValues[kvp.Value];

            return _fieldName.Count;
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

    public enum MappingMode
    {
        All = 0,
        ExcludeUnmapped = 1,
        Join = 2,
        JoinCaseInsensitive = 3,
    }
}
