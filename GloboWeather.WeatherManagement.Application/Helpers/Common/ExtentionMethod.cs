using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GloboWeather.WeatherManagement.Application.Helpers.Common
{
    public static class ExtentionMethod
    {
        public static int EnumToInt<TValue>(this TValue value) where TValue : struct, IConvertible
        {
            if (!typeof(TValue).IsEnum)
            {
                throw new ArgumentException(nameof(value));
            }

            return (int)(object)value;
        }

        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
        {
            try
            {
                return dictionary[key];
            }
            catch (Exception ex)
            {
                return default(TValue);
            }
        }

        public static string GetString(this object text, string defaultValue = "", bool isLower = false, bool isTrim = false)
        {
            if (text == null)
                return defaultValue;
            else
            {
                string result = isTrim ? text.ToString().Trim() : text.ToString();
                return isLower ? result.ToLower() : result;
            }
        }

        public static bool GetBool(this object value, bool defaultValue = false)
        {
            bool result = defaultValue;

            bool.TryParse(value.GetString(), out result);

            return result;
        }

        public static int GetInt(this object value, int defaultValue = 0)
        {
            int result = defaultValue;

            int.TryParse(value.GetString(), out result);

            return result;
        }

        public static int? GetIntNull(this object value)
        {
            if (int.TryParse(value.GetString(), out int result))
                return result;
            return null;
        }

        public static float GetFloat(this object value, float defaultValue = 0)
        {
            float result = defaultValue;

            float.TryParse(value.GetString(), out result);

            return result;
        }

        public static decimal GetDecimal(this object value, decimal defaultValue = 0, int? roundNumber = null)
        {
            decimal result = defaultValue;

            decimal.TryParse(value.GetString(), out result);

            if (roundNumber.HasValue)
                result = Math.Round(result, roundNumber.Value, MidpointRounding.AwayFromZero);
            return result;
        }

        public static decimal? GetDecimalNull(this object value, int? roundNumber = null)
        {
            if (!decimal.TryParse(value.GetString(), out decimal result))
                return null;
            if (roundNumber.HasValue)
                result = Math.Round(result, roundNumber.Value, MidpointRounding.AwayFromZero);
            return result;
        }

        public static bool IsNumber(this object value)
        {
            float result = 0;

            return float.TryParse(value.GetString(), out result);
        }

        public static DateTime? GetDate(this object value, DateTime? defaultValue = null)
        {
            DateTime result = DateTime.Now;

            if (!defaultValue.HasValue)
                defaultValue = DateTime.Now;

            if (DateTime.TryParse(value.GetString(), out result))
            {
                if (result == DateTime.MinValue)
                    return defaultValue;
                return result;
            }
            else
            {
                return defaultValue;
            }
        }

        public static DateTime GetStartOfDate(this object value, DateTime? defaultValue = null)
        {
            return ((DateTime)GetDate(value, defaultValue)).Date;
        }

        public static DateTime GetEndOfDate(this object value, DateTime? defaultValue = null)
        {
            return GetStartOfDate(value, defaultValue).AddDays(1).AddSeconds(-1);
        }

        public static string StandardizedString(this object text)
        {
            if (text == null)
                return string.Empty;
            else
            {
                return text.ToString().Trim().Replace("  ", " ");
            }
        }

        public static TValue Get<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key)
        {
            TValue value;

            if (source.TryGetValue(key, out value))
            {
                return value;
            }

            return default(TValue);
        }

    }
}