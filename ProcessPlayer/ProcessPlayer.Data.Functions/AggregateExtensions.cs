using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProcessPlayer.Data.Functions
{
	public static class AggregateExtensions
	{
		#region private classes

		private class ItemsComparer : IComparer<IList>
		{
			#region private methods

			private static int CompareObjects(Type type, object x, object y)
			{
				var xc = x as IComparable;

				if (xc != null)
					return xc.CompareTo(y);

				var yc = y as IComparable;

				if (yc != null)
					return -yc.CompareTo(x);

				if (x is string && y is string)
					return string.Compare((string)x, (string)y);

				double dx = double.MinValue, dy = double.MinValue;

				if (type == typeof(byte)
					|| type == typeof(decimal)
					|| type == typeof(double)
					|| type == typeof(float)
					|| type == typeof(int)
					|| type == typeof(long)
					|| type == typeof(sbyte)
					|| type == typeof(short)
					|| type == typeof(uint)
					|| type == typeof(ulong)
					|| type == typeof(ushort))
				{
					dx = Convert.ToDouble(x);
					dy = Convert.ToDouble(y);
				}

				return Convert.ToInt32(dx - dy);
			}

			#endregion

			#region public methods

			public int Compare(IList x, IList y)
			{
				foreach (var kvp in Order)
				{
					var index = kvp.Key;

					if (index >= 0)
					{
						if (x[index] != null && y[index] != null)
						{
							var type1 = x[index].GetType();
							var type2 = y[index].GetType();

							int value;
							if (type1 == type2 && (value = CompareObjects(type1, x[index], y[index])) != 0)
								return kvp.Value == 0 ? value : -value;
						}
						else if (x[index] != null && y[index] == null)
							return kvp.Value == 0 ? 1 : -1;
						else if (x[index] == null && y[index] != null)
							return kvp.Value == 0 ? -1 : 1;
					}
				}

				return 0;
			}

			#endregion

			#region properties

			public IDictionary<int, int> Order { get; set; }

			#endregion
		}

		#endregion

		#region private static methods

		public static bool isDateTime(object obj)
		{
			return obj is DateTime;
		}

		public static bool isNumeric(object obj)
		{
			return obj is double
				|| obj is int
				|| obj is bool
				|| obj is byte
				|| obj is decimal
				|| obj is float
				|| obj is long
				|| obj is sbyte
				|| obj is short
				|| obj is uint
				|| obj is ulong
				|| obj is ushort;
		}

		#endregion

		#region public static methods

		public static object auto(IEnumerable<object> values, int current, params object[] parameters)
		{
			if (values == null || !values.Any())
				return null;

			var first = values.FirstOrDefault(v => v != null && v != DBNull.Value);

			if (first == null)
				return null;

			if (first is DateTime || first is DateTime?)
				return interval(values, current, parameters);
			if (first is decimal || first is decimal?)
				return sum(values, current, parameters);
			if (first is int || first is int?)
				return sum(values, current, parameters);

			return count(values, current, parameters);
		}

		public static object avg(IEnumerable<object> values, int current, params object[] parameters)
		{
			if (values == null || !values.Any())
				return null;

			var first = values.FirstOrDefault(v => v != null && v != DBNull.Value);

			if (first == null)
				return null;

			if (first is DateTime || first is DateTime?)
				return "#ERR";
			if (first is decimal || first is decimal?)
				return values.Average(v => v == null || v == DBNull.Value ? (decimal?)null : Convert.ToDecimal(v));
			if (first is int || first is int?)
				return values.Average(v => v == null || v == DBNull.Value ? (int?)null : Convert.ToInt32(v));
			if (first is string || first == null)
				return "#ERR";
			return values.Average(v => v == null || v == DBNull.Value ? (double?)null : Convert.ToDouble(v));
		}

		public static object count(IEnumerable<object> values, int current, params object[] parameters)
		{
			if (values == null || !values.Any())
				return null;
			return values.Count(v => v != null && v != DBNull.Value);
		}

		public static object countd(IEnumerable<object> values, int current, params object[] parameters)
		{
			if (values == null || !values.Any())
				return null;
			return values.Distinct().Count(v => v != null && v != DBNull.Value);
		}

		public static object first(IEnumerable<object> values, int current, params object[] parameters)
		{
			var count = parameters == null || parameters.Length == 0 || !(parameters[0] is int) ? 0 : (int)parameters[0];

			return values.Skip(count).FirstOrDefault();
		}

		public static object interval(IEnumerable<object> values, int current, params object[] parameters)
		{
			if (values == null || !values.Any())
				return null;

			var first = values.FirstOrDefault(v => v != null && v != DBNull.Value);

			if (first == null)
				return null;

			if (first is DateTime || first is DateTime?)
				return values.Select(v => v == null || v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v)).Max()
					.Subtract(values.Select(v => v == null || v == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(v)).Min())
					.Days;
			return "#ERR";
		}

		public static object lag(IEnumerable<object> values, int current, params object[] parameters)
		{
			var count = parameters == null || parameters.Length == 0 || !(parameters[0] is int) ? 1 : (int)parameters[0];
			var def = parameters == null || parameters.Length < 2 ? null : parameters[1];

			return values.Take(current + 1).Reverse().Skip(count).FirstOrDefault() ?? def;
		}

		public static object last(IEnumerable<object> values, int current, params object[] parameters)
		{
			var count = parameters == null || parameters.Length == 0 || !(parameters[0] is int) ? 0 : (int)parameters[0];

			return values.Reverse().Skip(count).FirstOrDefault();
		}

		public static object lead(IEnumerable<object> values, int current, params object[] parameters)
		{
			var count = parameters == null || parameters.Length == 0 || !(parameters[0] is int) ? 0 : (int)parameters[0];
			var def = parameters == null || parameters.Length < 2 ? null : parameters[1];

			return values.Skip(current + count).FirstOrDefault() ?? def;
		}

		public static object min(IEnumerable<object> values, int current, params object[] parameters)
		{
			if (values == null || !values.Any())
				return null;

			var first = values.FirstOrDefault(v => v != null && v != DBNull.Value);
			if (first == null)
				return null;

			if (first is DateTime || first is DateTime?)
				return values.Min(v => v == null || v == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(v));
			if (first is decimal || first is decimal?)
				return values.Min(v => v == null || v == DBNull.Value ? (decimal?)null : Convert.ToDecimal(v));
			if (first is int || first is int?)
				return values.Min(v => v == null || v == DBNull.Value ? (int?)null : Convert.ToInt32(v));
			if (first is string || first == null)
				return values.Min(v => v == null || v == DBNull.Value ? null : v.ToString());
			return values.Min(v => v == null || v == DBNull.Value ? (double?)null : Convert.ToDouble(v));
		}

		public static object max(IEnumerable<object> values, int current, params object[] parameters)
		{
			if (values == null || !values.Any())
				return null;

			var first = values.FirstOrDefault(v => v != null && v != DBNull.Value);
			if (first == null)
				return null;

			if (first is DateTime || first is DateTime?)
				return values.Max(v => v == null || v == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(v));
			if (first is decimal || first is decimal?)
				return values.Max(v => v == null || v == DBNull.Value ? (decimal?)null : Convert.ToDecimal(v));
			if (first is int || first is int?)
				return values.Max(v => v == null || v == DBNull.Value ? (int?)null : Convert.ToInt32(v));
			if (first is string || first == null)
				return values.Max(v => v == null || v == DBNull.Value ? null : v.ToString());
			return values.Max(v => v == null || v == DBNull.Value ? (double?)null : Convert.ToDouble(v));
		}

		public static object none(IEnumerable<object> values, int current, params object[] parameters)
		{
			if (values == null || !values.Any())
				return null;

			return values.Skip(1).Any()
				? null
				: values.First();
		}

		public static IEnumerable<IList> range(IEnumerable<IList> context, object[] current, IEnumerable<IList> itemsSource, IList<int> indexes, IDictionary<int, int> order, RangeClause range = null)
		{
			if (indexes == null || indexes.Count == 0)
				return context;

			var gv = indexes.Aggregate(itemsSource, (current1, i) =>
				(from item in
					 (from item in current1
					  group item by item[i] into gs
					  where gs.All(g => Equals(g[i], current[i]))
					  select gs).FirstOrDefault()
				 select item));

			if (range != null)
			{
				if (range.Extent.Between != null)
				{
					if (range.Extent.Between.Start.Preceding != null)
					{
						if (range.Extent.Between.Start.Preceding.Amount != null)
							gv = gv.Skip(range.Extent.Preceding.Amount.Value);
						else if (range.Extent.Between.Start.Preceding.Preceding != null)
							switch (range.Extent.Between.Start.Preceding.Preceding)
							{
								case WindowFrame.Current:
									gv = gv.SkipWhile(i => !Equals(i, current));
									break;
							}
					}

					if (range.Extent.Between.End.Following != null)
					{
						if (range.Extent.Between.End.Following.Amount != null)
							gv = gv.Take(range.Extent.Between.End.Following.Amount.Value);
						else if (range.Extent.Between.End.Following.Preceding != null)
							switch (range.Extent.Between.End.Following.Preceding)
							{
								case WindowFrame.Current:
									gv = gv.TakeWhile(i => !Equals(i, current)).Take(1);
									break;
							}
					}
				}
				else if (range.Extent.Preceding != null)
				{
					if (range.Extent.Preceding.Amount != null)
						gv = gv.Skip(range.Extent.Preceding.Amount.Value);
					else if (range.Extent.Preceding.Preceding != null)
						switch (range.Extent.Preceding.Preceding)
						{
							case WindowFrame.Current:
								gv = gv.SkipWhile(i => !Equals(i, current));
								break;
						}
				}
			}

			if (order != null)
				gv = gv.OrderBy(i => i, new ItemsComparer { Order = order });

			return gv;
		}

		public static object sum(IEnumerable<object> values, int current, params object[] parameters)
		{
			if (values == null || !values.Any())
				return null;

			var first = values.FirstOrDefault(v => v != null && v != DBNull.Value);
			if (first == null)
				return null;

			if (first is DateTime || first is DateTime?)
				return "#ERR";
			if (first is decimal || first is decimal?)
				return values.Sum(v => v == null || v == DBNull.Value ? (decimal?)null : Convert.ToDecimal(v));
			if (first is int || first is int?)
				return values.Sum(v => v == null || v == DBNull.Value ? (int?)null : Convert.ToInt32(v));
			if (first is string || first == null)
				return "#ERR";
			return values.Sum(v => v == null || v == DBNull.Value ? (double?)null : Convert.ToDouble(v));
		}

		public static object variance(IEnumerable<object> values1, IEnumerable<object> values2, int current, params object[] parameters)
		{
			if (values1 == null || !values1.Any())
				return null;

			object v1 = null, v2 = null;

			switch (Convert.ToInt32(parameters[0]))
			{
				case 2:
					v1 = avg(values1, current, parameters);
					v2 = avg(values2, current, parameters);
					break;
				case 1:
					v1 = count(values1, current, parameters);
					v2 = count(values2, current, parameters);
					break;
				case 7:
					v1 = countd(values1, current, parameters);
					v2 = countd(values2, current, parameters);
					break;
				case 3:
					v1 = interval(values1, current, parameters);
					v2 = interval(values2, current, parameters);
					break;
				case 4:
					v1 = max(values1, current, parameters);
					v2 = max(values2, current, parameters);
					break;
				case 5:
					v1 = min(values1, current, parameters);
					v2 = min(values2, current, parameters);
					break;
				case 6:
					v1 = sum(values1, current, parameters);
					v2 = sum(values2, current, parameters);
					break;
			}

			if (v2 == null)
				return v1;
			if (isDateTime(v1) && isDateTime(v2))
				return ((DateTime)v1).Subtract((DateTime)v2).TotalDays;
			if (isNumeric(v1) && isNumeric(v2))
				return Convert.ToDouble(v1) - Convert.ToDouble(v2);
			if (v1 is string && v2 is string)
				return string.Concat(v1, v2);
			if (v1 is TimeSpan && v2 is TimeSpan)
				return ((TimeSpan)v1).TotalDays - ((TimeSpan)v2).TotalDays;
			return "#ERR";
		}

		public static object variancepct(IEnumerable<object> values1, IEnumerable<object> values2, int current, params object[] parameters)
		{
			if (values1 == null || !values1.Any())
				return null;

			object v1 = null, v2 = null;

			switch (Convert.ToInt32(parameters[0]))
			{
				case 2:
					v1 = avg(values1, current, parameters);
					v2 = avg(values2, current, parameters);
					break;
				case 1:
					v1 = count(values1, current, parameters);
					v2 = count(values2, current, parameters);
					break;
				case 7:
					v1 = countd(values1, current, parameters);
					v2 = countd(values2, current, parameters);
					break;
				case 3:
					v1 = interval(values1, current, parameters);
					v2 = interval(values2, current, parameters);
					break;
				case 4:
					v1 = max(values1, current, parameters);
					v2 = max(values2, current, parameters);
					break;
				case 5:
					v1 = min(values1, current, parameters);
					v2 = min(values2, current, parameters);
					break;
				case 6:
					v1 = sum(values1, current, parameters);
					v2 = sum(values2, current, parameters);
					break;
			}

			if (v2 == null)
				return v1;
			if (isNumeric(v1) && isNumeric(v2))
				return Convert.ToDouble(v1) / Convert.ToDouble(v2) * 100;
			if (v1 is TimeSpan && v2 is TimeSpan)
				return ((TimeSpan)v1).TotalDays / ((TimeSpan)v2).TotalDays * 100;
			return "#ERR";
		}

		#endregion
	}
}
