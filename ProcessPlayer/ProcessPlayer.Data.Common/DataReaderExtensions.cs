using System;
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

        #endregion
    }
}
