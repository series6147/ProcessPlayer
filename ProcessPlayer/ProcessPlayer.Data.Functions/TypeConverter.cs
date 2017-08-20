using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ProcessPlayer.Data.Functions
{
	public class TypeConverter
	{
		#region private variables

		private readonly IFormatProvider _formatProvider = CultureInfo.InvariantCulture;
		private readonly string[] _dateTimeFormats;
		private readonly IDictionary<string, object> _nullValues;
		private static readonly string[] _dateTimePatternsLeadingMonth;
		private static readonly string[] _dateTimePatternsLeadingDay;
		private static readonly string[] _dateTimePatternsCommon =
        { 
            "yyyyMMdd",
            "yyyy-M-d",
            "yyyy-M-d"
        };

		#endregion

		#region public methods

		public object ChangeType(object v, Type targetType)
		{
			var s = v as string;
			if (s != null)
			{
				if (_nullValues.ContainsKey(s))
					return null;
				if (targetType == typeof(bool) && string.Equals(s, "YES", StringComparison.InvariantCultureIgnoreCase))
					return true;
				if (targetType == typeof(bool) && string.Equals(s, "NO", StringComparison.InvariantCultureIgnoreCase))
					return false;
			}

			return Convert.ChangeType(v, targetType, _formatProvider);
		}

		public DateTime ParseDateTime(string s)
		{
			DateTime dt;

			if (TryParseDateTime(s, out dt))
				return dt;
			throw new FormatException(string.Format("'{0}' is not a valid date and time.", s));
		}

		public DateTime ToDateTime(object v)
		{
			if (v == null || v == DBNull.Value)
				throw new FormatException("null value cannot be converted to date and time.");

			switch (Type.GetTypeCode(v.GetType()))
			{
				case TypeCode.Int16:
					return ToDateTime((Int16)v);
				case TypeCode.Int32:
					return ToDateTime((Int32)v);
				case TypeCode.Int64:
					return ToDateTime((int)(Int64)v);
				case TypeCode.Double:
					return ToDateTime((double)v);
				case TypeCode.Decimal:
					return ToDateTime((decimal)v);
				case TypeCode.UInt16:
					return ToDateTime((UInt16)v);
				case TypeCode.UInt32:
					return ToDateTime((int)(UInt32)v);
				case TypeCode.UInt64:
					return ToDateTime((int)(UInt64)v);
			}

			return ParseDateTime(Convert.ToString(v));
		}
		public DateTime ToDateTime(int d)
		{
			var year = d / 10000;
			var month = (d - year * 10000) / 100;
			var day = d - year * 10000 - month * 100;

			return new DateTime(year, month, day);
		}
		public DateTime ToDateTime(decimal d)
		{
			return d > 10000000m ? ToDateTime((int)d) : DateTime.FromOADate((double)d);
		}
		public DateTime ToDateTime(double d)
		{
			return d > 10000000 ? ToDateTime((int)d) : DateTime.FromOADate(d);
		}

		public DateTime? TryParseDateTime(string s)
		{
			DateTime dt;

			if (TryParseDateTime(s, out dt))
				return dt;
			return null;
		}
		public bool TryParseDateTime(string s, out DateTime dt)
		{
			if (DateTime.TryParse(s, out dt))
				return true;

			double odt;
			if (double.TryParse(s, out odt))
				if (odt > -657435.0 && odt < 2958466.0)
					try
					{
						dt = DateTime.FromOADate(odt);
						return true;
					}
					catch
					{
						return false;
					}

			foreach (var f in _dateTimeFormats)
				if (DateTime.TryParseExact(s, f, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
					return true;

			return false;
		}

		#endregion

		#region constructors

		static TypeConverter()
		{
			_dateTimePatternsLeadingMonth =
				_dateTimePatternsCommon.Concat(new[]
                {
                    "M/d/yyyy",
                    "MM/dd/yyyy",
                    "M/d/yy",
                    "MM/dd/yy",
                    "M-d-yyyy",
                    "MM-dd-yyyy",
                    "M-d-yy",
                    "MM-dd-yy",
                    "M.d.yyyy",
                    "MM.dd.yyyy",
                    "M.d.yy",
                    "MM.dd.yy"
                }).ToArray();

			_dateTimePatternsLeadingDay =
				_dateTimePatternsCommon.Concat(new[]
                {
                    "d/M/yyyy",
                    "dd/MM/yyyy",
                    "d/M/yy",
                    "dd/MM/yy",
                    "d-M-yyyy",
                    "dd-MM-yyyy",
                    "d-M-yy",
                    "dd-MM-yy",
                    "d.M.yyyy",
                    "dd.MM.yyyy",
                    "d.M.yy",
                    "dd.MM.yy"
                }).ToArray();
		}

		public TypeConverter()
		{
			var dateTimeFormat = CultureInfo.CurrentUICulture.DateTimeFormat;

			if (dateTimeFormat.ShortDatePattern.StartsWith("M"))
				_dateTimeFormats = _dateTimePatternsLeadingMonth;
			else if (dateTimeFormat.ShortDatePattern.StartsWith("d"))
				_dateTimeFormats = _dateTimePatternsLeadingDay;
			else
				_dateTimeFormats = _dateTimePatternsCommon;
		}

		public TypeConverter(IEnumerable<string> nullValues)
		{
			_nullValues = nullValues == null ? new Dictionary<string, object>() : nullValues.Distinct().ToDictionary(s => s, s => (object)null);
		}

		#endregion
	}
}
