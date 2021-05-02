using System;
using System.Data;
using System.Globalization;

namespace Shared
{
    /// <summary>
    /// Convert a value to other type
    /// </summary>
    public class DBConvert
    {
        private const Int16 SMALLINT_MIN_VALUE = Int16.MinValue;
        private const int INT_MIN_VALUE = int.MinValue;
        private const long LONG_MIN_VALUE = long.MinValue;
        private const double DOUBLE_MIN_VALUE = double.MinValue;
        private const decimal DECIMAL_MIN_VALUE = decimal.MinValue;
        private static DateTime DATETIME_MIN_VALUE = DateTime.MinValue;
        private static char CHAR_MIN_VALUE = char.MinValue;

        #region Parse Value Before Save To DB
        /// <summary>
        /// Parse a object value to data type
        /// </summary>
        /// <param name="value">A int value to parse</param>
        /// <returns>A object value</returns>
        public static object ParseToDBValue(object _value)
        {
            if (_value == null)
                return DBNull.Value;
            Type type = _value.GetType();

            if (type == typeof(System.Char))
            {
                return ParseToDBValue((char)_value);
            }
            if (type == typeof(System.String))
            {
                return ParseToDBValue((string)_value);
            }
            if (type == typeof(System.Int16))
            {
                return ParseToDBValue((Int16)_value);
            }
            if (type == typeof(System.Int32))
            {
                return ParseToDBValue((int)_value);
            }
            if (type == typeof(System.Int64))
            {
                return ParseToDBValue((long)_value);
            }
            if (type == typeof(System.Decimal))
            {
                return ParseToDBValue((decimal)_value);
            }
            if (type == typeof(System.Double))
            {
                return ParseToDBValue((double)_value);
            }
            if (type == typeof(System.DateTime))
            {
                return ParseToDBValue((DateTime)_value);
            }
            return DBNull.Value;
        }
        /// <summary>
        /// Parse a int16 value to data type
        /// </summary>
        /// <param name="value">A int value to parse</param>
        /// <returns>A object value</returns>
        public static object ParseToDBValue(Int16 value)
        {
            return (value == SMALLINT_MIN_VALUE) ? DBNull.Value : (object)value;
        }
        /// <summary>
        /// Parse a int value to data type
        /// </summary>
        /// <param name="value">A int value to parse</param>
        /// <returns>A object value</returns>
        public static object ParseToDBValue(int value)
        {
            return (value == INT_MIN_VALUE) ? DBNull.Value : (object)value;
        }
        /// <summary>
        /// Parse a long value to data type
        /// </summary>
        /// <param name="value">A long value to parse</param>
        /// <returns>A object value</returns>
        public static object ParseToDBValue(long value)
        {
            return (value == LONG_MIN_VALUE) ? DBNull.Value : (object)value;
        }
        /// <summary>
        /// Parse a double value to data type
        /// </summary>
        /// <param name="value">A double value to parse</param>
        /// <returns>A object value</returns>
        public static object ParseToDBValue(double value)
        {
            return (value == DOUBLE_MIN_VALUE) ? DBNull.Value : (object)value;
        }
        /// <summary>
        /// Parse a decimal value to data type
        /// </summary>
        /// <param name="value">A decimal value to parse</param>
        /// <returns>A object value</returns>
        public static object ParseToDBValue(decimal value)
        {
            return (value == DECIMAL_MIN_VALUE) ? DBNull.Value : (object)value;
        }
        /// <summary>
        /// Parse a DateTime value to data type
        /// </summary>
        /// <param name="value">A DateTime value to parse</param>
        /// <returns>A object value</returns>
        public static object ParseToDBValue(DateTime value)
        {
            return (value == DATETIME_MIN_VALUE) ? DBNull.Value : (object)value;
        }
        /// <summary>
        /// Parse a string value to data type
        /// </summary>
        /// <param name="value">A string value to parse</param>
        /// <returns>A object value</returns>
        public static object ParseToDBValue(string value)
        {
            return (value == null || value == string.Empty) ? DBNull.Value : (object)value;
        }
        /// <summary>
        /// Parse a char value to data type
        /// </summary>
        /// <param name="value">A char value to parse</param>
        /// <returns>A object value</returns>
        public static object ParseToDBValue(char value)
        {
            return (value == CHAR_MIN_VALUE) ? DBNull.Value : (object)value;
        }
        /// <summary>
        /// Parse a char value to data type
        /// </summary>
        /// <param name="value">A char value to parse</param>
        /// <returns>A object value</returns>

        #endregion

        #region Parse Data Value to Value Type
        /// <summary>
        /// Parse a data type to int16 value
        /// </summary>
        /// <param name="value">A data value need to parse</param>
        /// <returns>A int16 value</returns>
        public static Int16 ParseDBToSmallInt(IDataReader dre, string column)
        {

            Int16 result = SMALLINT_MIN_VALUE;
            try
            {
                result = dre.IsDBNull(dre.GetOrdinal(column)) ? SMALLINT_MIN_VALUE : dre.GetInt16(dre.GetOrdinal(column));
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// Parse a data type to int32 value
        /// </summary>
        /// <param name="value">A data value need to parse</param>
        /// <returns>A int32 value</returns>
        public static int ParseDBToInt(IDataReader dre, string column)
        {
            int result = INT_MIN_VALUE;
            try
            {
                result = dre.IsDBNull(dre.GetOrdinal(column)) ? INT_MIN_VALUE : int.Parse(dre.GetInt32(dre.GetOrdinal(column)).ToString());
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// Parse a data type to int64 value
        /// </summary>
        /// <param name="value">A data value need to parse</param>
        /// <returns>A int64 value</returns>
        public static long ParseDBToLong(IDataReader dre, string column)
        {
            long result;
            try
            {
                result = dre.IsDBNull(dre.GetOrdinal(column)) ? LONG_MIN_VALUE : dre.GetInt64(dre.GetOrdinal(column));
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// Parse a data type to double value
        /// </summary>
        /// <param name="value">A data value need to parse</param>
        /// <returns>A double value</returns>
        public static double ParseDBToDouble(IDataReader dre, string column)
        {
            double result;
            try
            {
                result = dre.IsDBNull(dre.GetOrdinal(column)) ? DOUBLE_MIN_VALUE : dre.GetDouble(dre.GetOrdinal(column));
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// Parse a data type to decimal value
        /// </summary>
        /// <param name="value">A data value need to parse</param>
        /// <returns>A decimal value</returns>
        public static decimal ParseDBToDecimal(IDataReader dre, string column)
        {
            decimal result;
            try
            {
                result = dre.IsDBNull(dre.GetOrdinal(column)) ? DECIMAL_MIN_VALUE : dre.GetDecimal(dre.GetOrdinal(column));
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// Parse a data type to dateTime value
        /// </summary>
        /// <param name="value">A data value need to parse</param>
        /// <returns>A dateTime value</returns>
        public static DateTime ParseDBToDateTime(IDataReader dre, string column)
        {
            DateTime result;
            try
            {
                result = dre.IsDBNull(dre.GetOrdinal(column)) ? DATETIME_MIN_VALUE : dre.GetDateTime(dre.GetOrdinal(column));
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        /// <summary>
        /// Parse a data type to string value
        /// </summary>
        /// <param name="value">A data value need to parse</param>
        /// <returns>A string value</returns>
        public static string ParseDBToString(IDataReader dre, string column)
        {
            string result = string.Empty;
            try
            {
                result = dre.IsDBNull(dre.GetOrdinal(column)) ? string.Empty : dre.GetString(dre.GetOrdinal(column));
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public static char ParseDBToChar(IDataReader dre, string column)
        {
            char result;
            try
            {
                result = dre.IsDBNull(dre.GetOrdinal(column)) ? CHAR_MIN_VALUE : char.Parse(dre.GetString(dre.GetOrdinal(column)));
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        #endregion

        #region Parse 1 Value To other value type
        public static string ParseString(Int16 value)
        {
            return (value == SMALLINT_MIN_VALUE) ? string.Empty : value.ToString();
        }
        public static string ParseString(int value)
        {
            return (value == INT_MIN_VALUE) ? string.Empty : value.ToString();
        }
        public static string ParseString(long value)
        {
            return (value == LONG_MIN_VALUE) ? string.Empty : value.ToString();
        }
        public static string ParseString(double value)
        {
            return (value == DOUBLE_MIN_VALUE) ? string.Empty : value.ToString();
        }
        public static string ParseString(decimal value)
        {
            return (value == DECIMAL_MIN_VALUE) ? string.Empty : value.ToString();
        }
        public static string ParseString(DateTime value, string format)
        {
            return (value == DATETIME_MIN_VALUE) ? string.Empty : value.ToString(format);
        }
        public static string ParseString(char value)
        {
            return (value == CHAR_MIN_VALUE) ? string.Empty : value.ToString();
        }
        public static string ParseString(object value)
        {
            return (value == DBNull.Value) ? string.Empty : value.ToString();
        }
        public static Int16 ParseSmallInt(string value)
        {
            Int16 result;
            value = value.Replace(",", "").Trim();
            try { result = Int16.Parse(value); }
            catch { result = SMALLINT_MIN_VALUE; }
            return result;
        }
        public static int ParseInt(string value)
        {
            int result;
            try
            {
                value = value.Replace(",", "").Trim();
                result = int.Parse(value);
            }
            catch { result = INT_MIN_VALUE; }
            return result;
        }
        public static int ParseInt(object obj)
        {
            int result = int.MinValue;
            if (obj != DBNull.Value && obj != null)
                result = (int)obj;
            return result;
        }
        public static long ParseLong(string value)
        {
            long result;
            try
            {
                value = value.Replace(",", "").Trim();
                result = long.Parse(value);
            }
            catch { result = LONG_MIN_VALUE; }
            return result;
        }
        public static long ParseLong(object obj)
        {
            long result = long.MinValue;
            if (obj != DBNull.Value && obj != null)
                result = (long)obj;
            return result;
        }
        public static double ParseDouble(string value)
        {
            double result;
            value = value.Replace(",", "").Trim();
            try { result = double.Parse(value); }
            catch { result = DOUBLE_MIN_VALUE; }
            return result;
        }

        public static double ParseDouble(object value)
        {
            double result = DOUBLE_MIN_VALUE;
            if (value != null && value != DBNull.Value)
                result = (double)value;
            return result;
        }

        public static decimal ParseDecimal(string value)
        {
            decimal result;
            value = value.Replace(",", "").Trim();
            try { result = decimal.Parse(value); }
            catch { result = DECIMAL_MIN_VALUE; }
            return result;
        }
        public static DateTime ParseDateTime(string value, string format)
        {
            DateTime result;
            DateTimeFormatInfo info;
            info = (DateTimeFormatInfo)CultureInfo.CurrentUICulture.DateTimeFormat.Clone();
            info.ShortDatePattern = format;
            info.FullDateTimePattern = format;
            try { result = DateTime.Parse(value, info); }
            catch { result = DATETIME_MIN_VALUE; }
            return result;
        }
        public static DateTime ParseDateTime(object value)
        {
            DateTime result = DateTime.MinValue;
            if (value != null && value != DBNull.Value)
                result = (DateTime)value;
            return result;
        }
        #endregion
    }
}
