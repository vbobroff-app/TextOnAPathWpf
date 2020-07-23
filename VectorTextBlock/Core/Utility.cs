using System;
using System.Globalization;
using System.Windows;

namespace VectorTextBlock
{
    /// <summary>
    /// Helper class with Extensions Methods
    /// </summary>
    internal static class Utility
    {
        /// <summary>
        /// Compare object values that may be null
        /// </summary>
        /// <param name="value"></param>
        /// <param name="eqValue"></param>
        /// <returns></returns>
        public static bool IsEqual(this object value, object eqValue)
        {
            return value?.Equals(eqValue) ?? eqValue == null;
        }

        public static bool IsEqual(this double value, double eqValue, double precision = 1E-12)
        {
            return Math.Abs(value - eqValue) < precision;
        }

        /// <summary>
        /// Convert string to double according culture symbols
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this string value)
        {
            // Try parsing in the current culture
            if (
                !double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out var result) &&
                // Then try in US english
                !double.TryParse(value, NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"),
                    out result) &&
                // Then in neutral language
                !double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                throw new ArgumentException();
            }

            return result;

        }

        /// <summary>
        /// Dynamic actual Height of FrameworkElement
        /// </summary>
        /// <param name="fe"></param>
        /// <returns></returns>
        public static double RealHeight(this FrameworkElement fe)
        {
            return double.IsNaN(fe.Height) ? fe.ActualHeight : fe.Height;
        }

        /// <summary>
        /// Dynamic actual Width of FrameworkElement
        /// </summary>
        /// <param name="fe"></param>
        /// <returns></returns>
        public static double RealWidth(this FrameworkElement fe)
        {
            return double.IsNaN(fe.Width) ? fe.ActualWidth : fe.Width;
        }
    }
}
