using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProcessPlayer.Data.Functions
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        #region public static methods

        /// <summary>
        /// CLEANs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string clean(string value)
        {
            return Regex.Replace(value, "[\u0000-\u001F]", "");
        }

        /// <summary>
        /// CODEs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int code(string value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            return value[0];
        }

        /// <summary>
        /// CONCATENATEs the specified values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static string concatenate(params string[] values)
        {
            return string.Concat(values);

        }

        /// <summary>
        /// EXACTs the specified value1.
        /// </summary>
        /// <param name="value1">The value1.</param>
        /// <param name="value2">The value2.</param>
        /// <returns></returns>
        public static bool exact(string value1, string value2)
        {
            return value1 == value2;
        }

        /// <summary>
        /// FINDs the specified value1.
        /// </summary>
        /// <param name="findWhat">The value1.</param>
        /// <param name="findWhere">The value2.</param>
        /// <param name="startPosition">The start position.</param>
        /// <returns></returns>
        public static int find(string findWhat, string findWhere, int startPosition)
        {
            return findWhere.IndexOf(findWhat, startPosition, StringComparison.Ordinal) + 1;
        }

        /// <summary>
        /// Formats the specified value.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static string format(string format, params object[] values)
        {
            return string.Format(format, values);
        }

        /// <summary>
        /// LEFTs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="numberOfCharacters">The number of characters.</param>
        /// <returns></returns>
        public static string left(string value, int numberOfCharacters)
        {
            if (value.Length < numberOfCharacters)
                numberOfCharacters = value.Length;

            return value.Substring(0, numberOfCharacters);
        }

        /// <summary>
        /// LEFTs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="numberOfCharacters">The number of characters.</param>
        /// <returns></returns>
        public static string left(object value, int numberOfCharacters)
        {
            if (value == null || value == DBNull.Value)
                return "";

            return left(Convert.ToString(value), numberOfCharacters);
        }

        /// <summary>
        /// LENs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int len(string value)
        {
            return value.Length;
        }

        /// <summary>
        /// LOWERs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string lower(string value)
        {
            return value.ToLower(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// RIGHTs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="numberOfCharacters">The number of characters.</param>
        /// <returns></returns>
        public static string right(string value, int numberOfCharacters)
        {
            var start = value.Length - numberOfCharacters;

            if (start < 0)
                start = 0;

            if (value.Length < numberOfCharacters)
                numberOfCharacters = value.Length;

            return value.Substring(start, numberOfCharacters);
        }

        /// <summary>
        /// RIGHTs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="numberOfCharacters">The number of characters.</param>
        /// <returns></returns>
        public static string right(object value, int numberOfCharacters)
        {
            if (value == null || value == DBNull.Value)
                return "";

            return right(Convert.ToString(value), numberOfCharacters);
        }

        /// <summary>
        /// MIDs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startPosition">The start position.</param>
        /// <param name="numberOfCharacters">The number of characters.</param>
        /// <returns></returns>
        public static string mid(string value, int startPosition, int numberOfCharacters)
        {
            if (startPosition < 1 || startPosition > value.Length + 1)
                startPosition = 1;

            if (numberOfCharacters + startPosition > value.Length + 1)
                numberOfCharacters = value.Length - startPosition + 1;

            return value.Substring(startPosition - 1, numberOfCharacters);
        }

        /// <summary>
        /// PROPERs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string proper(string value)
        {
            var rx = new Regex(@"(?<=\w)\w");

            return rx.Replace(value, m => m.Value.ToLower(CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// REPTs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="numberOfTimes">The number of times.</param>
        /// <returns></returns>
        public static string rept(string value, int numberOfTimes)
        {
            return new string(Enumerable.Repeat(value, numberOfTimes).SelectMany(s => s.ToCharArray()).ToArray());
        }

        /// <summary>
        /// FINDs the specified value1.
        /// </summary>
        /// <param name="findWhat">The find what.</param>
        /// <param name="findWhere">The find where.</param>
        /// <param name="startPosition">The start position.</param>
        /// <returns></returns>
        public static int search(string findWhat, string findWhere, int startPosition)
        {
            return findWhere.IndexOf(findWhat, startPosition, StringComparison.InvariantCultureIgnoreCase) + 1;
        }

        /// <summary>
        /// SUBSTITUTEs the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="oldText">The old text.</param>
        /// <param name="newText">The new text.</param>
        /// <param name="nthAppearance">The NTH appearance.</param>
        /// <returns></returns>
        public static string substitute(string text, string oldText, string newText, int nthAppearance)
        {
            return text.Replace(oldText, newText);
        }

        /// <summary>
        /// Converts the specified value to text using the format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string text(object value, string format)
        {
            return string.Format("{0:" + format + "}", value);
        }

        /// <summary>
        /// CHARs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string toChar(int value)
        {
            return "" + (char)value;
        }

        /// <summary>
        /// TRIMs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="characters">The characters.</param>
        /// <returns></returns>
        public static string trim(string value, string characters = null)
        {
            return value.Trim();
        }

        /// <summary>
        /// TRIMLEFT the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="characters">The characters.</param>
        /// <returns></returns>
        public static string trimLeft(string value, string characters = null)
        {
            return string.IsNullOrEmpty(characters) ? value.TrimStart() : value.TrimStart(characters.ToCharArray());
        }

        /// <summary>
        /// TRIMRIGHT the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="characters">The characters.</param>
        /// <returns></returns>
        public static string trimRight(string value, string characters = null)
        {
            return string.IsNullOrEmpty(characters) ? value.TrimEnd() : value.TrimEnd(characters.ToCharArray());
        }

        /// <summary>
        /// UPPERs the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string upper(string value)
        {
            return value.ToUpper(CultureInfo.InvariantCulture);
        }

        public static double value(string number)
        {
            double d;

            if (!double.TryParse(number, out d)
                && !double.TryParse(number, NumberStyles.Any, CultureInfo.CurrentCulture, out d)
                && !double.TryParse(number, NumberStyles.Any, CultureInfo.CurrentUICulture, out d))
                double.TryParse(number, NumberStyles.Any, CultureInfo.InvariantCulture, out d);

            return d;
        }

        #endregion
    }
}
