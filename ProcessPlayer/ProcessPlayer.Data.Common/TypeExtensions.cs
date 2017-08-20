using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ProcessPlayer.Data.Common
{
    public static class TypeExtensions
    {
        #region public static methods

        public static string ConvertHexToString(this string hexValue)
        {
            var i = 0;
            var sb = new StringBuilder();

            while (i < hexValue.Length)
            {
                sb.Append(Convert.ToChar(Convert.ToUInt32(hexValue.Substring(i, 2), 16)));

                i += 2;
            }

            return sb.ToString();
        }

        public static string ConvertStringToHex(this string asciiString)
        {
            var sb = new StringBuilder();

            foreach (char c in asciiString)
                sb.Append(String.Format("{0:x2}", (uint)Convert.ToUInt32(c)));

            return sb.ToString();
        }

        public static int FindValue(this IList<int> list, int indexStart, int indexEnd, int value)
        {
            var count = list.Count;

            while (true)
            {
                if (indexStart <= indexEnd && indexEnd < count)
                {
                    if (indexStart == indexEnd)
                        return indexStart;

                    var index = indexStart + (int)Math.Ceiling((indexEnd - indexStart) / 2d);

                    if (list[index] == value)
                        return index;

                    if (list[index] > value)
                        indexEnd = index - 1;
                    else
                        indexStart = index + 1;
                }
                else
                    return indexStart;
            }
        }

        public static int FindValue(this IList<object[]> list, int indexStart, int indexEnd, int value, int idx)
        {
            var count = list.Count;

            while (true)
            {
                if (indexStart <= indexEnd && indexEnd < count)
                {
                    if (indexStart == indexEnd)
                        return indexStart;

                    var index = indexStart + (int)Math.Ceiling((indexEnd - indexStart) / 2d);

                    if ((int)list[index][idx] == value)
                        return index;

                    if ((int)list[index][idx] > value)
                        indexEnd = index - 1;
                    else
                        indexStart = index + 1;
                }
                else
                    return indexStart;
            }
        }

        public static SqlDbType GetDBType(this Type type)
        {
            SqlParameter param = new SqlParameter();
            TypeConverter converter = TypeDescriptor.GetConverter(param.DbType);

            if (converter.CanConvertFrom(type))
                param.DbType = (DbType)converter.ConvertFrom(type.Name);
            else
            {
                try
                {
                    param.DbType = (DbType)converter.ConvertFrom(type.Name);
                }
                catch { }
            }

            return param.SqlDbType;
        }

        public static object GetDefault<T>(this T type) where T : Type
        {
            return default(T);
        }

        public static IEnumerable<int> IntersectSorted(this IList<int> list1, IList<int> list2)
        {
            int count1 = list1.Count
                , count2 = list2.Count
                , index1 = 0
                , index2 = 0;

            while (index1 < list1.Count && index2 < list2.Count)
            {
                if (list1[index1] == list2[index2])
                {
                    yield return list1[index1];

                    index1++;
                    index2++;
                }
                else
                {
                    int indexStart;
                    int indexEnd;

                    if (list1[index1] < list2[index2])
                    {
                        var inc = 1;

                        while (index1 + inc < count1 && list1[index1 + inc] < list2[index2])
                            inc <<= 1;

                        indexEnd = index1 + inc < count1 ? index1 + inc : count1 - 1;
                        indexStart = index1 + inc >> 1;

                        index1 = inc == 1
                            ? index1 + 1
                            : FindValue(list1, indexStart, indexEnd, list2[index2]);
                    }
                    else
                    {
                        var inc = 1;

                        while (index2 + inc < count2 && list2[index2 + inc] < list1[index1])
                            inc <<= 1;

                        indexEnd = index2 + inc < count2 ? index2 + inc : count2 - 1;
                        indexStart = index2 + inc >> 1;

                        index2 = inc == 1
                            ? index2 + 1
                            : FindValue(list2, indexStart, indexEnd, list1[index1]);
                    }
                }
            }
        }

        public static IEnumerable<object[]> IntersectSorted(this IList<object[]> list1, IList<object[]> list2, int idx)
        {
            int count1 = list1.Count
                , count2 = list2.Count
                , index1 = 0
                , index2 = 0;

            while (index1 < list1.Count && index2 < list2.Count)
            {
                if ((int)list1[index1][idx] == (int)list2[index2][idx])
                {
                    yield return list1[index1];

                    index1++;
                    index2++;
                }
                else
                {
                    int indexStart;
                    int indexEnd;

                    if ((int)list1[index1][idx] < (int)list2[index2][idx])
                    {
                        var inc = 1;

                        while (index1 + inc < count1 && (int)list1[index1 + inc][idx] < (int)list2[index2][idx])
                            inc <<= 1;

                        indexEnd = index1 + inc < count1 ? index1 + inc : count1 - 1;
                        indexStart = index1 + inc >> 1;

                        index1 = inc == 1
                            ? index1 + 1
                            : FindValue(list1, indexStart, indexEnd, (int)list2[index2][idx], idx);
                    }
                    else
                    {
                        var inc = 1;

                        while (index2 + inc < count2 && (int)list2[index2 + inc][idx] < (int)list1[index1][idx])
                            inc <<= 1;

                        indexEnd = index2 + inc < count2 ? index2 + inc : count2 - 1;
                        indexStart = index2 + inc >> 1;

                        index2 = inc == 1
                            ? index2 + 1
                            : FindValue(list2, indexStart, indexEnd, (int)list1[index1][idx], idx);
                    }
                }
            }
        }

        public static bool IsDateTime(this object value)
        {
            return value is DateTime;
        }

        public static bool IsDateTime(this Type type)
        {
            return type == typeof(DateTime);
        }

        public static bool IsNumeric(this object value)
        {
            if (value is decimal || value is decimal?)
                return true;
            else if (value is double || value is double?)
                return true;
            else if (value is float || value is float?)
                return true;
            else if (value is int || value is int?)
                return true;
            else if (value is long || value is long?)
                return true;
            else if (value is short || value is short?)
                return true;
            else if (value is uint || value is uint?)
                return true;
            else if (value is ulong || value is ulong?)
                return true;
            else if (value is ushort || value is ushort?)
                return true;
            return false;
        }

        public static bool IsNumeric(this Type type)
        {
            if (type == typeof(decimal) || type == typeof(decimal?))
                return true;
            else if (type == typeof(double) || type == typeof(double?))
                return true;
            else if (type == typeof(float) || type == typeof(float?))
                return true;
            else if (type == typeof(int) || type == typeof(int?))
                return true;
            else if (type == typeof(long) || type == typeof(long?))
                return true;
            else if (type == typeof(short) || type == typeof(short?))
                return true;
            else if (type == typeof(uint) || type == typeof(uint?))
                return true;
            else if (type == typeof(ulong) || type == typeof(ulong?))
                return true;
            else if (type == typeof(ushort) || type == typeof(ushort?))
                return true;
            return false;
        }

        public static T ToValue<T>(this object value)
        {
            try
            {
                return value == null || value == DBNull.Value
                    ? default(T)
                    : value is T
                        ? (T)value
                        : (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        public static T ToValue<T>(this object value, T defaultValue)
        {
            try
            {
                return value == null || value == DBNull.Value
                    ? defaultValue
                    : value is T
                        ? (T)value
                        : (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion
    }
}
