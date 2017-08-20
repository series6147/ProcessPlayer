using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ProcessPlayer.Data.Common
{
    public static class DataReaderExtensions
    {
        #region public static methods

        public static IEnumerable<T> AsEnumerable<T>(this IDataReader dataReader, Func<IDataRecord, T> converter)
        {
            if (dataReader == null)
                throw new ArgumentNullException("dataReader");

            if (converter == null)
                throw new ArgumentNullException("converter");

            return dataReader.AsEnumerable(true, converter);
        }

        public static IEnumerable<T> AsEnumerable<T>(this IDataReader dataReader, bool closeDataReader, Func<IDataRecord, T> converter)
        {
            if (dataReader == null)
                throw new ArgumentNullException("dataReader");

            if (converter == null)
                throw new ArgumentNullException("converter");

            try
            {
                while (dataReader.Read())
                    yield return converter(dataReader);
            }
            finally
            {
                if (closeDataReader)
                    dataReader.Close();
            }
        }

        public static IEnumerable<string> GetFieldNames(this IDataReader reader)
        {
            return reader == null ? null : Enumerable.Range(0, reader.FieldCount).Select(i => reader.GetName(i));
        }

        public static IDataReader ToDataReader(this object obj)
        {
            if (obj is IDataReader)
                return (IDataReader)obj;
            else if (obj is IDictionary)
            {
                var dict = (IDictionary)obj;
                var keyTypes = dict.Keys.Cast<object>().GroupBy(o => o == null ? typeof(object) : o.GetType());
                var keyType = keyTypes.Count() == 1 ? keyTypes.First().Key : typeof(object);
                var valueTypes = dict.Values.Cast<object>().GroupBy(o => o == null ? typeof(object) : o.GetType());
                var valueType = valueTypes.Count() == 1 ? valueTypes.First().Key : typeof(object);

                var readerType = typeof(DictionaryReader<,>).MakeGenericType(new Type[] { keyType, valueType });

                return Activator.CreateInstance(readerType, obj as IDictionary) as IDataReader;
            }
            return null;
        }

        #endregion
    }
}
