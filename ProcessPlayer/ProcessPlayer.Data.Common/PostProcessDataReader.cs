using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProcessPlayer.Data.Common
{
    public class PostProcessDataReader : IDataReader
    {
        #region private variables

        private IDataReader _reader;
        private IDictionary<int, string> _fieldOrdinal;
        private IDictionary<string, Func<object[], object>> _fieldProcess;
        private IDictionary<string, int> _fieldName;
        private HashSet<string> _fields;

        #endregion

        #region private methods

        private T getValue<T>(int i)
        {
            var field = _fieldOrdinal[i];
            object value = null;

            if (_fieldProcess.ContainsKey(field))
            {
                var values = new object[_reader.FieldCount];

                _reader.GetValues(values);

                value = _fieldProcess[field](values);
            }
            else
                value = _reader.GetValue(i);

            return value is T ? (T)value : default(T);
        }

        #endregion

        #region constructors

        public PostProcessDataReader(IDataReader reader, IDictionary<string, Func<object[], object>> fieldProcess)
        {
            if (fieldProcess == null)
                throw new ArgumentNullException("columnProcess");

            if (reader == null)
                throw new ArgumentNullException("reader");

            _fieldProcess = fieldProcess;
            _fields = new HashSet<string>(Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i)));

            foreach (var cp in fieldProcess.Where(cp => !_fields.Contains(cp.Key)))
                _fields.Add(cp.Key);

            var j = 0;

            _fieldOrdinal = _fields.ToDictionary(f => j++, f => f);
            _fieldName = _fieldOrdinal.ToDictionary(f => f.Value, f => f.Key);
            _reader = reader;
        }

        #endregion

        #region IDataReader Members

        public void Close()
        {
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
            return _reader.NextResult();
        }

        public bool Read()
        {
            return _reader.Read();
        }

        public int RecordsAffected
        {
            get { return _reader.RecordsAffected; }
        }

        public void Dispose()
        {
            _fieldName = null;
            _fieldOrdinal = null;
            _fieldProcess = null;
            _fields = null;
            _reader.Dispose();
            _reader = null;
        }

        public int FieldCount
        {
            get { return _fields.Count; }
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
            return i < _reader.FieldCount && !_fieldProcess.ContainsKey(_fieldOrdinal[i]) ? _reader.GetDataTypeName(i) : i < _fields.Count ? "Object" : null;
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
            return i < _reader.FieldCount && !_fieldProcess.ContainsKey(_fieldOrdinal[i]) ? _reader.GetFieldType(i) : i < _fields.Count ? typeof(object) : null;
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
            var res = _reader.GetValues(values);

            foreach (var p in _fieldProcess)
            {
                values[_fieldName[p.Key]] = p.Value(values);

                res++;
            }

            return res;
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
